using InvoiceCreatorApp.Models;
using InvoiceCreatorApp.MVVM;
using InvoiceCreatorApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvoiceCreatorApp.ViewModels
{
    public class InvoiceViewModel : ViewModelBase
    {
        Regex isdouble = new Regex(@"^-?\d+(\,\d+)?$");
        Regex isStreet = new Regex(@"^[A-Za-z]+(?:\s[A-Za-z0-9'_-]+)+$");


        private Customer _customer = new Customer();
        private Company _company = new Company();

        private ObservableCollection<Product> _products = new ObservableCollection<Product>();
        private Product _newProduct = new Product();

        //private double _totalTax;
        //private double _finalPrice;
        //private double _totalPrice;

        private Product _selectedItem;


        /// <summary>
        /// Befehl zum Hinzufügen eines Postens zur Rechnung
        /// </summary>
        public RelayCommand AddCommand => new RelayCommand(execute => AddProduct(), canExecute => CanAddProduct());

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


        public Company Company
        {
            get => _company;
            set { _company = value; OnPropertyChanged(); }
        }

        public Customer Customer
        {
            get => _customer;
            set { _customer = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(); }
        }

        public Product NewProduct
        {
            get => _newProduct;
            set { _newProduct = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Ausgewähltes Rechnungsobjekt
        /// </summary>
        public Product SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                if (_selectedItem != null)
                {
                    NewProduct.Description = _selectedItem.Description;
                    NewProduct.Quantity = _selectedItem.Quantity;
                    NewProduct.PricePerUnit = _selectedItem.PricePerUnit;
                }
            }
        }

        public double SubTotal => Products.Sum(price => price.TotalPrice);
        public double VAT => SubTotal * 0.2;
        public double Total => SubTotal + VAT;


        private void AddProduct()
        {
            Products.Add(new Product
            {
                Description = NewProduct.Description,
                Quantity = NewProduct.Quantity,
                PricePerUnit = NewProduct.PricePerUnit
            });

            UpdateInvoiceTotals();
            NewProduct = new Product();

            // Setzt die Eingabefelder zurück
            ClearWindowItem();
            OnPropertyChanged();
            UpdatePositions();

        }



        /// <summary>
        /// Aktualisiert einen Posten in der Rechnung und die Gesamtsummen
        /// </summary>
        private void UpdateOneItemInInvoice()
        {
            if (SelectedItem != null)
            {
                // Aktualisiert die Eigenschaften des ausgewählten Postens



                SelectedItem.Description = NewProduct.Description;
                SelectedItem.Quantity = NewProduct.Quantity;
                SelectedItem.PricePerUnit = NewProduct.PricePerUnit;

                // Benachrichtigt die Benutzeroberfläche über die Änderungen in der Rechnungssammlung
                OnPropertyChanged(nameof(Products));
                SelectedItem = null;// Setzt den ausgewählten Posten zurück
                UpdateInvoiceTotals();     // Aktualisiert die Gesamtsummen der Rechnung  

                // Setzt die Eingabefelder zurück
                ClearWindowItem();
                OnPropertyChanged();
                UpdatePositions();
            }
        }

        /// <summary>
        /// Löscht den ausgewählten Posten aus der Rechnung
        /// </summary>
        private void DeleteOneItemInInvoice()
        {

            Products.Remove(SelectedItem);

            UpdateInvoiceTotals();// Aktualisiert die Gesamtsummen der Rechnung
            UpdatePositions();// Aktualisiert die Positionen der verbleibenden Posten
            ClearWindowItem();
            OnPropertyChanged(); // Benachrichtigt die Benutzeroberfläche über die Änderungen
        }

        /// <summary>
        /// Speichert die Rechnung und aktualisiert die Gesamtsummen
        /// </summary>
        private void SaveInvoice()
        {
            Random random = new Random();
            string invoiceNumber = random.Next(1000, 9999).ToString();
            DateTime issueDate = DateTime.Today.Date.AddDays(-random.Next(1, 120));
            DateTime ddd = DateTime.Now.Date.Date;

            Invoice invoice = new Invoice()
            {             
                Company = new Company
                {
                    CompanyName = Company.CompanyName,
                    CompanyStreet = Company.CompanyStreet,
                    CompanyCity = Company.CompanyCity,
                    CompanyPostCode = Company.CompanyPostCode,
                },
                Customer = new Customer
                {
                    CustomerName = Customer.CustomerName,
                    CustomerNumber = Customer.CustomerNumber,
                    CustomerStreet = Customer.CustomerStreet,
                    CustomerCity = Customer.CustomerCity,
                    CustomerPostCode = Customer.CustomerPostCode
                },
                Products = Products.ToList(),
                DateOfIssue = issueDate,
                InvoiceNumber = invoiceNumber,
                
            };

            InvoiceDocumentCreator saveDocument = new InvoiceDocumentCreator();// Erstellt ein neues Dokument für die Rechnung
            saveDocument.SaveInvoice(invoice, Products);// Speichert das Rechnungsdokument
            InvoiceData.AddInvoice(invoice);
            

            Products.Clear();// Löscht alle Posten aus der aktuellen Rechnungssammlung
            ClearWinodowAll();
            UpdateInvoiceTotals();// Aktualisiert die Gesamtsummen der Rechnung
        }

        private void RandomExpenses()
        {
            Random random = new Random();
            int count = 20;
            for (int i = 0; i < count; i++)
            {
                // zufällige Werte für Ausgaben
                string expenseNumber = $"{i + 1000}";
                DateTime issueDate = DateTime.Today.Date.AddDays(-random.Next(1, 120));
                string companyName = $"Company {i + 1}";
                double netto = random.Next(0, 1001);
                double vat = netto * 0.2; // 20% Tax
                double total = netto + vat;

                Expense expense = new Expense()
                {
                    CompanyName = companyName,
                    IssueDate = issueDate,
                    ExpenseNumber = expenseNumber,
                    VAT = vat,
                    Total = total,
                    Netto = netto,
                };
                ExampleExpenseData.AddRandomExpenses(expense);
            }
        }

        /// <summary>
        /// Überprüft, ob ein Posten zur Rechnung hinzugefügt werden kann
        /// </summary>
        private bool CanAddProduct()
        {
            return !string.IsNullOrWhiteSpace(Customer.CustomerName) &&
                   Customer.CustomerName.Length > 3 &&

                   !string.IsNullOrWhiteSpace(Customer.CustomerStreet) &&
                   Customer.CustomerStreet.Length > 3 &&
                   isStreet.IsMatch(Customer.CustomerStreet) &&

                   !string.IsNullOrWhiteSpace(Customer.CustomerCity) &&
                   Customer.CustomerCity.Length > 3 &&
                   Customer.CustomerCity.All(char.IsLetter) &&

                   !string.IsNullOrWhiteSpace(Customer.CustomerPostCode) &&
                   Customer.CustomerPostCode.Length > 3 &&
                   Customer.CustomerPostCode.All(char.IsDigit) &&

                  !string.IsNullOrWhiteSpace(Customer.CustomerNumber) &&
                  Customer.CustomerNumber.Length > 3 &&
                  Customer.CustomerNumber.All(char.IsDigit) &&

                  !string.IsNullOrWhiteSpace(NewProduct.Description) &&
                  NewProduct.Description.Length > 2 &&

                  !string.IsNullOrWhiteSpace(NewProduct.Quantity) &&
                  NewProduct.Quantity.Length > 0 &&
                  NewProduct.Quantity.Length < 5 &&
                  NewProduct.Quantity.All(char.IsDigit) &&

                  !string.IsNullOrWhiteSpace(NewProduct.PricePerUnit) &&
                  NewProduct.PricePerUnit.Length > 0 &&
                  NewProduct.PricePerUnit.Length < 9 &&
                  isdouble.IsMatch(NewProduct.PricePerUnit);
        }

        /// <summary>
        /// Überprüft, ob die Rechnung gespeichert werden kann
        /// </summary>
        private bool CanSaveInvoice()
        {
            // Überprüft, ob die notwendigen Felder ausgefüllt sind und ob es Posten in der Rechnung gibt
            return !string.IsNullOrWhiteSpace(Customer.CustomerName) &&
                   Customer.CustomerName.Length > 3 &&

                   !string.IsNullOrWhiteSpace(Customer.CustomerStreet) &&
                   Customer.CustomerStreet.Length > 3 &&
                   isStreet.IsMatch(Customer.CustomerStreet) &&

                   !string.IsNullOrWhiteSpace(Customer.CustomerCity) &&
                   Customer.CustomerCity.Length > 3 &&
                   Customer.CustomerCity.All(char.IsLetter) &&

                   !string.IsNullOrWhiteSpace(Customer.CustomerPostCode) &&
                   Customer.CustomerPostCode.Length > 3 &&
                   Customer.CustomerPostCode.All(char.IsDigit) &&

                  !string.IsNullOrWhiteSpace(Customer.CustomerNumber) &&
                  Customer.CustomerNumber.Length > 3 &&
                  Customer.CustomerNumber.All(char.IsDigit)
                  && Products.Count != 0;
        }

        /// <summary>
        /// Überprüft, ob die Rechnung aktualisiert werden kann
        /// </summary>
        private bool CanUpdateInvoice()
        {
            return CanAddProduct() && SelectedItem != null;
        }

        private bool CanMonthlyBalance()
        {
            return InvoiceData.GetInvoices().Count > 0;
        }



        private void MonthlyBalanceSheet()
        {
            RandomExpenses();
            MonthlyBalanceView monthlyBalanceView = new MonthlyBalanceView();
            monthlyBalanceView.DataContext = new MonthlyBalanceViewModel();
            monthlyBalanceView.Show();
        }

        private void ClearWindowItem()
        {
            NewProduct.Description = string.Empty;
            NewProduct.Quantity = string.Empty;
            NewProduct.PricePerUnit = string.Empty;
        }

        private void ClearWinodowAll()
        {
            ClearWindowItem();
            Customer.CustomerName = string.Empty;
            Customer.CustomerNumber = string.Empty;
            Customer.CustomerStreet = string.Empty;
            Customer.CustomerCity = string.Empty;
            Customer.CustomerPostCode = string.Empty;
        }

        /// <summary>
        /// Aktualisiert die Gesamtsummen der Rechnung
        /// </summary>
        private void UpdateInvoiceTotals()
        {
            // Berechnet die Gesamtsummen der Steuern, des Endpreises und des Gesamtpreises
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(VAT));
            OnPropertyChanged(nameof(Total));
        }

        /// <summary>
        /// Aktualisiert die Positionen der Posten in der Rechnung
        /// </summary>
        private void UpdatePositions()
        {
            // Setzt die Positionen der Posten basierend auf ihrer Reihenfolge in der Sammlung
            for (int i = 0; i < Products.Count; i++)
            {
                Products[i].Position = i + 1;
            }
        }
    }
}
