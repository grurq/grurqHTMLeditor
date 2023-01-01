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
    public partial class Form5 : Form
    {
        public static RichTextBox rich;
        public Form5()
        {
            
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            rich.Parent = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //まずは下方健作のみ
            

            
        }
    }
}
