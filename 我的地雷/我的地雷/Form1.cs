using System;
using System.Collections;
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
    public partial class Form1 : Form
    {
        theView f2 = new theView();  //建立控制表格的東西
        static int howmany_rows=3;   //給一個初值row
        static int howmany_columns=4;  //給一個初值columns
        static int 目前翻幾個;         //用來計算翻了幾個，要可以往上加所以宣告成全域變數static
        static int howMany_grid; //?  總共有幾格 要改成活得所以要弄個變數
        static int[,] grid;     //       數字地圖    
        static Button[,] the_btns;      //用來翻的ui button 蓋在數字地圖上
        
        static int 翻幾個就贏了;       //用來計算的
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //開新局的按鈕事件
            f2.the_upDown_rows.Value = Form1.howmany_rows;
            //把初值給表格二
            f2.the_upDown_columns.Value = howmany_columns;
            f2.ShowDialog(); //強制回應的表格

            Form1.howmany_rows = (int)f2.the_upDown_rows.Value;
            Form1.howmany_columns = (int)f2.the_upDown_columns.Value;

            the_new_game(); //建立新的遊戲 邏輯
            the_new_ui();   //建立畫面    View

        }

        void the_new_ui()
        {
            if (the_btns != null)
            {
                for (int R = 0; R < the_btns.GetLength(0); R++)
                {
                    for (int C = 0; C < the_btns.GetLength(1); C++)
                    {
                        Controls.Remove(the_btns[R, C]);
                    }
                }
            }

            the_btns = new Button[howmany_rows, howmany_columns];
            for (int R = 0; R < howmany_rows; R++)
            {
                for (int C = 0; C < howmany_columns; C++)
                {
                    Button btn = new Button();


                    btn.MouseDown += Btn_MouseDown;  //註冊事件
                    btn.Tag = grid[R, C];  //將數字地圖的數字跟tag屬性綁定
                    // fix #2 when you are done testing, comment the next line
                    //btn.Text = btn.Tag + "";

                    the_btns[R, C] = btn;

                    btn.Location = new Point(C * 30, R * 30);
                    btn.Size = new Size(30, 30);
                    Controls.Add(btn);
                }
            }
        }

        private void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button target = (Button)sender;

            if (e.Button == MouseButtons.Left)
            {
                // A. 無效翻,已被標註是地雷
                if (target.Image != null)
                {
                    Console.WriteLine("無效翻,已被標註是地雷");
                    return;
                }
                // B. 無效翻,已被翻過
                //Console.WriteLine("無效翻,已被翻過");
                // C. Console.WriteLine("翻,定生死");
                if (((int)target.Tag) < 0)
                {
                    Console.WriteLine("炸,定死");
                    MessageBox.Show("Sorry, you stepped on a bomb, a new game");
                    button1_Click(null, null);
                }
                else
                {
                    Console.WriteLine("不是炸彈, 決定是否已贏了(尚未寫出程式來知道是否贏了)");
                    target.Text = "" + target.Tag;
                    target.Enabled = false;

                    目前翻幾個 += 1;
                    if (目前翻幾個 == 翻幾個就贏了)
                    {
                        // fix # teacher  贏了要做什是還沒寫好
                        Console.WriteLine("贏了");
                        MessageBox.Show("Congratulations, you are the person, a new game");
                        button1_Click(null, null);
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                // target.Image = Image.FromFile("D:/吳旭騰的觀念/我的地雷/我的地雷/mine.png");
                if (target.Image == null)
                {
                    target.Image = Image.FromFile("D:/吳旭騰的觀念/我的地雷/我的地雷/mine.png");
                    Console.WriteLine("註記它可能是地雷");
                }
                else
                {
                    target.Image = null;
                    Console.WriteLine("取消它可能是地雷的註記");
                }

            }
        }

        //用了靜態的物件所以搭配靜態的方法，不需要提供回傳值所以用void;
        static void the_new_game()
        {
            目前翻幾個 = 0;

            grid = new int[howmany_rows, howmany_columns]; //數字矩陣跟howmany_r c綁定
            howMany_grid = howmany_columns * howmany_rows; //矩陣總數

            the_print_map();
            theGenRandom();  //布雷
            Console.WriteLine();
            Console.WriteLine();

            the_print_map();
            theCalulate();
            the_print_map();
            int howManyBombs = 0;
            for (int i = 0; i < howmany_rows; i++)
            {
                for (int C = 0; C < howmany_columns; C++)
                {
                    if (grid[i, C] < 0)
                    {
                        howManyBombs += 1;
                    }
                }
            }
            Console.WriteLine("howManyBombs 為 " + howManyBombs);
            翻幾個就贏了 = howmany_columns * howmany_rows - howManyBombs;
            Console.WriteLine("翻幾個就贏了    為  " + 翻幾個就贏了);
        }

        static void theGenRandom()   //放雷  
        {
            ArrayList ts = new ArrayList();
            int p = 0;
            while (p < howMany_grid)  //使用到howMany_grid 來控p
            {
                ts.Add(p);
                Console.WriteLine(p);
                p += 1;

            }
            Console.Write(ts);
            int k = 1;
            Random RN = new Random();

            while (k <= 2)
            {
                int kd = RN.Next(ts.Count);
                Console.Write(kd);
                int si = (int)ts[kd];
                Console.Write(si);
                grid[si / howmany_columns, si % howmany_columns] = -9;
                Console.WriteLine(ts[kd]);
                ts.RemoveAt(kd);
                k += 1;
            }
        }
        static void the_print_map()
        {
            int R = 0;
            while (R < howmany_rows)
            {
                int k = 0;
                while (k < howmany_columns)
                {
                    if (grid[R, k] >= 0)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(grid[R, k] + "    ");
                    k += 1;
                }
                Console.WriteLine();
                R += 1;
            }
            Console.WriteLine("##########################################");
        }
        static void theCalulate()  //數雷
        {
            int R = 0;
            while (R < howmany_rows)
            {
                int k = 0;
                while (k < howmany_columns)
                {
                    int howManyMines = grid[R, k];
                    if (howManyMines >= 0)// it is not a bomb
                    {
                        int h_min = (k - 1 < 0) ? 0 : k - 1;
                        int h_max = (k + 1 >= howmany_columns) ? howmany_columns - 1 : k + 1;
                        int v_min = (R - 1 < 0) ? 0 : R - 1;
                        int v_max = (R + 1 >= howmany_rows) ? howmany_rows - 1 : R + 1;

                        int howManyBombs = 0;
                        int ROW = v_min;
                        while (ROW <= v_max)
                        {
                            int COLUMN = h_min;
                            while (COLUMN <= h_max)
                            {
                                if (grid[ROW, COLUMN] < 0)
                                {
                                    howManyBombs += 1;
                                }
                                COLUMN += 1;
                            }

                            ROW += 1;
                        }
                        grid[R, k] = howManyBombs;
                    }

                    k += 1;
                }

                R += 1;
            }
            Console.WriteLine("##########################################");
        }
    }
}
