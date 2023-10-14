using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvenTrack.View
{
    /// <summary>
    /// Interaction logic for BSwitch.xaml
    /// </summary>
    public partial class BSwitch : UserControl
    {
        public BSwitch()
        {
            InitializeComponent();
        }

        private void InventoryA_Click(object sender, RoutedEventArgs e)
        {
            var inventoryA = new InventoryA();
            inventoryA.Show();
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
}
