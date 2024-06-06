using System.Collections.Generic;

namespace InvoiceCreatorApp.Models
{
    public class ExampleExpenseData
    {
        private static List<Expense> _expenses = new List<Expense>();

        /// <summary>
        /// Gibt die Liste aller Ausgaben zurück
        /// </summary>
        /// <returns>Liste der Ausgaben</returns>
        public static List<Expense> GetExpenses() => _expenses;

        /// <summary>
        /// Fügt eine zufällige Ausgabe zur Liste hinzu
        /// </summary>
        /// <param name="expense">Die hinzuzufügende Ausgabe</param>
        public static void AddRandomExpenses(Expense expense)
        {
            _expenses.Add(expense);            
        }
    }
}
