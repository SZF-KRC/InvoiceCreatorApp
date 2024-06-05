using InvoiceCreatorApp.Models;
using InvoiceCreatorApp.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace InvoiceCreatorApp.ViewModels
{
    public class MonthlyBalanceViewModel : ViewModelBase
    {
        private ObservableCollection<Invoice> _invoices = new ObservableCollection<Invoice>(InvoiceData.GetInvoices());
        private ObservableCollection<Expense> _expenses = new ObservableCollection<Expense> (ExampleExpenseData.GetExpenses());
        private ObservableCollection<Invoice> _displayedInvoices;
        private ObservableCollection<Expense> _displayedExpenses;
       
        // lists for count all incomes and expenses
        List<double> _Incomes = new List<double>();
        List<double> _Expenses = new List<double>();

        // initial dates
        private DateTime _startDate = DateTime.Today.AddDays(-120);
        private DateTime _endDate = DateTime.Today;
        private DateTime _startDateSelected = DateTime.Today.AddDays(-120);
        private DateTime _endDateSelected = DateTime.Today;

        private double _finalBalance;
        private double _expense;
        private double _income;

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

        public DateTime StartDateSelected
        {
            get => _startDateSelected;
            set { _startDateSelected = value; OnPropertyChanged(); }
        }

        public DateTime EndDateSelected
        {
            get =>_endDateSelected;
            set { _endDateSelected = value; OnPropertyChanged(); }
        }
       
        public DateTime StartDate 
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

        public DateTime EndDate
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

        
        public double Income
        {
            get => _income;
            set { _income = value; OnPropertyChanged(); }
        }
       
        public double Expense
        {
            get => _expense;
            set { _expense = value; OnPropertyChanged();}
        }

       
        public double FinalBalance
        {
            get => _finalBalance;
            set { _finalBalance = value; OnPropertyChanged(); }
        }      

        public ObservableCollection<Invoice> Invoices 
        {
            get => _displayedInvoices;
            set { _displayedInvoices = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Expense> Expenses
        {
            get => _displayedExpenses;
            set { _displayedExpenses = value; OnPropertyChanged(); }
        }

        public MonthlyBalanceViewModel()
        {
            // Initialize displayed invoices with all invoices initially
            _displayedInvoices = new ObservableCollection<Invoice>(_invoices);
            _displayedExpenses = new ObservableCollection<Expense>(_expenses);

            // Subscribe to changes in start and end dates to update displayed invoices
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(StartDateSelected) || args.PropertyName == nameof(EndDateSelected))
                {
                    UpdateDisplayedInvoices();
                }
            };

            UpdateDisplayedInvoices();
        }

        private void UpdateDisplayedInvoices()
        {
            _displayedInvoices.Clear();
            _displayedExpenses.Clear();
            _Incomes.Clear();
            _Expenses.Clear();

            foreach (var invoice in _invoices)
            {
                if (invoice.DateOfIssue >= _startDateSelected && invoice.DateOfIssue <= _endDateSelected)
                {
                    _displayedInvoices.Add(invoice);
                    _Incomes.Add(invoice.Total);
                }
            }

            foreach(var expense in _expenses)
            {
                if(expense.IssueDate >= _startDateSelected && expense.IssueDate <= _endDateSelected)
                {
                    _displayedExpenses.Add(expense);
                    _Expenses.Add(expense.Total);
                }
            }
            Income = UpdateIncome();
            Expense = UpdateExpense();
            FinalBalance = UpdateFinalBalance();
        }

        private double UpdateIncome() { return _Incomes.Sum(); }

        private double UpdateExpense() {  return _Expenses.Sum(); }

        private double UpdateFinalBalance()
        {
            double balance = _Incomes.Sum() - _Expenses.Sum();
            return Math.Round(balance, 2);
        }


    }
}
