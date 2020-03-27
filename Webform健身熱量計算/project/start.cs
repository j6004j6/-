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
    public partial class start : Form
    {
        public start()
        {
            InitializeComponent();
            this.pictureBox2.AllowDrop = true;
            this.pictureBox2.DragDrop += PictureBox2_DragDrop;
            this.pictureBox2.DragEnter += PictureBox2_DragEnter;
            //==================
            this.pictureBox4.AllowDrop = true;
            this.pictureBox4.DragDrop += PictureBox4_DragDrop;
            this.pictureBox4.DragEnter += PictureBox4_DragEnter;
            //==================
            this.pictureBox3.AllowDrop = true;
            this.pictureBox3.DragEnter += PictureBox3_DragEnter;
            this.pictureBox3.DragDrop += PictureBox3_DragDrop;
            //=============================
            this.tabPage1.BackColor = Color.LightBlue;
            this.tabPage2.BackColor = Color.LightGoldenrodYellow;
            this.tabPage3.BackColor = Color.LightGray;
            this.tabPage4.BackColor = Color.LightSalmon;
            //===============================================


            var q = from p in this.dbcontext.Mymeats
                    group p by p.foodname into g
                    select new { g.Key };
            foreach (var item in q)
            {
                this.comboBox2.Items.Add(item.Key);
            }
            this.comboBox2.SelectedIndex = this.comboBox2.Items.Count-3;
            //===============================
            var J = from K in this.dbcontext.Myvegetables
                    group K by K.foodname into H
                    select new { H.Key };
            foreach (var item in J)
            {
                this.comboBox3.Items.Add(item.Key);
            }
            this.comboBox3.SelectedIndex = 6;
            //=================================
            var L = from M in this.dbcontext.Myvegetables
                    group M by M.foodname into N
                    select new { N.Key };
            foreach (var item in L)
            {
                this.comboBox4.Items.Add(item.Key);
            }
            this.comboBox4.SelectedIndex = 6;
            //==================================
            var R = from S in this.dbcontext.Myvegetables
                    group S by S.foodname into T
                    select new { T.Key };
            foreach (var item in L)
            {
                this.comboBox5.Items.Add(item.Key);
            }
            this.comboBox5.SelectedIndex = 6;
            //===================================
            var U = from V in this.dbcontext.Mydrinks
                    group V by V.foodname into W
                    select new { W.Key };
            foreach (var item in U)
            {
                this.comboBox6.Items.Add(item.Key);
            }
            this.comboBox6.SelectedIndex = 2;
            //=============================
            var exercise = (from i in this.dbcontext.Myexercises
                     group i by i.partname into z
                     select new { z.Key });
            foreach (var item in exercise)
            {
                this.comboBox7.Items.Add(item.Key);
            }
            this.comboBox7.SelectedIndex = this.comboBox7.Items.Count - 1;
            //this.dataGridView1.DataSource = exercise.ToList();


        }

        private void PictureBox3_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
            this.pictureBox3.Image = Image.FromFile(filenames[0]);
        }

        private void PictureBox3_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void PictureBox4_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void PictureBox4_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
            this.pictureBox4.Image = Image.FromFile(filenames[0]);
        }

        private void PictureBox2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void PictureBox2_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
            this.pictureBox2.Image = Image.FromFile(filenames[0]);
        }

        ProjectEntities dbcontext = new ProjectEntities();
        private void button1_Click(object sender, EventArgs e)
        {
            double h = double.Parse(this.textBox2.Text);
            double w = double.Parse(this.textBox3.Text);
            double y = double.Parse(this.textBox4.Text);
            //===========================


            if (this.comboBox1.Text == "男")
            {
                double BMR = (13.7 * w) + (5.0 * h) - (6.8 * y) + 66;
                this.textBox5.Text = BMR.ToString();
                var q = (from p in this.dbcontext.Mypictures
                         where p.male == this.comboBox1.Text
                         select new { p.picture }).Single();

                byte[] bytes = q.picture;
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                this.pictureBox1.Image = Image.FromStream(ms);


            }
            else if (this.comboBox1.Text == "女")
            {
                double BMR = (9.6 * w) + (1.8 * h) - (4.7 * y) + 655;
                this.textBox5.Text = BMR.ToString();
                var q = (from p in this.dbcontext.Mypictures
                         where p.male == this.comboBox1.Text
                         select new { p.picture }).Single();

                byte[] bytes = q.picture;
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                this.pictureBox1.Image = Image.FromStream(ms);

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                byte[] bytes = { 1, 3 };
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                this.pictureBox2.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bytes = ms.GetBuffer();

                this.dbcontext.Mymeats.Add(new Mymeat { foodname = this.textBox6.Text, picture = bytes, calories = int.Parse(this.textBox7.Text) });

                this.dbcontext.SaveChanges();
                MessageBox.Show("insert ok");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] bytes = { 1, 3 };
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            this.pictureBox4.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            bytes = ms.GetBuffer();

            this.dbcontext.Mydrinks.Add(new Mydrink { foodname = this.textBox11.Text, picture = bytes, calories = int.Parse(this.textBox10.Text) });

            this.dbcontext.SaveChanges();
            MessageBox.Show("insert ok");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] bytes = { 1, 3 };
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            this.pictureBox3.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            bytes = ms.GetBuffer();

            this.dbcontext.Myvegetables.Add(new Myvegetable { foodname = this.textBox9.Text, picture = bytes, calories = int.Parse(this.textBox8.Text) });

            this.dbcontext.SaveChanges();
            MessageBox.Show("insert ok");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            var q = (from p in this.dbcontext.Mymeats
                     where p.foodname == this.comboBox2.Text
                     select new { p.picture }).Single();

            byte[] bytes = q.picture;
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            this.pictureBox5.Image = Image.FromStream(ms);

            //=====================
            var J = (from K in this.dbcontext.Mymeats
                     where K.foodname == this.comboBox2.Text
                     select new { K.calories }).Single();

            //this.textBox1.Text = q.calorin.ToString();
            int b = J.calories.Value;
            this.textBox13.Text = b.ToString();
            // MessageBox.Show("this number=" + b);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var q = (from p in this.dbcontext.Myvegetables
                     where p.foodname == this.comboBox3.Text
                     select new { p.picture }).Single();

            byte[] bytes = q.picture;
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            this.pictureBox6.Image = Image.FromStream(ms);

            //=====================
            var J = (from K in this.dbcontext.Myvegetables
                     where K.foodname == this.comboBox3.Text
                     select new { K.calories }).Single();

            //this.textBox1.Text = q.calorin.ToString();
            int c = J.calories.Value;
            this.textBox14.Text = c.ToString();
            // MessageBox.Show("this number=" + b);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            var q = (from p in this.dbcontext.Myvegetables
                     where p.foodname == this.comboBox4.Text
                     select new { p.picture }).Single();

            byte[] bytes = q.picture;
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            this.pictureBox7.Image = Image.FromStream(ms);

            //=====================
            var J = (from K in this.dbcontext.Myvegetables
                     where K.foodname == this.comboBox4.Text
                     select new { K.calories }).Single();

            //this.textBox1.Text = q.calorin.ToString();
            int d = J.calories.Value;
            this.textBox15.Text = d.ToString();
            // MessageBox.Show("this number=" + b);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            var q = (from p in this.dbcontext.Myvegetables
                     where p.foodname == this.comboBox5.Text
                     select new { p.picture }).Single();

            byte[] bytes = q.picture;
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            this.pictureBox8.Image = Image.FromStream(ms);

            //=====================
            var J = (from K in this.dbcontext.Myvegetables
                     where K.foodname == this.comboBox5.Text
                     select new { K.calories }).Single();

            //this.textBox1.Text = q.calorin.ToString();
            int f = J.calories.Value;
            this.textBox16.Text = f.ToString();
            // MessageBox.Show("this number=" + b);
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

            var q = (from p in this.dbcontext.Mydrinks
                     where p.foodname == this.comboBox6.Text
                     select new { p.picture }).Single();

            byte[] bytes = q.picture;
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            this.pictureBox9.Image = Image.FromStream(ms);

            //=====================
            var J = (from K in this.dbcontext.Mydrinks
                     where K.foodname == this.comboBox6.Text
                     select new { K.calories }).Single();

            //this.textBox1.Text = q.calorin.ToString();
            int a = J.calories.Value;
            this.textBox17.Text = a.ToString();
            // MessageBox.Show("this number=" + b);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int a = int.Parse(this.textBox13.Text);
            int b = int.Parse(this.textBox14.Text);
            int c = int.Parse(this.textBox15.Text);
            int d = int.Parse(this.textBox16.Text);
            int f = int.Parse(this.textBox17.Text);
            int total = a + b + c + d + f;
            this.textBox12.Text = total.ToString();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try

            {
                this.textBox19.DataBindings.Clear();
                this.pictureBox10.DataBindings.Clear();
                var q = from p in this.dbcontext.Myexercises
                        where p.partname == this.comboBox7.Text
                        select p;

                this.bindingSource1.DataSource = q.ToList();
                this.dataGridView1.DataSource = bindingSource1;
                this.bindingNavigator1.BindingSource = this.bindingSource1;

                //=================================================
                this.textBox19.DataBindings.Add("Text", this.bindingSource1, "frequency");
                this.pictureBox10.DataBindings.Add("Image", this.bindingSource1, "picture", true);


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //private void button7_Click(object sender, EventArgs e)
        //{ //更改資料
        //    var q = (from p in this.dbcontext.Myexercises
        //             where p.partname == "背"
        //             select p).FirstOrDefault();
        //    //MessageBox.Show("123" + q.partname);

        //    if (q == null) return;

        //    q.partname = "背部" ;

        //    this.dbcontext.SaveChanges();
        //    MessageBox.Show("ok");
        //}

        private void button6_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbcontext.Myexercises.AsEnumerable()
                    where p.partname == this.comboBox7.Text
                    select p;
            int total = 0;
            foreach (var item in q)
            {
                total += item.calories.Value;
            }
            this.textBox18.Text = total.ToString();             
        }
    }
 }
   

