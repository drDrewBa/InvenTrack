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
using InvenTrack.Model;
using LiveCharts.Wpf;
using LiveCharts;

namespace InvenTrack.View
{
    /// <summary>
    /// Interaction logic for AReports.xaml
    /// </summary>
    public partial class AReports : UserControl
    {

        // Initializing connections and variables -----------------------------------------------------------------------------------------------------------------------------------
        private readonly SqlConnection conn;
        private SqlCommand cmd;
        private string connectionString = @"Data Source=DESKTOP-QP317C6;Initial Catalog=JaensGadgetGarage;Integrated Security=True";
        public Func<ChartPoint, string> PointLabel { get; set; }

        // Construct the child view -------------------------------------------------------------------------------------------------------------------------------------------------
        public AReports()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            LoadAuditGrid();
            LoadPie();
            LoadSalesChart();

            DataContext = this;
        }

        // Load Data Grid ------------------------------------------------------------------------------------------------------------------------------------------------------------
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
                MessageBox.Show(ex.Message, "InvenTrack", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        private void LoadPie()
        {
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT Product, Stock FROM Inventory", conn);
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();

                var seriesCollection = new SeriesCollection();
                var totalStock = dt.AsEnumerable().Sum(row => row.Field<int>("Stock"));

                foreach (DataRow row in dt.Rows)
                {
                    var product = row["Product"].ToString();
                    var stock = Convert.ToDouble(row["Stock"]);
                    double stockPercentage = (stock / totalStock) * 100;

                    var pieSeries = new PieSeries
                    {
                        Title = product,
                        Values = new ChartValues<double> { stockPercentage },
                        DataLabels = true,
                        LabelPoint = point => $"{stockPercentage:F2}%" 
                    };

                    // Add the current PieSeries to the SeriesCollection
                    seriesCollection.Add(pieSeries);
                }

                // Bind the SeriesCollection to the PieChart
                pieChart.Series = seriesCollection;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSalesChart()
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("SELECT OrdersDate, TotalSales FROM Sales ORDER BY OrdersDate", conn);
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();

                // Create a new SeriesCollection for the chart
                var seriesCollection = new SeriesCollection();
                var lineSeries = new LineSeries
                {
                    Title = "TotalSales",
                    Values = new ChartValues<double>(),
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 10,
                };

                // Add data points to the LineSeries
                foreach (DataRow row in dt.Rows)
                {
                    var ordersDate = Convert.ToDateTime(row["OrdersDate"]);
                    var totalSales = Convert.ToDouble(row["TotalSales"]);

                    lineSeries.Values.Add(totalSales);
                }

                // Set the X-axis values to the date values
                var xValues = dt.AsEnumerable().Select(row => Convert.ToDateTime(row["OrdersDate"])).ToList();
                salesChart.AxisX.Add(new Axis
                {
                    Title = "OrdersDate",
                    Labels = xValues.Select(date => date.ToString("yyyy-MM-dd")).ToList()
                });

                seriesCollection.Add(lineSeries);
                salesChart.Series = seriesCollection;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, 
                    "InvenTrack", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
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
                MessageBox.Show(ex.Message, 
                    "InvenTrack", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }
    }
}
