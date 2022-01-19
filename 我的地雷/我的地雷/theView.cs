using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 我的地雷
{
    public partial class theView : Form
    {
        public NumericUpDown the_upDown_rows = new NumericUpDown();
        public NumericUpDown the_upDown_columns = new NumericUpDown();
        public theView()
        {
            InitializeComponent();

            the_upDown_rows.Location = new Point(110, 60);
            the_upDown_rows.Minimum = 3;
            the_upDown_rows.Maximum = 5;
            Controls.Add(the_upDown_rows);

            the_upDown_columns.Location = new Point(110, 130);
            the_upDown_columns.Maximum = 5;
            the_upDown_columns.Minimum = 3;
            Controls.Add(the_upDown_columns);
          
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
