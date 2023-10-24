using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for AReports.xaml
    /// </summary>
    public partial class AReports : UserControl
    {
        private readonly SqlConnection conn;
        private SqlCommand cmd;
        private string connectionString = @"Data Source=DESKTOP-QP317C6;Initial Catalog=JaensGadgetGarage;Integrated Security=True";

        public AReports()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
            LoadAuditGrid();
        }

        private void LoadAuditGrid()
        {
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM Audit", conn);
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();

                AReportsDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClearAlerts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("DELETE FROM Audit", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadAuditGrid();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
