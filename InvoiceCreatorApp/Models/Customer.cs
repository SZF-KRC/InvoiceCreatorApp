using InvoiceCreatorApp.MVVM;

namespace InvoiceCreatorApp.Models
{
    public class Customer: ViewModelBase 
    {
        private string _customerName;
        private string _customerNumber;
        private string _customerCity;
        private string _customerStreet;
        private string _customerPostCode;

        public string CustomerName 
        {
            get => _customerName; 
            set { _customerName = value; OnPropertyChanged(); }
        }
        public string CustomerNumber 
        { 
            get => _customerNumber;
            set { _customerNumber = value; OnPropertyChanged();}
        }
        public string CustomerCity 
        { 
            get => _customerCity;
            set { _customerCity = value; OnPropertyChanged();}
        }
        public string CustomerStreet 
        { 
            get => _customerStreet;
            set { _customerStreet = value; OnPropertyChanged();}
        }
        public string CustomerPostCode 
        {
            get => _customerPostCode;
            set { _customerPostCode = value; OnPropertyChanged();}
        }
    }
}
