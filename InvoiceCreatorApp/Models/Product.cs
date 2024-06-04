using InvoiceCreatorApp.MVVM;
using System;

namespace InvoiceCreatorApp.Models
{
    public class Product:ViewModelBase 
    {
        private string _description;
        private string _quantity;
        private string _pricePerUnit;    

        public string Description 
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }
        public string Quantity 
        { 
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); } 
        }
        public string PricePerUnit 
        {
            get => _pricePerUnit;
            set { _pricePerUnit = value; OnPropertyChanged(); }
        }

        public double TotalPrice => (Int32.Parse(Quantity)) * (Double.Parse(PricePerUnit));

        public int Position { get; set; }
    }
}
