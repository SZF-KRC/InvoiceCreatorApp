using InvoiceCreatorApp.MVVM;
using System.Windows.Input;


namespace InvoiceCreatorApp.ViewModels
{
    /// <summary>
    /// ViewModel für die Login-Ansicht
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        /// <summary>
        /// Der Benutzername, der im Login-Formular eingegeben wurde
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        /// <summary>
        /// Das Passwort, das im Login-Formular eingegeben wurde
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        /// <summary>
        /// Gibt an, ob die Login-Ansicht sichtbar ist
        /// </summary>
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); }
        }

        /// <summary>
        /// Fehlermeldung, die angezeigt wird, wenn der Login fehlschlägt
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        /// <summary>
        /// Befehl für den Login-Vorgang
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Konstruktor für LoginViewModel
        /// </summary>
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        /// <summary>
        /// Überprüft, ob der Login-Befehl ausgeführt werden kann
        /// </summary>
        /// <param name="obj">Parameter des Befehls</param>
        /// <returns>True, wenn der Befehl ausgeführt werden kann, andernfalls False</returns>
        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || Password == null || Password.Length < 3)
            {
                validData = false;
            }
            else
            {
                validData = true;
            }
            return validData;
        }

        /// <summary>
        /// Führt den Login-Befehl aus
        /// </summary>
        /// <param name="obj">Parameter des Befehls</param>
        private void ExecuteLoginCommand(object obj)
        {
            if(Username == "admin12321" &&  Password == "password65456")
            {
                IsViewVisible = false;
            }
            else { ErrorMessage = "* Benutzername oder Passwort ungültig"; }
        }
    }
}
