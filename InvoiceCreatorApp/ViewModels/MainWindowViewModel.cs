using InvoiceCreatorApp.Models;
using InvoiceCreatorApp.MVVM;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvoiceCreatorApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Invoice> oneInvoice { get; set; }

        private string _companyName;
        private string _customerNumber;
        private string _customerName;
        private string _descriptionOfGoods;
        private string _numberOfGoods;
        private string _pricePerPiece;
        private double _totalTax;
        private double _finalPrice;
        private double _totalPrice;
        private Invoice _selectedItem;


        public RelayCommand AddCommand => new RelayCommand(execute => AddInvoice(), canExecute => CanAddInvoice());
        public RelayCommand UpdateCommand => new RelayCommand(execute => UpdateInvoice(), canExecute => CanUpdateInvoice());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteInvoice(), canExecute => SelectedItem != null);
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveInvoice(), canExecute => CanSaveInvoice());


        public MainWindowViewModel()
        {
            oneInvoice = new ObservableCollection<Invoice>();
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
                }
            }
        }

        public string NumberOfGoods
        {
            get => _numberOfGoods;
            set
            {
                if (_numberOfGoods != value)
                {
                    _numberOfGoods = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PricePerPiece
        {
            get => _pricePerPiece;
            set
            {
                if (_pricePerPiece != value)
                {
                    _pricePerPiece = value;
                    OnPropertyChanged();
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

        private void SaveInvoice()
        {
            InvoiceDocumentCreator saveDocument = new InvoiceDocumentCreator();
            saveDocument.SaveInvoice(oneInvoice, CustomerName, CustomerNumber);

            var invoiceData = new InvoiceData();
            foreach (var invoice in oneInvoice)
            {
                InvoiceData.AddInvoice(invoice);
            }

            oneInvoice.Clear();
            UpdateInvoiceTotals();


        }

        private bool CanSaveInvoice()
        {
            return !string.IsNullOrWhiteSpace(CustomerName) &&
                  !string.IsNullOrWhiteSpace(CustomerNumber) &&
                  CustomerNumber.Length > 3 &&
                  CustomerNumber.All(char.IsDigit) &&
                  oneInvoice.Count != 0 ;
        }



        private bool CanUpdateInvoice()
        {
            //return !string.IsNullOrWhiteSpace(CustomerName) &&
            //      !string.IsNullOrWhiteSpace(DescriptionOfGoods) && 
            //      !string.IsNullOrWhiteSpace(NumberOfGoods) && 
            //      !string.IsNullOrWhiteSpace(PricePerPiece) &&
            //      CustomerNumber.Length > 3 &&
            //      CustomerNumber.All(char.IsDigit) &&
            //      NumberOfGoods.Length > 0 &&
            //      NumberOfGoods.All(char.IsDigit) &&
            //      PricePerPiece.Length > 0 &&
            //      PricePerPiece.All(char.IsDigit)&&
            return CanAddInvoice() &&
                  SelectedItem !=null;
        }

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

                OnPropertyChanged(nameof(oneInvoice));
                SelectedItem = null;
                UpdateInvoiceTotals();       

                DescriptionOfGoods = string.Empty;
                NumberOfGoods = string.Empty;
                PricePerPiece = string.Empty;
                OnPropertyChanged();
            }
            
        }

        Regex isdouble = new Regex(@"^-?\d+(\,\d+)?$");

        private bool CanAddInvoice()
        {
            return  !string.IsNullOrWhiteSpace(CustomerName) &&
                  !string.IsNullOrWhiteSpace(DescriptionOfGoods) &&
                  !string.IsNullOrWhiteSpace(CustomerNumber) &&
                  CustomerNumber.Length > 3 &&
                  CustomerNumber.All(char.IsDigit) &&
                  !string.IsNullOrWhiteSpace(NumberOfGoods) &&
                  NumberOfGoods.Length > 0 &&
                  NumberOfGoods.All(char.IsDigit)&&
                  !string.IsNullOrWhiteSpace(PricePerPiece)&&
                  PricePerPiece.Length >0 &&
                  isdouble.IsMatch(PricePerPiece);


                
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
                Position = oneInvoice.Count+1,
            };
            oneInvoice.Add(invoice);
            UpdateInvoiceTotals();

            DescriptionOfGoods = string.Empty;
            NumberOfGoods = string.Empty;
            PricePerPiece = string.Empty;
            OnPropertyChanged();
        }

        private void DeleteInvoice()
        {
            oneInvoice.Remove(SelectedItem);
            UpdateInvoiceTotals();
            UpdatePositions();
            OnPropertyChanged();         
        }
        private void UpdateInvoiceTotals()
        {
            TotalTax = oneInvoice.Sum(i => i.CalculationTax());
            FinalPrice = oneInvoice.Sum(i => i.CalculationFinalPrice());
            TotalPrice = oneInvoice.Sum(i => i.TotalPrice());
        }
        private void UpdatePositions()
        {
            for (int i = 0; i < oneInvoice.Count; i++)
            {
                oneInvoice[i].Position = i + 1;
            }
        }
    }
}
