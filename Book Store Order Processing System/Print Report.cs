using CrystalDecisions.CrystalReports.Engine;
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
    public partial class Print_Report : Form
    {
        public Print_Report()
        {
            InitializeComponent();
        }

        private void Print_Report_Load(object sender, EventArgs e)
        {
            string cs = "Data Source=pathushi\\mssqlserver04;Initial Catalog=BookStoreOrder;Integrated Security=True";
            SqlConnection con = new SqlConnection(cs);
            con.Open();

            // Get data from Returnbooks table
            string sql = "SELECT order_id, item_name,price,quantity From orderdetails";
            SqlCommand com = new SqlCommand(sql, con);

            SqlDataAdapter dap = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            dap.Fill(ds);

            // Bind data with the crystal report
            ReportDocument rpt = new ReportDocument();
            rpt.Load(@"C:\Users\ws\source\repos\Book Store Order Processing System\Book Store Order Processing System\Report.rpt");
            rpt.SetDataSource(ds.Tables[0]);

            this.crystalReportViewer1.ReportSource = rpt;

            con.Close();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
    }

