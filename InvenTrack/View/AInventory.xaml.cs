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

        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-QP317C6;Initial Catalog=JaensGadgetGarage;Integrated Security=True");
        SqlCommand cmd;
        private int selectedID;

        public AInventory()
        {
            InitializeComponent();
            cmd = new SqlCommand("SELECT * FROM Inventory", conn);
            LoadGrid(cmd);
        }

        // Check if operation requested is viable
        private bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(productTextBox.Text) || string.IsNullOrWhiteSpace(categoryTextBox.Text) || string.IsNullOrWhiteSpace(brandTextBox.Text) ||
                !int.TryParse(stockTextBox.Text, out _) || !decimal.TryParse(priceTextBox.Text, out _))
            {
                MessageBox.Show("There are missing values or incorrect inputs.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private bool ProductExists(string productName)
        {
            conn.Open();
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Inventory WHERE Product = @Product", conn);
            checkCmd.Parameters.AddWithValue("@Product", productName);
            int count = (int)checkCmd.ExecuteScalar();
            conn.Close();
            return count > 0;
        }

        private void AInventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (AInventoryDataGrid.SelectedItem != null)
                {
                    DataRowView selectedRow = (DataRowView)AInventoryDataGrid.SelectedItem;
                    selectedID = (int)selectedRow["ID"];
                }
            }
            catch
            {
                MessageBox.Show("Row selected in NULL", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        // Update the DataGrid, update Inventory Operations
        private void ClearData()
        {
            productTextBox.Clear();
            categoryTextBox.Clear();
            brandTextBox.Clear();
            stockTextBox.Clear();
            priceTextBox.Clear();
        }

        public void LoadGrid()
        {
            cmd = new SqlCommand("SELECT * FROM Inventory", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            AInventoryDataGrid.ItemsSource = dt.DefaultView;
        }

        public void LoadGrid(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            AInventoryDataGrid.ItemsSource = dt.DefaultView;
        }

        // Operations
        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(searchTextBox.Text))
            {
                cmd = new SqlCommand($"SELECT * FROM Inventory WHERE Product LIKE '%{searchTextBox.Text}%' OR Brand LIKE '%{searchTextBox.Text}%' OR Category LIKE '%{searchTextBox.Text}%'", conn);
                LoadGrid(cmd);
            }
            else
            {
                cmd = new SqlCommand("SELECT * FROM Inventory", conn);
                LoadGrid(cmd);
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    string productName = productTextBox.Text;
                    if (ProductExists(productName))
                    {
                        MessageBox.Show("This item already exists. Use Update operation.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES (@Product, @Category, @Brand, @Stock, @Price)", conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Product", productTextBox.Text);
                        cmd.Parameters.AddWithValue("@Category", categoryTextBox.Text);
                        cmd.Parameters.AddWithValue("@Brand", brandTextBox.Text);
                        cmd.Parameters.AddWithValue("@Stock", stockTextBox.Text);
                        cmd.Parameters.AddWithValue("@Price", priceTextBox.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        LoadGrid();
                        ClearData();
                    }  
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AInventoryDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete the selected item?", "InvenTrack", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        DataRowView selectedRow = (DataRowView)AInventoryDataGrid.SelectedItem;
                        int selectedID = (int)selectedRow["ID"];
                        cmd = new SqlCommand($"DELETE FROM Inventory WHERE ID = '{selectedID}'", conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        ClearData();
                        LoadGrid();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AInventoryDataGrid.SelectedItem != null)
            {
                try
                {
                    if (IsValid())
                    {
                        conn.Open();
                        DataRowView selectedRow = (DataRowView)AInventoryDataGrid.SelectedItem;
                        int selectedID = (int)selectedRow["ID"];
                        cmd = new SqlCommand($"UPDATE Inventory SET Product = '{productTextBox.Text}', " +
                            $"Category = '{categoryTextBox.Text}', Brand = '{brandTextBox.Text}', " +
                            $"Stock = '{stockTextBox.Text}', Price = '{priceTextBox.Text}' WHERE ID = '{selectedID}'", conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        ClearData();
                        LoadGrid();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Inform the user that they need to select a row
                MessageBox.Show("Please select a row to update.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
