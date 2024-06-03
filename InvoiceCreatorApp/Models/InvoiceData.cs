using System.Collections.Generic;

namespace InvoiceCreatorApp.Models
{
    public class InvoiceData
    {
        private static List<FinalInvoice> _invoices = new List<FinalInvoice>();

        public static void AddInvoice(FinalInvoice invoice)
        {
            _invoices.Add(invoice);
        }

        public static List<FinalInvoice> GetInvoices()
        {
            return _invoices;
        }
    }
}
