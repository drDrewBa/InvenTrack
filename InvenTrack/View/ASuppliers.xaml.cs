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
    /// Interaction logic for ASuppliers.xaml
    /// </summary>
    public partial class ASuppliers : UserControl
    {

        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-QP317C6;Initial Catalog=JaensGadgetGarage;Integrated Security=True");
        SqlCommand cmd;

        public ASuppliers()
        {
            InitializeComponent();
            cmd = new SqlCommand("SELECT Name, Company, Phone FROM Contacts", conn);
            LoadGrid(cmd);
        }

        // Check if operation requested is viable
        private bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(companyTextBox.Text) || string.IsNullOrWhiteSpace(phoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(emailTextBox.Text) || string.IsNullOrWhiteSpace(addressTextBox.Text) || string.IsNullOrWhiteSpace(imageTextBox.Text))
            {
                MessageBox.Show("There are missing values or incorrect inputs.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private bool ContactExists(string num)
        {
            conn.Open();
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Contacts WHERE Phone = @Phone", conn);
            checkCmd.Parameters.AddWithValue("@Phone", num);
            int count = (int)checkCmd.ExecuteScalar();
            conn.Close();
            return count > 0;
        }

        // Updating tables and operations
        private void ClearData()
        {
            nameTextBox.Clear();
            companyTextBox.Clear();
            phoneTextBox.Clear();
            emailTextBox.Clear();
            addressTextBox.Clear();
            imageTextBox.Clear();
        }

        public void LoadGrid()
        {
            cmd = new SqlCommand("SELECT Name, Company, Phone FROM Contacts", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            ASuppliersDataGrid.ItemsSource = dt.DefaultView;
        }

        public void LoadGrid(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            ASuppliersDataGrid.ItemsSource = dt.DefaultView;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(searchTextBox.Text))
            {
                cmd = new SqlCommand($"SELECT Name, Company, Phone FROM Contacts WHERE Name LIKE '%{searchTextBox.Text}%' OR Company LIKE '%{searchTextBox.Text}%'", conn);
                LoadGrid(cmd);
            }
            else
            {
                cmd = new SqlCommand("SELECT Name, Company, Phone FROM Contacts", conn);
                LoadGrid(cmd);
            }
        }

        private void ASuppliersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    string num = phoneTextBox.Text;
                    if (ContactExists(num))
                    {
                        MessageBox.Show("This contact already exists. Use Update operation.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO Contacts (Phone, Name, Company, Email, Address, Image) VALUES (@Phone, @Name, @Company, @Email, @Address, @Image)", conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Phone", phoneTextBox.Text);
                        cmd.Parameters.AddWithValue("@Name", nameTextBox.Text);
                        cmd.Parameters.AddWithValue("@Company", companyTextBox.Text);
                        cmd.Parameters.AddWithValue("@Email", emailTextBox.Text);
                        cmd.Parameters.AddWithValue("@Address", addressTextBox.Text);
                        cmd.Parameters.AddWithValue("@Image", imageTextBox.Text);
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

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ASuppliersDataGrid.SelectedItem != null)
            {
                try
                {
                    if (IsValid())
                    {
                        conn.Open();
                        DataRowView selectedRow = (DataRowView)ASuppliersDataGrid.SelectedItem;
                        string selectedID = (string) selectedRow["Phone"];
                        cmd = new SqlCommand($"UPDATE Contact SET Name = '{nameTextBox.Text}', " +
                            $"Company = '{companyTextBox.Text}', Email = '{emailTextBox.Text}', " +
                            $"Address = '{addressTextBox.Text}', Image = '{imageTextBox.Text}' WHERE ID = '{selectedID}'", conn);
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
                MessageBox.Show("Please select a row to update.", "InvenTrack", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ASuppliersDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete the selected item?", "InvenTrack", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        DataRowView selectedRow = (DataRowView)ASuppliersDataGrid.SelectedItem;
                        string selectedID = (string) selectedRow["Phone"];
                        cmd = new SqlCommand($"DELETE FROM Contacts WHERE Phone = '{selectedID}'", conn);
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

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }
    }
}
