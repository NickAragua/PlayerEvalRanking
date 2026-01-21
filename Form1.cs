using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WYSAPlayerRanker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var test = ExcelPlayerDataLoader.LoadPlayersFromExcel("D:\\Downloads\\Soccer\\AgeDirector\\Fall2025\\Evals\\G56-6-CoachName.xlsx");
        }
    }
}
