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
    public partial class infomation : Form
    {
        
        public infomation()
        {
            InitializeComponent();
        }

        private void infomation_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            label2.Text = "ver."+ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
      }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            //ブラウザで開く
            System.Diagnostics.Process.Start("https://github.com/grurq/grurqHTMLeditor");
        }
    
    }
}
