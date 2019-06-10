using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

using ReactiveUI;

namespace Movere.ViewModels
{
    internal sealed class PrintDialogViewModel : ReactiveObject
    {
        private readonly PrintDocument _document;
        private readonly PreviewPrintController _controller;

        private readonly Action<bool> _closeAction;

        private IReadOnlyList<PrintPreviewPageViewModel> _printPreviewPages;

        public PrintDialogViewModel(PrintDocument document, Action<bool> closeAction)
        {
            _document = document;
            _closeAction = closeAction;

            _controller = new PreviewPrintController();
            _document.PrintController = _controller;

            PrinterSettings = new PrinterSettingsViewModel((PrinterSettings)_document.PrinterSettings.Clone());

            PrinterSettings
                .WhenAnyValue(vm => vm.PrinterSettings)
                .Subscribe(UpdatePrintPreview);

            PrintCommand = ReactiveCommand.Create(
                Print,
                this.ObservableForProperty(vm => vm.PrinterSettings.PrinterName).Select(x => x != null));

            CancelCommand = ReactiveCommand.Create(Cancel);

            var printers = new string[System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count];
            System.Drawing.Printing.PrinterSettings.InstalledPrinters.CopyTo(printers, 0);

            AvailablePrinters = printers;
        }

        public ICommand PrintCommand { get; }

        public ICommand CancelCommand { get; }

        public PrinterSettingsViewModel PrinterSettings { get; }

        public IReadOnlyList<string> AvailablePrinters { get; }

        public IReadOnlyList<PrintPreviewPageViewModel> PrintPreviewPages
        {
            get => _printPreviewPages;
            set => this.RaiseAndSetIfChanged(ref _printPreviewPages, value);
        }

        private void Print()
        {
            _document.PrinterSettings = PrinterSettings.PrinterSettings;
            Close(true);
        }

        private void Cancel() => Close(false);

        private void Close(bool result)
        {
            _document.PrintController = new StandardPrintController();
            _closeAction(result);
        }

        private void UpdatePrintPreview(PrinterSettings printerSettings)
        {
            _document.Print();

            var previewPageInfos = _controller.GetPreviewPageInfo();
            var printPreviewPages = new List<PrintPreviewPageViewModel>(previewPageInfos.Length);

            foreach (var info in previewPageInfos)
            {
                printPreviewPages.Add(new PrintPreviewPageViewModel(info));
            }

            PrintPreviewPages = printPreviewPages;
        }
    }
}
