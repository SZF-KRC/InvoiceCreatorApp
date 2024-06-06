using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InvoiceCreatorApp.MVVM
{
    /// <summary>
    /// Basisklasse für ViewModel, die die INotifyPropertyChanged-Schnittstelle implementiert
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Tritt ein, wenn sich der Wert einer Eigenschaft ändert
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Benachrichtigt die Benutzeroberfläche über eine Änderung einer Eigenschaft
        /// </summary>
        /// <param name="propertyName">Der Name der geänderten Eigenschaft</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
