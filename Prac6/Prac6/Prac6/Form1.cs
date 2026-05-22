using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prac6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.taBook.FillByLoad(this.dsPrac6.Book);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            taBook.FillBySearch(dsPrac6.Book, txtSearch.Text);
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            taBook.UpdateQuery(int.Parse(txtUpdateStock.Text), dgvBookList.CurrentRow.Cells[0].Value.ToString());
            taBook.FillByLoad(dsPrac6.Book);
        }

        private void dgvBookList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvInvoice.Rows.Add(
                dgvBookList.CurrentRow.Cells[0].Value.ToString(), 
                dgvBookList.CurrentRow.Cells[1].Value.ToString()
                );

            decimal total = 0;
            foreach (DataGridViewRow row in dgvInvoice.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    total += decimal.Parse(row.Cells[1].Value.ToString());
                }
            }
            txtTotal.Text = total.ToString("c2");
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            int pk;
            DialogResult result = MessageBox.Show("Do you want to Confirm?", "Confirmation", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                pk = (int)taBookOrder.InsertQueryPK(DateTime.Now, Decimal.Parse(txtTotal.Text, NumberStyles.Currency));
                MessageBox.Show("The Order for a Total Amount of " + txtTotal.Text + "has been Confirmed and the PK is " + pk);
                taBookOrder.Fill(dsPrac6.BookOrder); //to fill the gridview with the newly recorded order so that it can be viewed
                for (int i = 0; i < dgvInvoice.Rows.Count - 1; i++) //record the inner detail of the Order
                {
                    string bkID = dgvInvoice.Rows[i].Cells[0].Value.ToString();
                    decimal bkPrice = (decimal)dgvInvoice.Rows[i].Cells[1].Value;
                    taBookOrder.Insert(pk, bkID, 1, bkPrice);
                }
            }

        }
    }
}
