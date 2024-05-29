using System.Windows;
using System.Windows.Controls;

namespace InvoiceCreatorApp.Views
{
    /// <summary>
    /// Interaktionslogik für PasswordUserControlView.xaml
    /// </summary>
    public partial class PasswordUserControlView : UserControl
    {


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordUserControlView), new PropertyMetadata(string.Empty));


        public PasswordUserControlView()
        {
            InitializeComponent();
        }

        private void passwordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = passwordBox.Password;
        }
    }
}
