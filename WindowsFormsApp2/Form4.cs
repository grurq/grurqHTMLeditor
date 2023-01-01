using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form4 : Form
    {
        
        public string caption,tablealign;
        public int col, row,width, border;

        public Color[] colors=new Color[2];
        // combox3=0,枠線。4=1、背景色
        public bool inputbypx = false;
        public bool ctbottom;
        public string caution;

        public int padding,spacing;


        public Form4()
        {
            ctbottom = false;
            caution = "";
            tablealign = "なし";
            row = 1;
            col = 1;
            InitializeComponent();

        }



        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tmp = (ComboBox)sender;
            if (tmp.Text == "%")
            {
                inputbypx= false;
                numericUpDown3.Enabled = true;
                numericUpDown3.Maximum = 100;
                
            }
            if (tmp.Text == "px")
            {
                inputbypx = true;
                numericUpDown3.Enabled = true;
                numericUpDown3.Maximum = 1000;
                
            }
            if (tmp.Text == "未設定")
            {
                numericUpDown3.Enabled = false;
                width = 0;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tmp = (ComboBox)sender;
            Colorpick(0, ref tmp);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            caution = textBox1.Text;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            border = (int)numericUpDown4.Value;
        }

        private void cellpadding_ValueChanged(object sender, EventArgs e)
        {
            padding = (int)cellpadding.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tmp = (ComboBox)sender;
            switch (tmp.Text)
            {
                case "左":
                    tablealign = "left";

                    break;
                case "中央":
                    tablealign = "center";

                    break;
                case "右":
                    tablealign = "right";

                    break;
                default:
                    tablealign = "なし";
                    break;

            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            col = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            row = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            width = (int)numericUpDown3.Value;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ctbottom = true;
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            caution = textBox1.Text;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ctbottom = false;
        }

        private void cellspacing_ValueChanged(object sender, EventArgs e)
        {
            spacing = (int)cellspacing.Value;
        }

        // 枠線の色、背景色

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tmp = (ComboBox)sender;
            Colorpick(1, ref tmp);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




        private void Colorpick(int num, ref ComboBox box)
        {
            if (box.Text == "選択")
            {
                ColorDialog cd = new ColorDialog();
                /* 基本16色を設定
                 * 
                black 	#000000 	 silver 	#c0c0c0 	 gray 	#808080 	 
  	            white 	#ffffff 	 maroon 	#800000 	 red 	#ff0000 	 
              	purple 	#800080 	 fuchsia 	#ff00ff 	 green 	#008000 	 
              	lime 	#00ff00 	 olive 	    #808000 	 yellow #ffff00 	 
              	navy 	#000080 	 blue 	    #0000ff 	 teal 	#008080 	 
             	aqua 	#00ffff 	
                 
                 */
                cd.CustomColors = new int[] {
                    0x000000,0xc0c0c0,0x808080,0xffffff,0x800000,0xff0000,0x800080,0xff00ff,
                    0x008000,0x00ff00,0x808000,0xffff00,0x000080,0x0000ff,0x008080,0x00ffff,
            };
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    //選択された色の取得
                    colors[num] = cd.Color;
                    box.BackColor = colors[num];
                }
            }
            else if (box.Text == "なし")
            {

                colors[num] = Color.Empty;

            }
            else
            {


                colors[num] = Color.FromName(box.Text);
                box.BackColor = Color.FromName(box.Text);

            }

        }
    }
}
