using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shitcastClient
{
    public partial class inputBox : Form
    {

        public static string label;
        public static string button;
        public string box { get; set; }

        public inputBox(string flabel, string fbutton)
        {
            InitializeComponent();
            label = flabel;
            button = fbutton;
        }

        private void inputBox_Load(object sender, EventArgs e)
        {
            label1.Text = label;
            button1.Text = button;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("fill the box, or it'll become sad", ":<", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            box = textBox1.Text;
            this.Close();
        }
    }
}
