using System.Collections.Generic;

namespace InvoiceCreatorApp.Models
{
    public class InvoiceData
    {
        private static List<Invoice> _invoices = new List<Invoice>();

        public static void AddInvoice(Invoice invoiceData)
        {
            _invoices.Add(invoiceData);
        }

        public static List<Invoice> GetInvoices() => _invoices;
    }
}
