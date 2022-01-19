using project.Properties;
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

namespace project
{
    public partial class insert : Form
    {
        public insert()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.Default.ProjectConnectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                                                
                        command.CommandText = $"Insert into  Myexercise(partname, picture,kilo,calories,frequency) " +
                            $"values (@partname, @picture,@kilo,@calories,@frequency)";
                        command.Connection = conn;

                        //===========================
                        byte[] bytes = { 1, 3 };

                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        bytes = ms.GetBuffer();

                        //=========================

                        command.Parameters.Add("@partname", SqlDbType.NVarChar, 50).Value = this.textBox1.Text;
                        command.Parameters.Add("@picture", SqlDbType.VarBinary).Value = bytes;

                        command.Parameters.Add("@calories", SqlDbType.Int).Value = int.Parse(this.textBox2.Text);
                        command.Parameters.Add("@kilo", SqlDbType.Int).Value = int.Parse(this.textBox3.Text);
                        command.Parameters.Add("@frequency", SqlDbType.Int).Value = int.Parse(this.textBox4.Text);
                        conn.Open();
                        command.ExecuteNonQuery();

                        MessageBox.Show("insert member successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
