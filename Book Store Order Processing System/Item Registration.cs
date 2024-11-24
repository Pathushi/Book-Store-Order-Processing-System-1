using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Book_Store_Order_Processing_System
{
    public partial class Item_Registration : Form
    {
       

        public Item_Registration()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtItemId.Text) ||
                string.IsNullOrWhiteSpace(txtItemName.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Validate price field
            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            string connectionString = "Data Source=pathushi\\mssqlserver04;Initial Catalog=BookStoreOrder;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string sql = "INSERT INTO items(item_id, item_name, price) VALUES(@item_id, @item_name, @price)";
                    using (SqlCommand com = new SqlCommand(sql, con))
                    {
                        // Add parameters
                        com.Parameters.AddWithValue("@item_id", txtItemId.Text);
                        com.Parameters.AddWithValue("@item_name", txtItemName.Text);
                        com.Parameters.AddWithValue("@price", price);

                        int ret = com.ExecuteNonQuery();
                        MessageBox.Show("No of records inserted: " + ret);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtItemId.Text) ||
                string.IsNullOrWhiteSpace(txtItemName.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Validate numeric fields
            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            string connectionString = "Data Source=pathushi\\mssqlserver04;Initial Catalog=BookStoreOrder;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Define SQL Command passing parameters
                    string sql = "UPDATE items SET item_name=@item_name, price=@price WHERE item_id=@item_id";
                    using (SqlCommand com = new SqlCommand(sql, con))
                    {
                        // Add parameters
                        com.Parameters.AddWithValue("@item_id", txtItemId.Text);
                        com.Parameters.AddWithValue("@item_name", txtItemName.Text);
                        com.Parameters.AddWithValue("@price", price);

                        int ret = com.ExecuteNonQuery();
                        MessageBox.Show("No of records updated: " + ret);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get value 
            string itemId = txtItemId.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(itemId))
            {
                MessageBox.Show("Item ID cannot be blank");
                return;
            }

            // Connection string
            string connectionString = @"Data Source=pathushi\mssqlserver04;Initial Catalog=BookStoreOrder;Integrated Security=True";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Define SQL Command 
                    string sql = "DELETE FROM items WHERE item_id=@item_id";
                    using (SqlCommand com = new SqlCommand(sql, con))
                    {
                        com.Parameters.AddWithValue("@item_id", itemId);

                        // Execute Delete Command
                        DialogResult mret = MessageBox.Show("Are you sure you want to delete this record?",
                            "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (mret == DialogResult.Yes)
                        {
                            int ret = com.ExecuteNonQuery();
                            MessageBox.Show($"Number of records deleted: {ret}", "Information");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
            DashBoard ds = new DashBoard();
            ds.Show();
        }
    }
}
