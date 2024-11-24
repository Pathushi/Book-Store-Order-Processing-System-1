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
    public partial class Customer_Registration : Form
    {
        public Customer_Registration()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {

            // Get customer information
            string customerId = txtCustomerId.Text;
            string customerName = txtCustomerName.Text;
            string phone = txtPhone.Text;
            string email = txtEmail.Text;

            // Validate data
            try
            {
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    MessageBox.Show("Customer ID cannot be blank");
                    return;
                }
                if (string.IsNullOrWhiteSpace(customerName))
                {
                    MessageBox.Show("Customer name cannot be blank");
                    return;
                }
                if (string.IsNullOrWhiteSpace(phone) || phone.Length != 10 || !phone.All(char.IsDigit))
                {
                    MessageBox.Show("Customer phone number cannot be blank and it should be 10 digits");
                    return;
                }
                if (string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Customer email cannot be blank");
                    return;
                }

                // All are validated successfully
                MessageBox.Show("All validated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return;
            }

            // Register customer
            try
            {
                // Connect to database
                string connectionString = "Data Source=pathushi\\mssqlserver04;Initial Catalog=BookStoreOrder;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string sql = "INSERT INTO customer(customer_id, customer_name, phone, email) VALUES (@customer_id, @customer_name, @phone, @email)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@customer_id", customerId);
                        cmd.Parameters.AddWithValue("@customer_name", customerName);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@email", email);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Customer registration successful!", "Registration Complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while registering the customer: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            // Confirm the clearing of all fields
            DialogResult result = MessageBox.Show("Are you sure you want to clear all fields?",
                                                  "Confirm Clear",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Clear all text boxes
                txtCustomerId.Text = "";
                txtCustomerName.Text = "";
                txtPhone.Text = "";
                txtEmail.Text = "";
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Dispose();
            DashBoard ds = new DashBoard();
            ds.Show();
        }
    }
}
