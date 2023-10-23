using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for BOrders.xaml
    /// </summary>
    public partial class BOrders : UserControl
    {
        private SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-QP317C6;Initial Catalog=NathansNutrientNexus;Integrated Security=True");
        private SqlCommand cmd;
        private int id, stock, quantityToTransfer, quantityToReturn, updatedStock;
        private string product;
        private decimal price, totalSum, receivedValue;

        public BOrders()
        {
            InitializeComponent();
            LoadInventoryGrid();
            LoadOrderGrid();
        }

        // Update DataGrids and views
        public void LoadInventoryGrid()
        {
            cmd = new SqlCommand("SELECT ID, Product, Stock, Price FROM Inventory", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            BInventoryDataGrid.ItemsSource = dt.DefaultView;
        }

        public void LoadInventoryGrid(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            BInventoryDataGrid.ItemsSource = dt.DefaultView;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(searchTextBox.Text))
            {
                cmd = new SqlCommand($"SELECT * FROM Inventory WHERE Product LIKE '%{searchTextBox.Text}%' OR Brand LIKE '%{searchTextBox.Text}%' OR Category LIKE '%{searchTextBox.Text}%'", conn);
                LoadInventoryGrid(cmd);
            }
            else
            {
                cmd = new SqlCommand("SELECT ID, Product, Stock, Price FROM Inventory", conn);
                LoadInventoryGrid(cmd);
            }
        }

        // Computing for total
        private decimal CalculateTotal()
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-QP317C6;Initial Catalog=NathansNutrientNexus;Integrated Security=True"))
            {
                conn.Open();

                string query = "SELECT SUM(Total) FROM Orders";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        return totalSum = Convert.ToDecimal(result);
                    }
                    else
                    {
                        return (decimal)0.00;
                    }
                }
            }
        }

        public void LoadOrderGrid()
        {
            cmd = new SqlCommand("SELECT Product, Quantity, Price, Total FROM Orders", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            BOrderDataGrid.ItemsSource = dt.DefaultView;
        }

        // Commands
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {

            if (BInventoryDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Select an item.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(qtyTextBox.Text) || !int.TryParse(qtyTextBox.Text, out quantityToTransfer))
            {
                MessageBox.Show("Enter a valid integer for quantity.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            DataRowView selectedRow = (DataRowView)BInventoryDataGrid.SelectedItem;
            id = (int)selectedRow["ID"];
            product = selectedRow["Product"].ToString();
            stock = (int)selectedRow["Stock"];
            price = (decimal)selectedRow["Price"];
            quantityToTransfer = int.Parse(qtyTextBox.Text);

            if (quantityToTransfer > stock)
            {
                MessageBox.Show("Quantity to transfer cannot exceed the item's stock in Inventory.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using (SqlCommand cmd = new SqlCommand("UPDATE Inventory SET Stock = Stock - @Quantity WHERE ID = @ID", conn))
            {
                cmd.Parameters.AddWithValue("@Quantity", quantityToTransfer);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            using (SqlCommand cmd = new SqlCommand(
                "IF EXISTS (SELECT 1 FROM Orders WHERE ID = @ID) " +
                "BEGIN " +
                "    UPDATE Orders SET Quantity = Quantity + @Quantity WHERE ID = @ID; " +
                "END " +
                "ELSE " +
                "BEGIN " +
                "    INSERT INTO Orders (ID, Product, Quantity, Price) VALUES (@ID, @Product, @Quantity, @Price); " +
                "END", conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Product", product);
                cmd.Parameters.AddWithValue("@Quantity", quantityToTransfer);
                cmd.Parameters.AddWithValue("@Price", price);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            totalTxt.Text = CalculateTotal().ToString();
            LoadInventoryGrid();
            LoadOrderGrid();
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BOrderDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Select an item to return.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            DataRowView selectedRow = (DataRowView)BOrderDataGrid.SelectedItem;
            product = selectedRow["Product"].ToString();
            quantityToReturn = int.Parse(selectedRow["Quantity"].ToString());

            using (SqlCommand cmd = new SqlCommand("DELETE FROM Orders WHERE Product = @Product", conn))
            {
                cmd.Parameters.AddWithValue("@Product", product);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            using (SqlCommand cmd = new SqlCommand("UPDATE Inventory SET Stock = Stock + @Quantity WHERE Product = @Product", conn))
            {
                cmd.Parameters.AddWithValue("@Quantity", quantityToReturn);
                cmd.Parameters.AddWithValue("@Product", selectedRow["Product"].ToString());
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            totalTxt.Text = CalculateTotal().ToString();
            LoadInventoryGrid();
            LoadOrderGrid();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BOrderDataGrid.Items.Count == 1)
            {
                MessageBox.Show("Order is empty.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the order?", "InvenTrack", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return; // User chose not to cancel the order
            }

            conn.Open();

            DataView orderDataView = (DataView)BOrderDataGrid.ItemsSource; // Cast to DataView
            DataTable orderDataTable = orderDataView.Table; // Get the underlying DataTable

            while (orderDataTable.Rows.Count > 0)
            {
                DataRowView selectedRow = orderDataView[0]; // Get the first row as a DataRowView

                product = selectedRow["Product"].ToString();
                quantityToReturn = int.Parse(selectedRow["Quantity"].ToString());

                using (SqlCommand cmd = new SqlCommand("DELETE FROM Orders WHERE Product = @Product", conn))
                {
                    cmd.Parameters.AddWithValue("@Product", product);
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand("UPDATE Inventory SET Stock = Stock + @Quantity WHERE Product = @Product", conn))
                {
                    cmd.Parameters.AddWithValue("@Quantity", quantityToReturn);
                    cmd.Parameters.AddWithValue("@Product", product);
                    cmd.ExecuteNonQuery();
                }

                // Remove the processed row from the DataTable
                orderDataTable.Rows.Remove(selectedRow.Row);
            }

            conn.Close();

            MessageBox.Show("Orders has been canceled.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
            totalTxt.Text = "0.00";
            LoadInventoryGrid();
            LoadOrderGrid();
        }

        private void completeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(receivedTextBox.Text) || !decimal.TryParse(receivedTextBox.Text, out receivedValue))
            {
                MessageBox.Show("Enter a valid decimal for received amount.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            decimal totalAmount = CalculateTotal();
            decimal change = receivedValue - totalAmount;

            using (SqlCommand cmd = new SqlCommand("DELETE FROM Orders", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            changeTxt.Text = change.ToString("0.00");

            LoadOrderGrid();
        }
    }
}
