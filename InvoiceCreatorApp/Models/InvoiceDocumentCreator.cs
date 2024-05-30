﻿using Microsoft.Win32;
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
        public void SaveInvoice(ObservableCollection<Invoice> invoices, string customerName, string customerNumber)
        {       
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "DOCX files (*.docx)|*.docx"; 
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var document = DocX.Create(saveFileDialog.FileName))
                {
                    // Hlavička spoločnosti
                    var header = document.InsertParagraph();
                    header.AppendLine("WPF Bau GesmbH").FontSize(16).Bold();
                    header.AppendLine("Fensterstraße 12").FontSize(12);
                    header.AppendLine("8788 Guisberg").FontSize(12);
                    header.AppendLine("Phone: +43 660 111 222").FontSize(12);
                    header.AppendLine("Email: info@wpfbau.at").FontSize(12);
                    header.Alignment = Alignment.left;

                    // Informácie o faktúre
                    var invoiceInfo = document.InsertParagraph();
                    invoiceInfo.AppendLine("Rechnung").FontSize(18).Bold();
                    invoiceInfo.AppendLine($"Rechnungs-Nr.: {Guid.NewGuid()}").FontSize(12);
                    invoiceInfo.AppendLine($"Kunden-Nr.: KU-{customerNumber}").FontSize(12);
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
                    table.Rows[0].Cells[4].Paragraphs[0].Append("Preis pro Stück").Bold();
                    table.Rows[0].Cells[5].Paragraphs[0].Append("Gesamt").Bold();

                    int rowIndex = 1;
                    foreach (var invoice in invoices)
                    {
                        table.Rows[rowIndex].Cells[0].Paragraphs[0].Append(rowIndex.ToString());
                        table.Rows[rowIndex].Cells[1].Paragraphs[0].Append(invoice.Description);
                        table.Rows[rowIndex].Cells[2].Paragraphs[0].Append(invoice.NumberOfGoods.ToString());
                        table.Rows[rowIndex].Cells[4].Paragraphs[0].Append(invoice.PricePerPiece.ToString());
                        table.Rows[rowIndex].Cells[5].Paragraphs[0].Append(invoice.TotalPrice().ToString("F2"));
                        rowIndex++;
                    }

                    document.InsertTable(table);
                    // Celkové sumy
                    var totals = document.InsertParagraph();
                    totals.AppendLine($"Summe Netto: {invoices.Sum(i => i.TotalPrice()).ToString("F2")} €").FontSize(12).Bold();
                    totals.AppendLine($"20% USt. auf {invoices.Sum(i => i.CalculationTax()).ToString("F2")} €").FontSize(12).Bold();
                    totals.AppendLine($"Endsumme: {invoices.Sum(i=>i.CalculationFinalPrice()).ToString("F2")}€").FontSize(14).Bold();
                    document.Save();
                }                                
            }           
        } 
    }
}