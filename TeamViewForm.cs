using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WYSAPlayerRanker
{
    public partial class TeamViewForm : Form
    {
        PlayerRankingDataStore dataStore;
        public TeamViewForm(PlayerRankingDataStore dataStore)
        {
            InitializeComponent();

            this.dataStore = dataStore;
            ResizeEnd += (s, e) => RerenderViews();

            RerenderViews();
        }

        public void RerenderViews()
        {
            Controls.Clear();
            int leftOffset = 5;
            int topOffset = 5;

            foreach (var team in dataStore.Teams)
            {
                DataGridView dataGridView = new DataGridView();
                dataGridView.Left = leftOffset;
                dataGridView.Top = topOffset;
                dataGridView.Width = (int) (this.Width / 3.1);
                dataGridView.Height = (int) (this.Height / 3.1);
                dataGridView.DataSource = team.Value;
                dataGridView.Name = team.Key;
                dataGridView.Refresh();
                this.Controls.Add(dataGridView);

                leftOffset += dataGridView.Width + 5;
                if (leftOffset + dataGridView.Width > this.Width)
                {
                    topOffset += dataGridView.Height + 5;
                    leftOffset = 5;
                }
            }
        }
    }
}
