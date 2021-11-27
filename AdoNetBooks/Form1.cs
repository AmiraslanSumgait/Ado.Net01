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
    public partial class Form1 : Form
    {
        string connectionString = null;
        SqlDataAdapter da = null;
        DataTable dt = null;
        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["myConnString"].ConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    da = new SqlDataAdapter($"SELECT *FROM BOOKS", conn);
                    dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewBooks.DataSource = dt;

                    var queries = "SELECT NAME From Categories; Select FirstName + LastName From Authors";
                    SqlCommand command = new SqlCommand(queries, conn);
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        int line = 0;
                        do
                        {
                            while (dataReader.Read())
                            {
                                if (line == 0) cbx_Category.Items.Add(dataReader.GetValue(0));
                                else if (line == 1) cbx_Authors.Items.Add(dataReader.GetValue(0));

                               
                            }
                            line++;
                        } while (dataReader.NextResult());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbx_Authors_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    da = new SqlDataAdapter($"SELECT * FROM BOOKS  INNER JOIN AUTHORS ON Books.Id_Author = Authors.Id WHERE Authors.FirstName + Authors.LastName = '{cbx_Authors.SelectedItem.ToString()}'", conn);
                    dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewBooks.DataSource = dt;
                    cbx_Category.ResetText();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
            }
        }

        private void cbx_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    da = new SqlDataAdapter($"SELECT * FROM Books INNER JOIN Categories  ON Categories.Id = Books.Id_Category WHERE Categories.Name = '{cbx_Category.SelectedItem.ToString()}'", conn);
                    dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewBooks.DataSource = dt;
                    cbx_Category.ResetText();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    da = new SqlDataAdapter($"SELECT * from Books WHERE Name LIKE '{ textBox1.Text}%'", conn);
                    dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewBooks.DataSource = dt;
                    cbx_Authors.ResetText();
                    cbx_Category.ResetText();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            DeleteBook deleteBook = new DeleteBook();
            deleteBook.ShowDialog();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            AddBook addBook = new AddBook();
            addBook.ShowDialog();

        }

        private void btn_RefreshBooks_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    da = new SqlDataAdapter($"SELECT * FROM Books", conn);
                    dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewBooks.DataSource = dt;
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    da = new SqlDataAdapter($"SELECT * FROM Books", conn);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);
                    da.Update(dt);
                    MessageBox.Show("Update successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
            }
        }
    }
}
