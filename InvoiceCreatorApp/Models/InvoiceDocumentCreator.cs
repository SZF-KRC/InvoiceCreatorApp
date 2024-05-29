using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace InvoiceCreatorApp.Models
{
    public class InvoiceDocumentCreator
    {
        public void SaveInvoice(ObservableCollection<Invoice> invoices, string companyName, string customerName, string customerNumber)
        {       
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "DOCX files (*.docx)|*.docx"; // Opravený filter pre DOCX
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var document = DocX.Create(saveFileDialog.FileName))
                {
                    // Hlavička spoločnosti
                    var header = document.InsertParagraph();
                    header.AppendLine(companyName+" GesmbH").FontSize(16).Bold();
                    header.AppendLine("Fensterstraße 12").FontSize(12);
                    header.AppendLine("8788 Guisberg").FontSize(12);
                    header.AppendLine("Phone: +43 660 111 222").FontSize(12);
                    header.AppendLine("Email: info@wpfbau.at").FontSize(12);
                    header.Alignment = Alignment.left;

                    // Informácie o faktúre
                    var invoiceInfo = document.InsertParagraph();
                    invoiceInfo.AppendLine("Rechnung").FontSize(18).Bold();
                    invoiceInfo.AppendLine($"Rechnungs-Nr.: {Guid.NewGuid()}").FontSize(12);
                    invoiceInfo.AppendLine($"Kunden-Nr.: {customerNumber}").FontSize(12);
                    invoiceInfo.AppendLine($"Kundenname: {customerName}").FontSize(12);
                    invoiceInfo.AppendLine($"Datum: {DateTime.Now.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}").FontSize(12);
                    invoiceInfo.Alignment = Alignment.left;

                    // Tabuľka s položkami faktúry
                    var table = document.AddTable(invoices.Count + 1, 6);
                    table.Design = TableDesign.LightShadingAccent1;
                    table.Alignment = Alignment.center;
                    table.Rows[0].Cells[0].Paragraphs[0].Append("Pos.").Bold();
                    table.Rows[0].Cells[1].Paragraphs[0].Append("Bezeichnung").Bold();
                    table.Rows[0].Cells[2].Paragraphs[0].Append("Menge").Bold();
                    table.Rows[0].Cells[3].Paragraphs[0].Append("Einheit").Bold();
                    table.Rows[0].Cells[4].Paragraphs[0].Append("E-Preis").Bold();
                    table.Rows[0].Cells[5].Paragraphs[0].Append("Gesamt").Bold();

                    int rowIndex = 1;
                    foreach (var invoice in invoices)
                    {
                        table.Rows[rowIndex].Cells[0].Paragraphs[0].Append(rowIndex.ToString());
                        table.Rows[rowIndex].Cells[1].Paragraphs[0].Append(invoice.Description);
                        table.Rows[rowIndex].Cells[2].Paragraphs[0].Append(invoice.NumberOfGoods.ToString());
                        table.Rows[rowIndex].Cells[3].Paragraphs[0].Append("Stk."); // Jednotka môže byť statická alebo dynamická
                        table.Rows[rowIndex].Cells[4].Paragraphs[0].Append(invoice.PricePerPiece.ToString("F2"));
                        table.Rows[rowIndex].Cells[5].Paragraphs[0].Append(invoice.TotalPrice().ToString("F2"));
                        rowIndex++;
                    }

                    document.InsertTable(table);
                    // Celkové sumy
                    var totals = document.InsertParagraph();
                    totals.AppendLine($"Summe Netto: {invoices.Sum(i => i.TotalPrice()).ToString("F2")} €").FontSize(12).Bold();
                    totals.AppendLine($"19,00% USt. auf {invoices.Sum(i => i.TotalPrice()).ToString("F2")} €").FontSize(12).Bold();
                    document.Save();
                }                                
            }           
        } 
    }
}
