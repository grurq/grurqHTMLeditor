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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            richTextBox1.Text= Properties.Settings.Default.F1;
            richTextBox2.Text= Properties.Settings.Default.F2;
            richTextBox3.Text= Properties.Settings.Default.F3;
            richTextBox4.Text= Properties.Settings.Default.F4;
            richTextBox5.Text= Properties.Settings.Default.F5;
            richTextBox6.Text= Properties.Settings.Default.F6;
            richTextBox7.Text= Properties.Settings.Default.F7;
            richTextBox8.Text= Properties.Settings.Default.F8;
            richTextBox9.Text= Properties.Settings.Default.F9;
            richTextBox10.Text= Properties.Settings.Default.F10;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.F1 = richTextBox1.Text;
            Properties.Settings.Default.F2 = richTextBox2.Text;
            Properties.Settings.Default.F3 = richTextBox3.Text;
            Properties.Settings.Default.F4 = richTextBox4.Text;
            Properties.Settings.Default.F5 = richTextBox5.Text;
            Properties.Settings.Default.F6 = richTextBox6.Text;
            Properties.Settings.Default.F7 = richTextBox7.Text;
            Properties.Settings.Default.F8 = richTextBox8.Text;
            Properties.Settings.Default.F9 = richTextBox9.Text;
            Properties.Settings.Default.F10 = richTextBox10.Text;
       
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.F1 = "<p> </p>";
            Properties.Settings.Default.F2 = "<br>";
            Properties.Settings.Default.F3 = "<b> </b>";
            Properties.Settings.Default.F4 = "<u> </u>";
            Properties.Settings.Default.F5 = "<i> </i>";
            Properties.Settings.Default.F6 = "<a href=\"\"> </a>";
            Properties.Settings.Default.F7 = "<img src=\"\">";
            Properties.Settings.Default.F8 = "<blockquote> </blockquote>";
            Properties.Settings.Default.F9 = "<hr>";
            Properties.Settings.Default.F10  = "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\r\n" ;
            Properties.Settings.Default.F10 += "<head>\r\n";
            Properties.Settings.Default.F10 += "<meta http-equiv=\"Content - StyleChanged - Type\" content=\"text / css\">\r\n";
            Properties.Settings.Default.F10 += "<title>\r\n";
            Properties.Settings.Default.F10 += "\r\n";
            Properties.Settings.Default.F10 += "</title>\r\n";
            Properties.Settings.Default.F10 += "</head>\r\n";
            Properties.Settings.Default.F10 += "<body>\r\n";
            Properties.Settings.Default.F10 += "\r\n";
            Properties.Settings.Default.F10 += "</body>\r\n";
            Properties.Settings.Default.F10 += "</html>";

            Properties.Settings.Default.Save();
            richTextBox1.Text = Properties.Settings.Default.F1;
            richTextBox2.Text = Properties.Settings.Default.F2;
            richTextBox3.Text = Properties.Settings.Default.F3;
            richTextBox4.Text = Properties.Settings.Default.F4;
            richTextBox5.Text = Properties.Settings.Default.F5;
            richTextBox6.Text = Properties.Settings.Default.F6;
            richTextBox7.Text = Properties.Settings.Default.F7;
            richTextBox8.Text = Properties.Settings.Default.F8;
            richTextBox9.Text = Properties.Settings.Default.F9;
            richTextBox10.Text = Properties.Settings.Default.F10;
            


            /*
             <!DOCTYPE HTML PUBLIC " -//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<head>
<meta http-equiv="Content-StyleChanged-Type" content="text/css">
<title>

</title>
</head>
<body>

</body>
</html>
             
             */

        }
    }
}
