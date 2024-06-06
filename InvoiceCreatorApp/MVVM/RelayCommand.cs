using System;
using System.Windows.Input;

namespace InvoiceCreatorApp.MVVM
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Tritt ein, wenn sich die Ausführbarkeit des Befehls ändert
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Erstellt eine neue Instanz des RelayCommand
        /// </summary>
        /// <param name="execute">Die auszuführende Aktion</param>
        /// <param name="canExecute">Die Bedingung, ob die Aktion ausführbar ist</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Überprüft, ob der Befehl ausgeführt werden kann
        /// </summary>
        /// <param name="parameter">Parameter für die Bedingung</param>
        /// <returns>True, wenn der Befehl ausgeführt werden kann, andernfalls False</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Führt den Befehl aus
        /// </summary>
        /// <param name="parameter">Parameter für die auszuführende Aktion</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}


