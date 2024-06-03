using InvoiceCreatorApp.Models;
using InvoiceCreatorApp.MVVM;
using System.Collections.ObjectModel;
using System.Windows;

namespace InvoiceCreatorApp.ViewModels
{
    public class MonthlyBalanceViewModel : ViewModelBase
    {
        public RelayCommand OpenInvoiceCommand => new RelayCommand(execute => CloseMonthlyBalance(execute), canExecute => CanCloseBalance());

        private bool CanCloseBalance()
        {
            return true;
        }

        private void CloseMonthlyBalance(object obj)
        {
            Window window = obj as Window;
            if (window != null)
            {
                window.Close();
            }
        }

        private ObservableCollection<FinalInvoice> _invoices;

        public ObservableCollection<FinalInvoice> Invoices
        {
            get { return _invoices; }
            set { _invoices = value; OnPropertyChanged(); }
        }

        public MonthlyBalanceViewModel()
        {
            LoadInvoices();
        }

        private void LoadInvoices()
        {
            var invoices = InvoiceData.GetInvoices();
            Invoices = new ObservableCollection<FinalInvoice>(invoices);
        }
    }
}
