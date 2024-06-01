using InvoiceCreatorApp.Views;
using System.Windows;

namespace InvoiceCreatorApp
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    var mainView = new InvoiceView();
                    mainView.Show();
                    loginView.Close();
                    mainView.IsVisibleChanged += (x, dd) =>
                    {
                        if (mainView.IsVisible == false && mainView.IsLoaded)
                        {
                            var balance = new MonthlyBalanceView();
                            balance.Show();
                            mainView.Close();
                        }
                    };
                }
            };
        }
    }
}
