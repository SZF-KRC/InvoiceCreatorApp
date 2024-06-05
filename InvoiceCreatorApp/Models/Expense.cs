using System;

namespace InvoiceCreatorApp.Models
{
    public class Expense
    {
        public string ExpenseNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public string CompanyName { get; set;}

        public double Netto { get; set;}
        public double VAT {  get; set;}
        public double Total {  get; set;}
    }
}
