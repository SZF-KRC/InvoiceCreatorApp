using InvoiceCreatorApp.Models;
using InvoiceCreatorApp.MVVM;
using InvoiceCreatorApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;

namespace InvoiceCreatorApp.ViewModels
{
    public class InvoiceViewModel : ViewModelBase
    {
        Regex isdouble = new Regex(@"^-?\d+(\,\d+)?$");
        Regex isStreet = new Regex(@"^[A-Za-z]+(?:\s[A-Za-z0-9'_-]+)+$");
        public ObservableCollection<Invoice> oneInvoice { get; set; }
       


        private string _companyName = "WPF Bau GesmbH";
        private string _companyCity = "Guisberg";
        private string _companyStreet = "Fensterstraße 12";
        private string _companyPostalCode = "8788";


        private string _customerNumber;
        private string _customerName;
        private string _customerCity;
        private string _customerStreet;
        private string _customerPostalCode;


        private string _descriptionOfGoods;
        private string _numberOfGoods;
        private string _pricePerPiece;

        private double _totalTax;
        private double _finalPrice;
        private double _totalPrice;

        private Invoice _selectedItem;


        /// <summary>
        /// Befehl zum Hinzufügen eines Postens zur Rechnung
        /// </summary>
        public RelayCommand AddCommand => new RelayCommand(execute => AddOneItemToInvoice(), canExecute => CanAddInvoice());

        /// <summary>
        /// Befehl zum Aktualisieren eines Postens in der Rechnung
        /// </summary>
        public RelayCommand UpdateCommand => new RelayCommand(execute => UpdateOneItemInInvoice(), canExecute => CanUpdateInvoice());

        /// <summary>
        /// Befehl zum Löschen eines ausgewählten Postens aus der Rechnung
        /// </summary>
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteOneItemInInvoice(), canExecute => SelectedItem != null);

