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
    public partial class Place_Order : Form
    {
        public Place_Order()
        {
            InitializeComponent();
        }

        private void cmbItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cs = "Data Source=pathushi\\mssqlserver04;Initial Catalog=BookStoreOrder;Integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            string sql = "SELECT price FROM items WHERE item_name=@item_name";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@item_name", this.cmbItemName.Text);

            SqlDataReader dr = com.ExecuteReader();
            dr.Read();
            this.txtPrice.Text = dr.GetValue(0).ToString();
            con.Close();

        }

        private void Place_Order_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=pathushi\\mssqlserver04;Initial Catalog=BookStoreOrder;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string sql = "SELECT item_name FROM items";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            cmbItemName.Items.Add(dr.GetString(0));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }

            }
        
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string cs = "Data Source=pathushi\\mssqlserver04;Initial Catalog=BookStoreOrder;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // Inserting data into orderdetails table
                string sql = "INSERT INTO orderdetails(order_id, item_name, price, quantity) " +
                             "VALUES(@order_id, @item_name, @price, @quantity)";

                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.AddWithValue("@order_id", this.txtOrderId.Text);
                com.Parameters.AddWithValue("@item_name", this.cmbItemName.Text);
                com.Parameters.AddWithValue("@price", Convert.ToDecimal(this.txtPrice.Text));
                com.Parameters.AddWithValue("@quantity", Convert.ToInt32(this.numericUpDown1.Value));

                int ret = com.ExecuteNonQuery();

                if (ret == 1)
                {
                    MessageBox.Show("Item added to the order", "Information");

                    // Reload the DataGridView with the updated data
                    LoadOrderDetails();
                }
                else
                {
                    MessageBox.Show("Failed to add item to the order", "Error");
                }
            }
        }

        // Method to load order details into DataGridView
        private void LoadOrderDetails()
        {
            string cs = "Data Source=pathushi\\mssqlserver04;Initial Catalog=BookStoreOrder;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string sql = "SELECT order_id, item_name, price, quantity FROM orderdetails WHERE order_id = @order_id";
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.AddWithValue("@order_id", this.txtOrderId.Text);

                SqlDataAdapter dap = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                dap.Fill(ds);

                this.dataGridView1.DataSource = ds.Tables[0];
            }

        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            string cs = "Data Source=pathushi\\mssqlserver04;Initial Catalog=BookStoreOrder;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                string sql = "INSERT INTO orderitem(order_id, date, customer, email, total) VALUES (@order_id, @date, @customer, @customer, @total)";
                using (SqlCommand com = new SqlCommand(sql, con))
                {
                    com.Parameters.AddWithValue("@order_id", this.txtOrderId.Text);
                    com.Parameters.AddWithValue("@date", this.dateTimePicker1.Text);
                    com.Parameters.AddWithValue("@customer", this.txtCustomer.Text);  // Assuming you have a txtCustomer TextBox
                    com.Parameters.AddWithValue("@email", this.txtEmail.Text);
                    com.Parameters.AddWithValue("@total", this.txtTotal.Text);

                    int ret = com.ExecuteNonQuery();
                    if (ret == 1)
                    {
                        MessageBox.Show("Order placed", "Information");
                    }
                }
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
           
            Double total = 0.0;
            for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++) 
            {
                if (dataGridView1.Rows[i].Cells["price"].Value != null && dataGridView1.Rows[i].Cells["quantity"].Value != null)
                {
                    double price = Convert.ToDouble(dataGridView1.Rows[i].Cells["price"].Value);
                    int quantity = Convert.ToInt32(dataGridView1.Rows[i].Cells["quantity"].Value);
                    total += price * quantity;

                }
            }

            this.txtTotal.Text = total.ToString("F2");


        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string cs = @"Data Source=pathushi\mssqlserver04; Initial Catalog=BookStoreOrder;Integrated Security = True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            //load data to grid view
            string sql2 = "SELECT order_id,item_name,price*quantity as total FROM orderdetails WHERE order_id=@order_id";
            SqlCommand com2 = new SqlCommand(sql2, con);
            com2.Parameters.AddWithValue("@order_id", this.txtOrderId.Text);

            SqlDataAdapter dap = new SqlDataAdapter(com2);
            DataSet ds = new DataSet();
            dap.Fill(ds);
            this.dataGridView1.DataSource = ds.Tables[0];

            con.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get value 
            string orderId = txtOrderId.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(orderId))
            {
                MessageBox.Show("Order ID cannot be blank");
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
                    string sql = "DELETE FROM orderdetails WHERE order_id=@order_id";
                      

                    using (SqlCommand com = new SqlCommand(sql, con))
                    {
                        com.Parameters.AddWithValue("@order_id", orderId);

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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print_Report reportForm = new Print_Report();
            reportForm.Show();
        }
    }
}
