using System;
using System.Drawing.Printing;

using ReactiveUI;

namespace Movere.ViewModels
{
    internal sealed class PrinterSettingsViewModel : ReactiveObject
    {
        public PrinterSettingsViewModel(PrinterSettings printerSettings)
        {
            PrinterSettings = printerSettings;
        }

        public PrinterSettings PrinterSettings { get; }

        public string? PrinterName
        {
            get => PrinterSettings.PrinterName;
            set => UpdatePrinterSettings(() => PrinterSettings.PrinterName = value);
        }

        public short Copies
        {
            get => PrinterSettings.Copies;
            set => UpdatePrinterSettings(() => PrinterSettings.Copies = value);
        }

        public int MaximumCopies => PrinterSettings.MaximumCopies;

        private void UpdatePrinterSettings(Action update)
        {
            update();
            this.RaisePropertyChanged(String.Empty);
        }
    }
}
