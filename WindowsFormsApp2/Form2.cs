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
    public partial class Form2 : Form
    {
        public string imgpath;
        public string filepath;
        public string title;
        public Color[] colors= new Color[4]; // 背景、テキスト、リンク、既読リンク
        public string bgrepeat;
        public float[] texts = new float[] {-1,-1};
        public int[] margins = new int[] {-1,-1 };
        public bool[] inputbypx = new bool[] { false, false };
        public bool charset;
        public Uri relativeUri;

        public Form2()
        {
            
            // button3.DialogResult = DialogResult.Cancel;
            bgrepeat = "repeat";
            imgpath = "";
            charset=true;
            

            InitializeComponent();
        }
        #region btclick
        private void button1_Click(object sender, EventArgs e)
        {

            Button tmp = (Button)sender;
            try
            {
                imgpath =
                    OfdforPath("jpg(*.jpg;jpeg)|*.jpg;*.jpeg|png(*.png)|*.png|bmp(*.bmp)|*.bmp|gif(*.gif)|*.gif|すべてのファイル(*.*)|*.*", "画像ファイルを選択してください", true);
                textBox1.Text = imgpath;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }




        }


        private string OfdforPath(string filter, string title, bool URL) 
        
            //旧来の記述が巧く反映されなかったので、Form1よりimg
            // fpathはtextBox2の記載内容。
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = filter;
            ofd.Title = title;
            ofd.RestoreDirectory = true;
            /*
             * この一文は場所を固定するので避けた。DOBON.NETによれば現在のディレクトリが省略時参照される。
             * ofd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            */
            Uri u1 = new Uri(Form1.fpath);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Uri u2 = new Uri(ofd.FileName);
                Uri relativeUri = u1.MakeRelativeUri(u2);
                string relativeUrl = relativeUri.ToString();

                return relativeUrl;

            }
            return "";
        }
        #endregion

        #region colorset
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* 
             * 下記、1-4まで同じ処理を繰り返している。colorsの添え字は-1。
             * 選択後文字が「すべて選択」の状態になって見にくいので切ること。
             */
            ComboBox tmp = (ComboBox)sender;
            Colorpick(0, ref tmp);


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tmp = (ComboBox)sender;
            Colorpick(1, ref tmp);

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tmp = (ComboBox)sender;
            Colorpick(2, ref tmp);

        }


        private void Colorpick(int num, ref ComboBox box)
        {
            if (box.Text == "選択")
            {
                ColorDialog cd = new ColorDialog();
                // 基本16色を設定
                  
                //Black 	#000000 	 Silver 	#c0c0c0 	 Gray 	#808080 	 
  	            //White 	#ffffff 	 Maroon 	#800000 	 Red 	#ff0000 	 
              	//Purple 	#800080 	 Fuchsia 	#ff00ff 	 Green 	#008000 	 
              	//Lime 	#00ff00 	 Olive 	    #808000 	 Yellow #ffff00 	 
              	//Navy 	#000080 	 Blue 	    #0000ff 	 Teal 	#008080 	 
             	//Aqua 	#00ffff 	
                 
                 
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

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tmp = (ComboBox)sender;
            Colorpick(3, ref tmp);
        }


        #endregion

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            title = textBox3.Text;
        }

        #region pxperset


        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tmp = (ComboBox)sender;
            if (tmp.Text == "em") {
                inputbypx[0] = false;
                numericUpDown1.DecimalPlaces = 2;
                numericUpDown1.Increment = 0.05M ;
                numericUpDown2.DecimalPlaces = 2;
                numericUpDown2.Increment = 0.05M;
            }
            if (tmp.Text == "px") {
                inputbypx[0] = true;
                numericUpDown1.DecimalPlaces = 0;
                numericUpDown1.Increment = 1;
                numericUpDown2.DecimalPlaces = 0;
                numericUpDown2.Increment = 1;
            }
            
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tmp = (ComboBox)sender;
            if (tmp.Text == "%") {
                inputbypx[1] = false;
                numericUpDown3.Maximum = 100;
                numericUpDown4.Maximum = 100;
            }
            if (tmp.Text == "px")
            {
                inputbypx[1] = true;
                numericUpDown3.Maximum = 1000;
                numericUpDown4.Maximum = 1000;
            }

        }

        #endregion

        #region bgrepeat
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton tmp = (RadioButton)sender;
            if (tmp.Checked == true)
            {
                bgrepeat = "fixed";// background-attachment:fixed;

            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton tmp = (RadioButton)sender;
            if (tmp.Checked == true)
            {
                bgrepeat = "repeat";
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton tmp = (RadioButton)sender;
            if (tmp.Checked == true)
            {
                bgrepeat = "repeat-x";
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton tmp = (RadioButton)sender;
            if (tmp.Checked == true)
            {
                bgrepeat = "repeat-y";
            }
        }

        #endregion

        #region numercupdown
        /*
         *         private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
            if (comboBox5.Text == "px")
            {
                uint carry =(uint)texts[0];
                if (uint.TryParse(textBox4.Text, out carry) == true)
                {
                    texts[0] = carry;
                    textBox4.Text = texts[0].ToString();

                }
            }
            if (comboBox5.Text == "em")
            {
                float carry = texts[0];
                if (float.TryParse(textBox4.Text, out carry) == true && carry >= 0)
                {
                    texts[0] = carry;
                    textBox4.Text = texts[0].ToString();

                }
            }
           
        }

        */
        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            texts[0] = (float)numericUpDown1.Value;

        }

        private void numericUpDown2_Leave(object sender, EventArgs e)
        {
            texts[1] = (float)numericUpDown2.Value;
        }

        private void numericUpDown3_Leave(object sender, EventArgs e)
        {
            margins[0] = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_Leave(object sender, EventArgs e)
        {
            margins[1] = (int)numericUpDown4.Value;
        }



        #endregion


    }



}
