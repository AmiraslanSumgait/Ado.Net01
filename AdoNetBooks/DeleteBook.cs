using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetBooks
{
    public partial class DeleteBook : Form
    {
        public DeleteBook()
        {
            InitializeComponent();
        }

        private void btn_DeleteWithId_Click(object sender, EventArgs e)
        {
            if (textBox1.Text !="")
            {
                try
                {
                    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnString"].ConnectionString))
                    {
                        conn.Open();
                        string query = $"DELETE FROM Books WHERE Id = @Id";
                        SqlCommand command = new SqlCommand(query, conn);
                        command.Parameters.AddWithValue("@Id", textBox1.Text);
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                        MessageBox.Show("Delete succesfully","Succesfully",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Please write id which book you want delete","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }
    }
}
