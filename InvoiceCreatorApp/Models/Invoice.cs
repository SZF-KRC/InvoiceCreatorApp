using InvoiceCreatorApp.MVVM;

namespace InvoiceCreatorApp.Models
{
    public class Invoice : ViewModelBase
    {
        private string _description;
        private int _numberOfGoods;
        private double _pricePerPiece;
        private string _customerName;
        private string _customerNumber;

        public const double Tax = 0.2; // Tax 20%
        public string CompanyName { get; set; }
        public string CustomerName 
        {
            get {  return _customerName; }
            set {  _customerName = value; OnPropertyChanged(nameof(CustomerName)); } 
        }
        public string CustomerNumber 
        {
            get { return _customerNumber; }
            set { _customerNumber = value; OnPropertyChanged(nameof(CustomerNumber));}
        }
        
        public string Description 
        {
            get {return _description; }
            set {  _description = value; OnPropertyChanged(nameof(Description)); } 
        }

        public int NumberOfGoods 
        {
            get { return _numberOfGoods; }
            set { _numberOfGoods = value; OnPropertyChanged(nameof(NumberOfGoods)); OnPropertyChanged(nameof(NettoPrice)); }
        }

        public double PricePerPiece 
        {
            get { return _pricePerPiece;}
            set {  _pricePerPiece = value; OnPropertyChanged(nameof(PricePerPiece)); OnPropertyChanged(nameof(NettoPrice)); }
        }

        public double CalculationTax() => (NumberOfGoods * PricePerPiece) * Tax;

        public double CalculationFinalPrice() => (NumberOfGoods * PricePerPiece) * (1 + Tax);

        public double TotalPrice() => NumberOfGoods * PricePerPiece;

        public double NettoPrice => NumberOfGoods * PricePerPiece;

        public Invoice() { }



    }
}
