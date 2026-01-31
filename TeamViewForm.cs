using System.Collections.Generic;
using System.Windows.Forms;

namespace WYSAPlayerRanker
{
    public partial class TeamViewForm : Form
    {
        private Dictionary<string, Label> teamLabels = new Dictionary<string, Label>();

        private class DragDropData
        {
            public CoalescedPlayerData PlayerData { get; set; }
            public string SourceTeam { get; set; }
        }

        PlayerRankingDataStore dataStore;
        public TeamViewForm(PlayerRankingDataStore dataStore)
        {
            InitializeComponent();

            this.dataStore = dataStore;
            FormClosed += (s, e) => { ((Form1)this.Owner).RefreshTeamGridView(); };

            RenderViews();
        }

        public void RefreshGrids()
        {
            foreach (Control control in Controls)
            {
                if (control is DataGridView gridView)
                {
                    gridView.DataSource = null;
                    gridView.DataSource = dataStore.Teams[gridView.Name];
                    gridView.Refresh();
                }
            }
        }

        public void RenderViews()
        {
            Controls.Clear();
            int leftOffset = 5;
            int topOffset = 5;
            int gridCount = 1;
            int gridHeight = 0;

            foreach (var team in dataStore.Teams)
            {
                Label lblTeam = new Label();
                lblTeam.Left = leftOffset;
                lblTeam.Top = topOffset;
                lblTeam.Text = team.Key;
                Controls.Add(lblTeam); ;

                Label lblPlayerCount = new Label();
                lblPlayerCount.Name = team.Key;
                lblPlayerCount.Left = leftOffset + lblTeam.Width + 5;
                lblPlayerCount.Top = topOffset;
                lblPlayerCount.Text = $"Players: {team.Value.Count}";
                Controls.Add(lblPlayerCount);

                DataGridView dataGridView = new DataGridView();
                dataGridView.Left = leftOffset;
                dataGridView.Top = topOffset + lblTeam.Height;
                dataGridView.Width = (int) (Screen.PrimaryScreen.Bounds.Width / 3.2);
                dataGridView.Height = gridHeight = dataGridView.ColumnHeadersHeight * 17;
                team.Value.Sort((x, y) => -x.CombinedScore.CompareTo(y.CombinedScore)); // descending order
                dataGridView.DataSource = team.Value;
                dataGridView.Name = team.Key;
                dataGridView.DefaultCellStyle.Format = "0.00";
                dataGridView.Refresh();

                EnableDragAndDrop(dataGridView, team.Key);
                Controls.Add(dataGridView);

                leftOffset += dataGridView.Width + 5;
                if (gridCount % 3 == 0)
                {
                    topOffset += dataGridView.Height + lblTeam.Height + 10;
                    leftOffset = 5;
                }

                gridCount++;
            }

            this.Width = Screen.PrimaryScreen.Bounds.Width - 20;
            this.Height = (gridHeight + topOffset) * gridCount / 3;
        }

        private void EnableDragAndDrop(DataGridView gridView, string teamName)
        {
            gridView.AllowDrop = true;
            gridView.MouseDown += (s, e) => DataGridView_MouseDown(s, e, teamName);
            gridView.DragEnter += DataGridView_DragEnter;
            gridView.DragDrop += (s, e) => DataGridView_DragDrop(s, e, teamName);
        }

        private void DataGridView_MouseDown(object sender, MouseEventArgs e, string sourceTeam)
        {
            DataGridView gridView = sender as DataGridView;
            if (gridView == null)
            {
                return;
            }

            int rowIndex = gridView.HitTest(e.X, e.Y).RowIndex;

            if (rowIndex < 0 || rowIndex >= gridView.Rows.Count)
            {
                return;
            }

            CoalescedPlayerData selectedPlayer = gridView.Rows[rowIndex].DataBoundItem as CoalescedPlayerData;

            if (selectedPlayer != null)
            {
                DragDropData dropData = new DragDropData
                {
                    PlayerData = selectedPlayer,
                    SourceTeam = sourceTeam
                };
                gridView.DoDragDrop(dropData, DragDropEffects.Move);
            }
        }

        private void DataGridView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DragDropData)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void DataGridView_DragDrop(object sender, DragEventArgs e, string targetTeam)
        {
            if (!e.Data.GetDataPresent(typeof(DragDropData)))
            {
                return;
            }

            DragDropData dropData = e.Data.GetData(typeof(DragDropData)) as DragDropData;

            if (dropData == null)
            {
                return;
            }

            // Prevent dropping on the same team
            if (dropData.SourceTeam == targetTeam)
            {
                return;
            }

            CoalescedPlayerData droppedPlayer = dropData.PlayerData;

            if (droppedPlayer != null)
            {
                // Remove from source team and add to target team
                dataStore.RemovePlayerFromTeam(droppedPlayer, dropData.SourceTeam);
                dataStore.MovePlayerToTeam(droppedPlayer, targetTeam);
                dataStore.GetTeam(targetTeam).Sort((x, y) => -x.CombinedScore.CompareTo(y.CombinedScore)); // descending order

                Controls[dropData.SourceTeam].Text = $"Players: {dataStore.GetTeam(dropData.SourceTeam).Count}";
                Controls[targetTeam].Text = $"Players: {dataStore.GetTeam(targetTeam).Count}";
            }

            // Refresh the view to reflect changes
            RefreshGrids();
        }
    }
}
