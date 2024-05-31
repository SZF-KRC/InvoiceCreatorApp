using System.Collections.ObjectModel;

namespace InvoiceCreatorApp.Models
{
    public class InvoiceData
    {
        private static ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();

        /// <summary>
        /// Liste der Rechnungen
        /// </summary>
        public static ObservableCollection<Invoice> Invoices => invoices;

        /// <summary>
        /// Fügt eine neue Rechnung zur Liste hinzu
        /// </summary>
        /// <param name="invoice">Die hinzuzufügende Rechnung</param>
        public static void AddInvoice(Invoice invoice)
        {
            invoices.Add(invoice);
        }

        /// <summary>
        /// Gibt die Liste der Rechnungen zurück
        /// </summary>
        /// <returns>Liste der Rechnungen</returns>
        public static ObservableCollection<Invoice> GetInvoices()
        {
            return invoices;
        }
    }
}
