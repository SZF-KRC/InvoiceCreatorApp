using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCreatorApp.Models
{
    public class InvoiceData
    {
        public static ObservableCollection<Invoice> Invoices  = new ObservableCollection<Invoice>();   

        public static ObservableCollection<Invoice> GetInvoices() => Invoices;
        
        public static void AddInvoice(Invoice invoice) {  Invoices.Add(invoice); }
    }
}
