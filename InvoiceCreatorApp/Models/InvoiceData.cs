using System.Collections.ObjectModel;

namespace InvoiceCreatorApp.Models
{
    public class InvoiceData
    {
        private static ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();

        public static ObservableCollection<Invoice> Invoices => invoices;

        public static void AddInvoice(Invoice invoice)
        {
            invoices.Add(invoice);
        }

        public static ObservableCollection<Invoice> GetInvoices()
        {
            return invoices;
        }
    }
}
