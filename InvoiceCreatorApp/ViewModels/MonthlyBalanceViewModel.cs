using InvoiceCreatorApp.Models;
using InvoiceCreatorApp.MVVM;
using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Windows;

namespace InvoiceCreatorApp.ViewModels
{
    public class MonthlyBalanceViewModel : ViewModelBase
    {
        private ObservableCollection<Invoice> _invoices = new ObservableCollection<Invoice>(InvoiceData.GetInvoices());
        

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
     
        public ObservableCollection<Invoice> Invoices
        {
            get => _invoices; 
            set { _invoices = value; OnPropertyChanged(); }
        }


        private DateTime _startDate ;
        private DateTime _endDate = DateTime.Today ;
        public DateTime SelectedDateTimeStartDate 
        {
            get => _startDate; 
            set 
            { 
                if( value <= _endDate )
                {
                    _startDate = value;
                }
                else
                {
                    _endDate =value;
                }
               OnPropertyChanged();
                
            }
        }

        public DateTime SelectedDateTimeEndDate
        {
            get => _endDate;
            set
            {
                if( value >= _startDate)
                {
                    _endDate = value;
                }else
                {
                    _startDate = value;
                }
                OnPropertyChanged();
            }
        }
    }
}
