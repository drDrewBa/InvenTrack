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
    /// Interaction logic for BInventory.xaml
    /// </summary>
    public partial class BInventory : UserControl
    {
        private readonly SqlConnection conn;
        private SqlCommand cmd;
        private string connectionString = @"Data Source=DESKTOP-QP317C6;Initial Catalog=NathansNutrientNexus;Integrated Security=True";

        private int selectedID;
        private string selectedProduct;
        private string selectedCategory;
        private string selectedBrand;
        private string selectedStock;
        private string selectedPrice;

        public BInventory()
        {
            InitializeComponent();

            conn = new SqlConnection(connectionString);
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            clearBtn.Click += ClearBtn_Click;

            CheckLowStock();
            LoadAlertsGrid();
            LoadGrid("SELECT * FROM Inventory");
        }

        private void LoadGrid(string query)
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();
                BInventoryDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadAlertsGrid()
        {
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM Alerts", conn);
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                SqlCommand deleteCmd = new SqlCommand("DELETE FROM Alerts", conn);
                deleteCmd.ExecuteNonQuery();

                conn.Close();

                BAlertsDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AInventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BInventoryDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)BInventoryDataGrid.SelectedItem;
                selectedID = (int)selectedRow["ID"];
                selectedProduct = selectedRow["Product"].ToString();
                selectedCategory = selectedRow["Category"].ToString();
                selectedBrand = selectedRow["Brand"].ToString();
                selectedStock = selectedRow["Stock"].ToString();
                selectedPrice = selectedRow["Price"].ToString();

                productTextBox.Text = selectedProduct;
                categoryTextBox.Text = selectedCategory;
                brandTextBox.Text = selectedBrand;
                stockTextBox.Text = selectedStock;
                priceTextBox.Text = selectedPrice;
            }
        }

        private void ClearData()
        {
            productTextBox.Clear();
            categoryTextBox.Clear();
            brandTextBox.Clear();
            stockTextBox.Clear();
            priceTextBox.Clear();
        }

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
            try
            {
                conn.Open();
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Inventory WHERE Product = @Product", conn);
                checkCmd.Parameters.AddWithValue("@Product", productName);
                int count = (int)checkCmd.ExecuteScalar();
                return count > 0;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnClearAlerts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("DELETE FROM Alerts", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadAlertsGrid();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                try
                {
                    string productName = productTextBox.Text;
                    if (ProductExists(productName))
                    {
                        MessageBox.Show("This item already exists. Use the Update operation.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES (@Product, @Category, @Brand, @Stock, @Price)", conn);
                        cmd.Parameters.AddWithValue("@Product", productTextBox.Text);
                        cmd.Parameters.AddWithValue("@Category", categoryTextBox.Text);
                        cmd.Parameters.AddWithValue("@Brand", brandTextBox.Text);
                        cmd.Parameters.AddWithValue("@Stock", stockTextBox.Text);
                        cmd.Parameters.AddWithValue("@Price", priceTextBox.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        CheckLowStock();
                        LoadAlertsGrid();
                        LoadGrid("SELECT * FROM Inventory");
                        ClearData();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BInventoryDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete the selected item?", "InvenTrack", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)BInventoryDataGrid.SelectedItem;
                        int selectedID = (int)selectedRow["ID"];
                        conn.Open();
                        cmd = new SqlCommand($"DELETE FROM Inventory WHERE ID = '{selectedID}'", conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        CheckLowStock();
                        LoadAlertsGrid();
                        ClearData();
                        LoadGrid("SELECT * FROM Inventory");
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
            if (BInventoryDataGrid.SelectedItem != null)
            {
                if (IsValid())
                {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)BInventoryDataGrid.SelectedItem;
                        int selectedID = (int)selectedRow["ID"];
                        cmd = new SqlCommand("UPDATE Inventory SET Product = @Product, Category = @Category, Brand = @Brand, Stock = @Stock, Price = @Price WHERE ID = @ID", conn);
                        cmd.Parameters.AddWithValue("@Product", productTextBox.Text);
                        cmd.Parameters.AddWithValue("@Category", categoryTextBox.Text);
                        cmd.Parameters.AddWithValue("@Brand", brandTextBox.Text);
                        cmd.Parameters.AddWithValue("@Stock", stockTextBox.Text);
                        cmd.Parameters.AddWithValue("@Price", priceTextBox.Text);
                        cmd.Parameters.AddWithValue("@ID", selectedID);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        CheckLowStock();
                        LoadAlertsGrid();
                        ClearData();
                        LoadGrid("SELECT * FROM Inventory");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            string query;

            if (!string.IsNullOrEmpty(searchTextBox.Text))
            {
                query = $"SELECT * FROM Inventory WHERE Product LIKE '%{searchTextBox.Text}%' OR Brand LIKE '%{searchTextBox.Text}%' OR Category LIKE '%{searchTextBox.Text}%'";
                LoadGrid(query);
            }
            else
            {
                query = "SELECT * FROM Inventory";
                LoadGrid(query);
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void CheckLowStock()
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("SELECT ID, Product, Stock FROM Inventory WHERE Stock < 10", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                List<SqlCommand> insertAlertCmds = new List<SqlCommand>();

                while (reader.Read())
                {
                    int productId = (int)reader["ID"];
                    string productName = reader["Product"].ToString();

                    SqlCommand insertAlertCmd = new SqlCommand("INSERT INTO Alerts (AlertDateTime, Message) VALUES (@AlertDateTime, @Message)", conn);
                    insertAlertCmd.Parameters.AddWithValue("@AlertDateTime", DateTime.Now);
                    insertAlertCmd.Parameters.AddWithValue("@Message", $"{productName} is low in stock!");

                    insertAlertCmds.Add(insertAlertCmd);
                }

                reader.Close();
                conn.Close();

                foreach (var insertCmd in insertAlertCmds)
                {
                    conn.Open();
                    insertCmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}