        /// <summary>
        /// Befehl zum Speichern der Rechnung
        /// </summary>
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveInvoice(), canExecute => CanSaveInvoice());


        public RelayCommand MonthlyBalanceCommand => new RelayCommand(execute => MonthlyBalanceSheet(), canExecute => CanMonthlyBalance());
      
        /// <summary>
        /// Konstruktor für MainWindowViewModel
        /// Initialisiert die Sammlung von Rechnungen
        /// </summary>
        public InvoiceViewModel()
        {
            oneInvoice = new ObservableCollection<Invoice>();
        }

        /// <summary>
        /// Firmenname der Rechnung
        /// </summary>
        public string CompanyName
        {
            get => _companyName;
            set{_companyName = value; OnPropertyChanged();}
        }

        /// <summary>
        /// Straße des Unternehmens
        /// </summary>
        public string CompanyStreet
        {
            get => _companyStreet;
            set  {_companyStreet = value; OnPropertyChanged();}
        }

        /// <summary>
        /// Stadt des Unternehmens
        /// </summary>
        public string CompanyCity
        {
            get => _companyCity;
            set { _companyCity = value; OnPropertyChanged(); } 
        }

        /// <summary>
        /// Postleitzahl des Unternehmens
        /// </summary>
        public string CompanyPostalCode
        {
            get => _companyPostalCode;
            set{ _companyPostalCode = value; OnPropertyChanged(); } 
        }

        /// <summary>
        /// Kundennummer der Rechnung
        /// </summary>
        public string CustomerNumber
        {
            get => _customerNumber;
            set{_customerNumber = value;OnPropertyChanged();}
        }

        /// <summary>
        /// Kundenname der Rechnung
        /// </summary>
        public string CustomerName
        {
            get => _customerName;
            set{_customerName = value;OnPropertyChanged();}
        }

        /// <summary>
        /// Straße des Kunden
        /// </summary
        public string CustomerStreet
        {
            get => _customerStreet;
            set { _customerStreet = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Stadt des Kunden
        /// </summary>
        public string CustomerCity
        {
            get => _customerCity;
            set { _customerCity = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Postleitzahl des Kunden
        /// </summary>
        public string CustomerPostalCode
        {
            get =>_customerPostalCode;
            set { _customerPostalCode = value; OnPropertyChanged();} 
        }

        /// <summary>
        /// Warenbeschreibung der Rechnung
        /// </summary>
        public string DescriptionOfGoods
        {
            get => _descriptionOfGoods;
            set{ _descriptionOfGoods = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Anzahl der Waren in der Rechnung
        /// </summary>
        public string NumberOfGoods
        {
            get => _numberOfGoods;
            set{ _numberOfGoods = value;OnPropertyChanged();}
        }

        /// <summary>
        /// Preis pro Stück in der Rechnung
        /// </summary>
        public string PricePerPiece
        {
            get => _pricePerPiece;
            set{_pricePerPiece = value; OnPropertyChanged();}
        }

        /// <summary>
        /// Gesamte Steuer der Rechnung
        /// </summary>
        public double TotalTax
        {
            get => _totalTax;
            set{_totalTax = value; OnPropertyChanged();}
        }

        /// <summary>
        /// Gesamtpreis der Rechnung
        /// </summary>
        public double TotalPrice
        {
            get { return _totalPrice; }
            set{_totalPrice = value;OnPropertyChanged();}
        }

        /// <summary>
        /// Endpreis der Rechnung
        /// </summary>
        public double FinalPrice
        {
            get => _finalPrice;
            set{_finalPrice = value;OnPropertyChanged();}
        }

        /// <summary>
        /// Fügt einen neuen Posten zur Rechnung hinzu
        /// </summary>
        private void AddOneItemToInvoice()
        {
            // Erstellt einen neuen Posten und setzt die Eigenschaften
            var invoice = new Invoice
            {
                CompanyName = CompanyName,
                CompanyStreet = CompanyStreet,
                CompanyCity = CompanyCity,
                CompanyPostalCode = CompanyPostalCode,

                CustomerName = CustomerName,
                CustomerNumber = CustomerNumber,
                CustomerStreet = CustomerStreet,
                CustomerCity = CustomerCity,
                CustomerPostalCode = CustomerPostalCode,

                Description = DescriptionOfGoods,
                NumberOfGoods = NumberOfGoods,
                PricePerPiece = PricePerPiece,
                Position = oneInvoice.Count + 1,
            };
            oneInvoice.Add(invoice);// Fügt den neuen Posten zur Rechnungssammlung hinzu
            UpdateInvoiceTotals();// Aktualisiert die Gesamtsummen der Rechnung

            // Setzt die Eingabefelder zurück
            DescriptionOfGoods = string.Empty;
            NumberOfGoods = string.Empty;
            PricePerPiece = string.Empty;
            OnPropertyChanged();
        }

        /// <summary>
        /// Aktualisiert einen Posten in der Rechnung und die Gesamtsummen
        /// </summary>
        private void UpdateOneItemInInvoice()
        {
            if (SelectedItem != null)
            {
                // Aktualisiert die Eigenschaften des ausgewählten Postens
                SelectedItem.CustomerName = CustomerName;
                SelectedItem.CustomerStreet = CustomerStreet;
                SelectedItem.CustomerCity = CustomerCity;
                SelectedItem.CustomerPostalCode = CustomerPostalCode;
                
                SelectedItem.Description = DescriptionOfGoods;
                SelectedItem.NumberOfGoods = NumberOfGoods;
                SelectedItem.PricePerPiece = PricePerPiece;

                // Benachrichtigt die Benutzeroberfläche über die Änderungen in der Rechnungssammlung
                OnPropertyChanged(nameof(oneInvoice));
                SelectedItem = null;// Setzt den ausgewählten Posten zurück
                UpdateInvoiceTotals();     // Aktualisiert die Gesamtsummen der Rechnung  

                // Setzt die Eingabefelder zurück
                DescriptionOfGoods = string.Empty;
                NumberOfGoods = string.Empty;
                PricePerPiece = string.Empty;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Löscht den ausgewählten Posten aus der Rechnung
        /// </summary>
        private void DeleteOneItemInInvoice()
        {
            oneInvoice.Remove(SelectedItem);// Entfernt den ausgewählten Posten aus der Rechnungssammlung
            UpdateInvoiceTotals();// Aktualisiert die Gesamtsummen der Rechnung
            UpdatePositions();// Aktualisiert die Positionen der verbleibenden Posten
            OnPropertyChanged(); // Benachrichtigt die Benutzeroberfläche über die Änderungen
        }

        /// <summary>
        /// Speichert die Rechnung und aktualisiert die Gesamtsummen
        /// </summary>
        private void SaveInvoice()
        {
            string invoiceNumber = Guid.NewGuid().ToString();
            string currentTime = DateTime.Now.ToString("dd.MM.yyyy"); //CultureInfo.InvariantCulture
            InvoiceDocumentCreator saveDocument = new InvoiceDocumentCreator();// Erstellt ein neues Dokument für die Rechnung
            saveDocument.SaveInvoice(oneInvoice, invoiceNumber,currentTime, CustomerName, CustomerNumber, CustomerStreet, CustomerCity, CustomerPostalCode, CompanyName, CompanyStreet, CompanyCity, CompanyPostalCode);// Speichert das Rechnungsdokument

            var finalInvoice = new FinalInvoice
            {
                CustomerName = CustomerName,
                CustomerNum = CustomerNumber,
                InvoiceNum = invoiceNumber,
                CurrentTime = currentTime,
                Tax = _totalTax,
                Netto = _totalPrice,
                FinalPrice = _finalPrice,
                Items = new ObservableCollection<Invoice>(oneInvoice)
            };

            InvoiceData.AddInvoice(finalInvoice);

            oneInvoice.Clear();// Löscht alle Posten aus der aktuellen Rechnungssammlung
            UpdateInvoiceTotals();// Aktualisiert die Gesamtsummen der Rechnung
        }

        /// <summary>
        /// Überprüft, ob ein Posten zur Rechnung hinzugefügt werden kann
        /// </summary>
        private bool CanAddInvoice()
        {
            return !string.IsNullOrWhiteSpace(CustomerName) &&
                   CustomerName.Length > 3 &&

                   !string.IsNullOrWhiteSpace(CustomerStreet) &&
                   CustomerStreet.Length > 3 &&
                   isStreet.IsMatch(CustomerStreet) &&

                   !string.IsNullOrWhiteSpace(CustomerCity) &&
                   CustomerCity.Length > 3 &&
                   CustomerCity.All(char.IsLetter) &&

                   !string.IsNullOrWhiteSpace(CustomerPostalCode) &&
                   CustomerPostalCode.Length > 3 &&
                   CustomerPostalCode.All(char.IsDigit) &&

                  !string.IsNullOrWhiteSpace(CustomerNumber) &&
                  CustomerNumber.Length > 3 &&
                  CustomerNumber.All(char.IsDigit) &&

                  !string.IsNullOrWhiteSpace(DescriptionOfGoods) &&
                  DescriptionOfGoods.Length > 2 &&

                  !string.IsNullOrWhiteSpace(NumberOfGoods) &&
                  NumberOfGoods.Length > 0 &&
                  NumberOfGoods.Length < 3 &&
                  NumberOfGoods.All(char.IsDigit) &&

                  !string.IsNullOrWhiteSpace(PricePerPiece) &&
                  PricePerPiece.Length > 0 &&
                  PricePerPiece.Length < 9 &&
                  isdouble.IsMatch(PricePerPiece);
        }

        /// <summary>
        /// Überprüft, ob die Rechnung gespeichert werden kann
        /// </summary>
        private bool CanSaveInvoice()
        {
            // Überprüft, ob die notwendigen Felder ausgefüllt sind und ob es Posten in der Rechnung gibt
            return !string.IsNullOrWhiteSpace(CustomerName) &&
                   CustomerName.Length > 3 &&

                   !string.IsNullOrWhiteSpace(CustomerStreet) &&
                   CustomerStreet.Length > 3 &&
                   isStreet.IsMatch(CustomerStreet) &&

                   !string.IsNullOrWhiteSpace(CustomerCity) &&
                   CustomerCity.Length > 3 &&
                   CustomerCity.All(char.IsLetter) &&

                   !string.IsNullOrWhiteSpace(CustomerPostalCode) &&
                   CustomerPostalCode.Length > 3 &&
                   CustomerPostalCode.All(char.IsDigit) &&

                  !string.IsNullOrWhiteSpace(CustomerNumber) &&
                  CustomerNumber.Length > 3 &&
                  CustomerNumber.All(char.IsDigit)
                  && oneInvoice.Count != 0;
        }

        /// <summary>
        /// Überprüft, ob die Rechnung aktualisiert werden kann
        /// </summary>
        private bool CanUpdateInvoice()
        {
            return CanAddInvoice() && SelectedItem != null;
        }

        private bool CanMonthlyBalance()
        {

            return InvoiceData.GetInvoices().Count > 0;
        }

        /// <summary>
        /// Ausgewähltes Rechnungsobjekt
        /// </summary>
        public Invoice SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();

                if (_selectedItem != null)
                {
                    DescriptionOfGoods = _selectedItem.Description;
                    NumberOfGoods = _selectedItem.NumberOfGoods;
                    PricePerPiece = _selectedItem.PricePerPiece;
                    CustomerName = _selectedItem.CustomerName;
                    CustomerNumber = _selectedItem.CustomerNumber;
                    CustomerStreet = _selectedItem.CustomerStreet;
                    CustomerCity = _selectedItem.CustomerCity;
                    CustomerPostalCode = _selectedItem.CustomerPostalCode;
                }
            }
        }

        private void MonthlyBalanceSheet()
        {
            MonthlyBalanceView monthlyBalanceView = new MonthlyBalanceView();
            monthlyBalanceView.DataContext = new MonthlyBalanceViewModel();
            monthlyBalanceView.Show();
        }

       

        /// <summary>
        /// Aktualisiert die Gesamtsummen der Rechnung
        /// </summary>
        private void UpdateInvoiceTotals()
        {
            // Berechnet die Gesamtsummen der Steuern, des Endpreises und des Gesamtpreises
            TotalTax = oneInvoice.Sum(i => i.CalculationTax());
            FinalPrice = oneInvoice.Sum(i => i.CalculationFinalPrice());
            TotalPrice = oneInvoice.Sum(i => i.TotalPrice());
        }

        /// <summary>
        /// Aktualisiert die Positionen der Posten in der Rechnung
        /// </summary>
        private void UpdatePositions()
        {
            // Setzt die Positionen der Posten basierend auf ihrer Reihenfolge in der Sammlung
            for (int i = 0; i < oneInvoice.Count; i++)
            {
                oneInvoice[i].Position = i + 1;
            }
        }
    }
}
