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
using System.Data.SqlClient;
using System.Data;

namespace InvenTrack.View
{
    /// <summary>
    /// Interaction logic for AInventory.xaml
    /// </summary>
    public partial class AInventory : UserControl
    {
        public AInventory()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-QP317C6;Initial Catalog=JaensGadgetGarage;Integrated Security=True");

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Inventory", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            AInventoryDataGrid.ItemsSource = dt.DefaultView;
        }
    }
}
