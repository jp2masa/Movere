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
            set => UpdatePrinterSettings((ps, v) => ps.PrinterName = v, value);
        }

        public short Copies
        {
            get => PrinterSettings.Copies;
            set => UpdatePrinterSettings((ps, v) => ps.Copies = v, value);
        }

        public int MaximumCopies => PrinterSettings.MaximumCopies;

        public bool Collate
        {
            get => PrinterSettings.Collate;
            set => UpdatePrinterSettings((ps, v) => ps.Collate = v, value);
        }

        private void UpdatePrinterSettings<T>(Action<PrinterSettings, T> update, T value)
        {
            update(PrinterSettings, value);
            this.RaisePropertyChanged(String.Empty);
        }
    }
}
