using System;
using System.Windows.Forms;

namespace WYSAPlayerRanker
{
    public partial class ErrorLogDisplay : Form
    {
        public ErrorLogDisplay(string errorText)
        {
            InitializeComponent();

            txtError.Text = errorText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
