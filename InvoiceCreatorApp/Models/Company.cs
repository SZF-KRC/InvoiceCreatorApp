using InvoiceCreatorApp.MVVM;

namespace InvoiceCreatorApp.Models
{
    public class Company : ViewModelBase
    {
        private string _companyName = "WPF Bau GesmbH";
        private string _companyCity = "Guisberg";
        private string _companyStreet = "Fensterstraße 12";
        private string _companyPostCode = "8788";

        /// <summary>
        /// Firmenname der Rechnung
        /// </summary>
        public string CompanyName
        {
            get => _companyName;
            set { _companyName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Straße des Unternehmens
        /// </summary>
        public string CompanyStreet
        {
            get => _companyStreet;
            set { _companyStreet = value; OnPropertyChanged(); }
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
        public string CompanyPostCode
        {
            get => _companyPostCode;
            set { _companyPostCode = value; OnPropertyChanged(); }
        }
    }
}
