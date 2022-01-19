using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class blog : Form
    {
        public blog()
        {
            InitializeComponent();
        }
        ProjectEntities dbcontext = new ProjectEntities();
       
        private void button1_Click(object sender, EventArgs e)
        {

            this.dbcontext.Mymembers.Add(new Mymember { username =this.textBox1.Text, password = this.textBox2.Text });
            this.dbcontext.SaveChanges();
            MessageBox.Show("OK");
        }

       

        private void button2_Click_1(object sender, EventArgs e)
        {
            var q = from p in this.dbcontext.Mymembers.AsEnumerable()
                    where p.username == ($"{this.textBox1.Text}") && p.password == ($"{this.textBox2.Text}")
                    select p;


            if (q == null)
            {
                MessageBox.Show("logon fail");
            }
            else
            {
                start bg = new start();
                bg.Show();
              
            }
         
        }
    }
}
