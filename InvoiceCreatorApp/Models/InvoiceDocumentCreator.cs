using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace InvoiceCreatorApp.Models
{
    public class InvoiceDocumentCreator
    {
        /// <summary>
        /// Speichert die Rechnung in einer DOCX-Datei
        /// </summary>
        public void SaveInvoice(Invoice invoice, ObservableCollection<Product> products)
        {       
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "DOCX files (*.docx)|*.docx"; 
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var document = DocX.Create(saveFileDialog.FileName))
                {

                    // Firmenkopf
                    var header = document.InsertParagraph();
                    header.AppendLine($"{invoice.Company.CompanyName}").FontSize(16).Bold();
                    header.AppendLine($"Straße: {invoice.Company.CompanyStreet}").FontSize(12);
                    header.AppendLine($"PLZ,Stadt: {invoice.Company.CompanyPostCode}, {invoice.Company.CompanyCity}").FontSize(12);
                    header.AppendLine("Phone: +43 660 111 222").FontSize(12);
                    header.AppendLine("Email: info@wpfbau.at").FontSize(12);
                    header.Alignment = Alignment.left;

                    // Rechnungsinformationen
                    var invoiceInfo = document.InsertParagraph();
                    invoiceInfo.AppendLine("Rechnung").FontSize(18).Bold();
                    invoiceInfo.AppendLine($"Rechnungs-Nr.: {invoice.InvoiceNumber}").FontSize(12);
                    invoiceInfo.AppendLine($"Kunden-Nr.: KU-{invoice.Customer.CustomerNumber}").FontSize(12);
                    invoiceInfo.AppendLine($"Kundenname: {invoice.Customer.CustomerName}").FontSize(12);
                    invoiceInfo.AppendLine($"Straße: {invoice.Customer.CustomerStreet}").FontSize(12);
                    invoiceInfo.AppendLine($"PLZ,Stadt: {invoice.Customer.CustomerPostCode}, {invoice.Customer.CustomerCity}").FontSize(12);
                    invoiceInfo.AppendLine($"Datum: {invoice.DateOfIssue}").FontSize(12);
                    invoiceInfo.Alignment = Alignment.left;



                    // Tabelle mit den Rechnungsposten
                    var table = document.AddTable(products.Count + 1, 6);
                    table.Design = TableDesign.LightShadingAccent1;
                    table.Alignment = Alignment.center;
                    table.Rows[0].Cells[0].Paragraphs[0].Append("Pos.").Bold();
                    table.Rows[0].Cells[1].Paragraphs[0].Append("Bezeichnung").Bold();
                    table.Rows[0].Cells[2].Paragraphs[0].Append("Menge").Bold();
                    table.Rows[0].Cells[4].Paragraphs[0].Append("Preis pro Stück").Bold();
                    table.Rows[0].Cells[5].Paragraphs[0].Append("Gesamt").Bold();

                    int rowIndex = 1;
                    foreach (var oneItem in products)
                    {
                        table.Rows[rowIndex].Cells[0].Paragraphs[0].Append(rowIndex.ToString());
                        table.Rows[rowIndex].Cells[1].Paragraphs[0].Append(oneItem.Description);
                        table.Rows[rowIndex].Cells[2].Paragraphs[0].Append(oneItem.Quantity.ToString());
                        table.Rows[rowIndex].Cells[4].Paragraphs[0].Append(oneItem.PricePerUnit.ToString());
                        table.Rows[rowIndex].Cells[5].Paragraphs[0].Append(oneItem.TotalPrice.ToString("F2"));
                        rowIndex++;
                    }

                    document.InsertTable(table);
                    // Gesamtsummen
                    var totals = document.InsertParagraph();
                    totals.AppendLine($"Summe Netto: {invoice.Subtotal} €").FontSize(12).Bold();
                    totals.AppendLine($"20% USt. auf {invoice.VAT} €").FontSize(12).Bold();
                    totals.AppendLine($"Endsumme: {invoice.Total}€").FontSize(14).Bold();
                    document.Save();
                }                                
            }           
        } 
    }
}
