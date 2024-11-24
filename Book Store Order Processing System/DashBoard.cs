using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Book_Store_Order_Processing_System
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        private void btnItemRegistration_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Item_Registration item = new Item_Registration();
            item.Show();
        }

        private void btnCustomerRegistration_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Customer_Registration customer = new Customer_Registration();
            customer.Show();
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Place_Order place_Order = new Place_Order();
            place_Order.Show();
        }
    }
}
