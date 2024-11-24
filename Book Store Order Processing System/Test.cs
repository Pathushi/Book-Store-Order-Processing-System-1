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
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
    }
}
