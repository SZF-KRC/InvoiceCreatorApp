using System.Collections.Generic;

namespace InvoiceCreatorApp.Models
{
    public class InvoiceData
    {
        private static List<Invoice> _invoices = new List<Invoice>();

        /// <summary>
        /// Fügt eine neue Rechnung zur Liste hinzu
        /// </summary>
        /// <param name="invoiceData">Die hinzuzufügende Rechnung</param>
        public static void AddInvoice(Invoice invoiceData)
        {
            _invoices.Add(invoiceData);
        }

        /// <summary>
        /// Gibt die Liste aller Rechnungen zurück
        /// </summary>
        /// <returns>Liste der Rechnungen</returns>
        public static List<Invoice> GetInvoices() => _invoices;
    }
}
