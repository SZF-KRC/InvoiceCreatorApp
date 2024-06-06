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

        // Listen zum Zählen aller Einnahmen und Ausgaben
        List<double> _Incomes = new List<double>();
        List<double> _Expenses = new List<double>();

        // Anfangsdatum und Enddatum
        private DateTime _startDate = DateTime.Today.AddDays(-120);
        private DateTime _endDate = DateTime.Today;
        private DateTime _startDateSelected = DateTime.Today.AddDays(-120);
        private DateTime _endDateSelected = DateTime.Today;

        private double _finalBalance;
        private double _expense;
        private double _income;

        /// <summary>
        /// Befehl zum Schließen des Fensters für die monatliche Bilanz
        /// </summary>
        public RelayCommand OpenInvoiceCommand => new RelayCommand(execute => CloseMonthlyBalance(execute), canExecute => CanCloseBalance());

        /// <summary>
        /// Überprüft, ob die monatliche Bilanz geschlossen werden kann
        /// </summary>
        private bool CanCloseBalance()
        {
            return true;
        }

        /// <summary>
        /// Schließt das Fenster für die monatliche Bilanz
        /// </summary>
        private void CloseMonthlyBalance(object obj)
        {
            Window window = obj as Window;
            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Ausgewähltes Startdatum für den Bericht
        /// </summary>
        public DateTime StartDateSelected
        {
            get => _startDateSelected;
            set { _startDateSelected = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Ausgewähltes Enddatum für den Bericht
        /// </summary>
        public DateTime EndDateSelected
        {
            get =>_endDateSelected;
            set { _endDateSelected = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Anfangsdatum des Berichtszeitraums
        /// </summary>
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

        /// <summary>
        /// Enddatum des Berichtszeitraums
        /// </summary>
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

        /// <summary>
        /// Gesamteinnahmen im ausgewählten Zeitraum
        /// </summary>
        public double Income
        {
            get => _income;
            set { _income = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gesamtausgaben im ausgewählten Zeitraum
        /// </summary>
        public double Expense
        {
            get => _expense;
            set { _expense = value; OnPropertyChanged();}
        }

        /// <summary>
        /// Endgültiger Saldo im ausgewählten Zeitraum
        /// </summary>
        public double FinalBalance
        {
            get => _finalBalance;
            set { _finalBalance = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Angezeigte Rechnungen im ausgewählten Zeitraum
        /// </summary>
        public ObservableCollection<Invoice> Invoices 
        {
            get => _displayedInvoices;
            set { _displayedInvoices = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Angezeigte Ausgaben im ausgewählten Zeitraum
        /// </summary>
        public ObservableCollection<Expense> Expenses
        {
            get => _displayedExpenses;
            set { _displayedExpenses = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Konstruktor für MonthlyBalanceViewModel
        /// </summary>
        public MonthlyBalanceViewModel()
        {
            // Initialisiert die angezeigten Rechnungen mit allen Rechnungen
            _displayedInvoices = new ObservableCollection<Invoice>(_invoices);
            _displayedExpenses = new ObservableCollection<Expense>(_expenses);

            // Abonniert Änderungen an Start- und Enddatum, um die angezeigten Rechnungen zu aktualisieren
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(StartDateSelected) || args.PropertyName == nameof(EndDateSelected))
                {
                    UpdateDisplayedInvoices();
                }
            };

            UpdateDisplayedInvoices();
        }

        /// <summary>
        /// Aktualisiert die angezeigten Rechnungen basierend auf dem ausgewählten Datum
        /// </summary>
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

        /// <summary>
        /// Berechnet die gesamten Einnahmen
        /// </summary>
        /// <returns>Summe der Einnahmen</returns>
        private double UpdateIncome() { return _Incomes.Sum(); }

        /// <summary>
        /// Berechnet die gesamten Ausgaben
        /// </summary>
        /// <returns>Summe der Ausgaben</returns>
        private double UpdateExpense() {  return _Expenses.Sum(); }

        /// <summary>
        /// Berechnet den endgültigen Saldo
        /// </summary>
        /// <returns>Endgültiger Saldo</returns>
        private double UpdateFinalBalance()
        {
            double balance = _Incomes.Sum() - _Expenses.Sum();
            return Math.Round(balance, 2);
        }


    }
}
