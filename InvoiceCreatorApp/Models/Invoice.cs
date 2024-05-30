using InvoiceCreatorApp.MVVM;
using System;

namespace InvoiceCreatorApp.Models
{
    public class Invoice : ViewModelBase
    {
        private string _description;
        private string _numberOfGoods;
        private string _pricePerPiece;
        private string _customerName;
        private string _customerNumber;
        private int _position;

        public int Position 
        {
            get {  return _position; }
            set {  _position = value; OnPropertyChanged(); }
        }  

        public const double Tax = 0.2; // Tax 20%
        public string CompanyName { get; set; }
        public string CustomerName 
        {
            get {  return _customerName; }
            set {  _customerName = value; OnPropertyChanged(); } 
        }
        public string CustomerNumber 
        {
            get { return _customerNumber; }
            set { _customerNumber = value; OnPropertyChanged();}
        }
        
        public string Description 
        {
            get {return _description; }
            set {  _description = value; OnPropertyChanged(); } 
        }

        public string NumberOfGoods 
        {
            get { return _numberOfGoods; }
            set { _numberOfGoods = value; OnPropertyChanged(); OnPropertyChanged(nameof(NettoPrice)); }
        }

        public string PricePerPiece 
        {
            get { return _pricePerPiece;}
            set {  _pricePerPiece = value; OnPropertyChanged(); OnPropertyChanged(nameof(NettoPrice)); }
        }

        public double CalculationTax() => (Int32.Parse(NumberOfGoods) * (Double.Parse(PricePerPiece)) * Tax);

        public double CalculationFinalPrice() => (Int32.Parse(NumberOfGoods) * (Double.Parse(PricePerPiece)) * (1 + Tax));

        public double TotalPrice() => (Int32.Parse(NumberOfGoods) * (Double.Parse(PricePerPiece)));

        public double NettoPrice => (Int32.Parse(NumberOfGoods) * (Double.Parse(PricePerPiece)));

        public Invoice() { }



    }
}
