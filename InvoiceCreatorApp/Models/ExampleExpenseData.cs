using System.Collections.Generic;

namespace InvoiceCreatorApp.Models
{
    public class ExampleExpenseData
    {
        private static List<Expense> _expenses = new List<Expense>();

        public static List<Expense> GetExpenses() => _expenses;
     
        public static void AddRandomExpenses(Expense expense)
        {
            _expenses.Add(expense);            
        }
    }
}
