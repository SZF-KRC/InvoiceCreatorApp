using System.Collections.Generic;
using System.Linq;

namespace InvoiceCreatorApp.Models
{
    public class Invoice 
    {
        public Company Company { get; set; }
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

        public string DateOfIssue { get; set; }
        public string InvoiceNumber { get; set; }

        public double Subtotal => Products.Sum(p => p.TotalPrice);
        public double VAT => Subtotal * 0.20;
        public double Total => Subtotal + VAT;
    }
}
