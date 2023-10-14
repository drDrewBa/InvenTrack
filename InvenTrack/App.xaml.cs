using InvenTrack.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace InvenTrack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(Object sender, StartupEventArgs e)
        {

            var inventoryA = new InventoryA();
            inventoryA.Show();

            //var inventoryB = new InventoryB();
            //inventoryB.Show();

            //var loginView = new LoginView();
            //loginView.Show();
            //loginView.IsVisibleChanged += (s, ev) =>
            //{
            //    if (loginView.IsVisible == false && loginView.IsLoaded)
            //    {
            //        var mainView = new MainView();
            //        mainView.Show();
            //        loginView.Close();
            //    }
            //};
        }
    }
}
