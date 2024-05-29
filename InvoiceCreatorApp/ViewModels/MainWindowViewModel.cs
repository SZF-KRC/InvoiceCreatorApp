using InvoiceCreatorApp.Models;
using InvoiceCreatorApp.MVVM;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Xceed.Words.NET;

namespace InvoiceCreatorApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Invoice> Invoices { get; set; }
        public InvoiceData invoiceData { get; set; }

        private string _companyName;
        private string _customerNumber;
        private string _customerName;
        private string _descriptionOfGoods;
        private int _numberOfGoods;
        private double _pricePerPiece;
        private double _totalTax;
        private double _finalPrice;

        public RelayCommand AddCommand => new RelayCommand(execute => AddInvoice(), canExecute => CanAddInvoice());
        public RelayCommand UpdateCommand => new RelayCommand(execute => UpdateInvoice(), canExecute => CanUpdateInvoice());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteInvoice(), canExecute => SelectedItem != null);
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveInvoice(), canExecute => CanSaveInvoice());


        public MainWindowViewModel()
        {
            Invoices = new ObservableCollection<Invoice>();
        }


        public string CompanyName
        {
            get => _companyName;
            set
            {
                if (_companyName != value)
                {
                    _companyName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CompanyName));
                }
            }
        }

        public string CustomerNumber
        {
            get => _customerNumber;
            set
            {
                if (_customerNumber != value)
                {
                    _customerNumber = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CustomerNumber));
                }
            }
        }

        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (_customerName != value)
                {
                    _customerName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CustomerName));
                }
            }
        }
        public string DescriptionOfGoods
        {
            get => _descriptionOfGoods;
            set
            {
                if (_descriptionOfGoods != value)
                {
                    _descriptionOfGoods = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DescriptionOfGoods));
                }
            }
        }

        public int NumberOfGoods
        {
            get => (int)_numberOfGoods;
            set
            {
                if (_numberOfGoods != value)
                {
                    _numberOfGoods = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(NumberOfGoods));
                }
            }
        }

        public double PricePerPiece
        {
            get => _pricePerPiece;
            set
            {
                if (_pricePerPiece != value)
                {
                    _pricePerPiece = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PricePerPiece));
                }
            }
        }

        public double TotalTax
        {
            get => _totalTax;
            set
            {
                _totalTax = value;
                OnPropertyChanged();
            }
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        public double FinalPrice
        {
            get => _finalPrice;
            set
            {
                _finalPrice = value;
                OnPropertyChanged();
            }
        }

        private Invoice _currentInvoice;
        public Invoice CurrentInvoice
        {
            get { return _currentInvoice; }
            set { _currentInvoice = value; OnPropertyChanged(nameof(CurrentInvoice)); }
        }

       

        private void SaveInvoice()
        {
            InvoiceDocumentCreator saveDocument = new InvoiceDocumentCreator();
            saveDocument.SaveInvoice(Invoices, CompanyName, CustomerName, CustomerNumber);
       
        }

        private bool CanSaveInvoice()
        {
            return !string.IsNullOrWhiteSpace(CustomerName) &&
                  !string.IsNullOrWhiteSpace(CustomerNumber) && Invoices.Count != 0 ;
        }



        private bool CanUpdateInvoice()
        {
            return !string.IsNullOrWhiteSpace(CustomerName) &&
                  !string.IsNullOrWhiteSpace(DescriptionOfGoods) && NumberOfGoods > 0 && PricePerPiece > 0;
        }

        private Invoice _selectedItem;
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
                    CompanyName = _selectedItem.CompanyName;
                }
            }
        }


        private void UpdateInvoice()
        {
            if(SelectedItem != null)
            {
                SelectedItem.Description = DescriptionOfGoods;
                SelectedItem.NumberOfGoods = NumberOfGoods;
                SelectedItem.PricePerPiece = PricePerPiece;

                OnPropertyChanged(nameof(Invoices));
                SelectedItem = null;

                TotalTax = Invoices.Sum(i => i.CalculationTax());
                FinalPrice = Invoices.Sum(i => i.CalculationFinalPrice());
                TotalPrice = Invoices.Sum(i => i.TotalPrice());

                DescriptionOfGoods = string.Empty;
                NumberOfGoods = 0;
                PricePerPiece = 0;
                OnPropertyChanged();
            }
            
        }

        private bool CanAddInvoice()
        {
            return  !string.IsNullOrWhiteSpace(CustomerName) &&
                  !string.IsNullOrWhiteSpace(DescriptionOfGoods) && NumberOfGoods > 0 && PricePerPiece > 0;
        }

       


        private void AddInvoice()
        {
            var invoice = new Invoice
            {
                CompanyName = CompanyName,
                CustomerName = CustomerName,
                CustomerNumber = CustomerNumber,
                Description = DescriptionOfGoods,
                NumberOfGoods = NumberOfGoods,
                PricePerPiece = PricePerPiece,
            };
            Invoices.Add(invoice);
            TotalTax = Invoices.Sum(i => i.CalculationTax());
            FinalPrice = Invoices.Sum(i => i.CalculationFinalPrice());
            TotalPrice = Invoices.Sum(i => i.TotalPrice());

            DescriptionOfGoods = string.Empty;
            NumberOfGoods = 0;
            PricePerPiece = 0;
            OnPropertyChanged();
        }

        private void DeleteInvoice()
        {
            Invoices.Remove(SelectedItem);
            TotalTax = Invoices.Sum(i => i.CalculationTax());
            FinalPrice = Invoices.Sum(i => i.CalculationFinalPrice());
            TotalPrice = Invoices.Sum(i => i.TotalPrice());
            OnPropertyChanged();         
        }
    }
}
