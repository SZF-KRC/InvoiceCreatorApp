using InvoiceCreatorApp.MVVM;
using System;
using System.Collections.ObjectModel;

namespace InvoiceCreatorApp.Models
{
    public class FinalInvoice : ViewModelBase
    {
        public string CustomerName { get; set; }
        public string CustomerNum { get; set; }
        public string InvoiceNum { get; set; }
        public string CurrentTime { get; set; }
        public double Tax { get; set; }
        public double Netto { get; set; }
        public double FinalPrice { get; set; }
        public ObservableCollection<Invoice> Items { get; set; }

        public FinalInvoice()
        {
            Items = new ObservableCollection<Invoice>();
        }
    }
}
