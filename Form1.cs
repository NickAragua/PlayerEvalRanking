using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WYSAPlayerRanker
{
    public partial class Form1 : Form
    {
        private class DragDropData
        {
            public CoalescedPlayerData PlayerData { get; set; }
            public string SourceGridView { get; set; }
        }

        PlayerRankingDataStore dataStore = new PlayerRankingDataStore();

        private SortOrder coalescedSortOrder = SortOrder.Ascending;
        private string coalescedSortColumn = string.Empty;

        public Form1()
        {
            InitializeComponent();
            EnableIndividualGridViewDragAndDrop();
            InitializeTeamGridViewDragDrop();
            InitializeCoalescedGridViewDragDrop();
            EnableRegistrantsGridViewDragDrop();
            InitializeCoalescedGridViewSorting();
            InitializeGridViewFormatting();
        }

        private void InitializeGridViewFormatting()
        {
            CoalescedGridView.CellFormatting += CoalescedGridView_CellFormatting;
            TeamGridView.CellFormatting += TeamGridView_CellFormatting;
        }

        private void InitializeCoalescedGridViewSorting()
        {
            CoalescedGridView.ColumnHeaderMouseClick += CoalescedGridView_ColumnHeaderMouseClick;
        }

        private void EnableIndividualGridViewDragAndDrop()
        {
            IndividualGridView.AllowDrop = true;
            IndividualGridView.DragEnter += IndividualGridView_DragEnter;
            IndividualGridView.DragDrop += IndividualGridView_DragDrop;
        }

        private void InitializeCoalescedGridViewDragDrop()
        {
            TeamGridView.MouseDown += TeamGridView_MouseDown;

            // Configure CoalescedGridView as drop target
            CoalescedGridView.AllowDrop = true;
            CoalescedGridView.DragEnter += CoalescedGridView_DragEnter;
            CoalescedGridView.DragDrop += CoalescedGridView_DragDrop;
        }

        private void InitializeTeamGridViewDragDrop()
        {
            // Configure CoalescedGridView as drag source
            CoalescedGridView.MouseDown += CoalescedGridView_MouseDown;

            // Configure TeamGridView as drop target
            TeamGridView.AllowDrop = true;
            TeamGridView.DragEnter += TeamGridView_DragEnter;
            TeamGridView.DragDrop += TeamGridView_DragDrop;
        }

        private void EnableRegistrantsGridViewDragDrop()
        {
            // Configure TeamGridView as drop target
            RegistrantsGridView.AllowDrop = true;
            RegistrantsGridView.DragEnter += RegistrantsGridView_DragEnter;
            RegistrantsGridView.DragDrop += RegistrantsGridView_DragDrop;
        }

        private void TeamGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (TeamGridView.HitTest(e.X, e.Y).RowIndex >= 0)
            {
                int rowIndex = TeamGridView.HitTest(e.X, e.Y).RowIndex;

                if (rowIndex < 0 || rowIndex >= TeamGridView.Rows.Count)
                {
                    return;
                }

                CoalescedPlayerData selectedPlayer = TeamGridView.Rows[rowIndex].DataBoundItem as CoalescedPlayerData;

                if (selectedPlayer != null)
                {
                    DragDropData dropData = new DragDropData
                    {
                        PlayerData = selectedPlayer,
                        SourceGridView = "TeamGridView"
                    };
                    TeamGridView.DoDragDrop(dropData, DragDropEffects.Move);
                }
            }
        }

        private void CoalescedGridView_DragEnter(object sender, DragEventArgs e)
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

        private void CoalescedGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(DragDropData)))
            {
                return;
            }

            DragDropData dropData = e.Data.GetData(typeof(DragDropData)) as DragDropData;
            if (dropData == null || dropData.SourceGridView != "TeamGridView")
            {
                return;
            }

            CoalescedPlayerData droppedPlayer = dropData.PlayerData;
            string selectedTeam = cboSelectedTeam?.SelectedItem?.ToString();

            if (droppedPlayer != null)
            {
                // Add player to the selected team
                dataStore.RemovePlayerFromTeam(droppedPlayer, selectedTeam);
            }

            CoalescedGridView.DataSource = null;
            CoalescedGridView.DataSource = dataStore.CoalescedPlayerDataByName.Values.ToList();
            CoalescedGridView.Refresh();

            // Refresh TeamGridView with the updated team roster
            TeamGridView.ClearSelection();
            TeamGridView.DataSource = null;
            TeamGridView.DataSource = dataStore.GetTeam(selectedTeam);
            TeamGridView.Refresh();
        }

        private void CoalescedGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (CoalescedGridView.HitTest(e.X, e.Y).RowIndex >= 0)
            {
                int rowIndex = CoalescedGridView.HitTest(e.X, e.Y).RowIndex;
                CoalescedPlayerData selectedPlayer = CoalescedGridView.Rows[rowIndex].DataBoundItem as CoalescedPlayerData;

                if (selectedPlayer != null)
                {
                    DragDropData dropData = new DragDropData
                    {
                        PlayerData = selectedPlayer,
                        SourceGridView = "CoalescedGridView"
                    };
                    CoalescedGridView.DoDragDrop(dropData, DragDropEffects.Move);
                }
            }
        }

        private void TeamGridView_DragEnter(object sender, DragEventArgs e)
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

        private void TeamGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(DragDropData)))
            {
                return;
            }

            // Verify a team is selected
            if (cboSelectedTeam.SelectedItem == null)
            {
                MessageBox.Show("Please select a team first.", "No Team Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DragDropData dropData = e.Data.GetData(typeof(DragDropData)) as DragDropData;

            if (dropData == null || dropData.SourceGridView != "CoalescedGridView")
            {
                return;
            }

            CoalescedPlayerData droppedPlayer = dropData.PlayerData;
            string selectedTeam = cboSelectedTeam.SelectedItem.ToString();

            if (droppedPlayer != null)
            {
                // Add player to the selected team
                dataStore.MovePlayerToTeam(droppedPlayer, selectedTeam);
            }

            CoalescedGridView.DataSource = null;
            CoalescedGridView.DataSource = dataStore.CoalescedPlayerDataByName.Values.ToList();
            CoalescedGridView.Refresh();

            // Refresh TeamGridView with the updated team roster
            TeamGridView.DataSource = null;
            TeamGridView.DataSource = dataStore.GetTeam(selectedTeam);
            TeamGridView.Refresh();
        }

        private void IndividualGridView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Any(f => f.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase)))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void IndividualGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string excelFile = files.FirstOrDefault(f => f.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase));

                if (excelFile != null)
                {
                    var loadedData = ExcelPlayerDataLoader.LoadPlayersFromExcel(excelFile);
                    IndividualGridView.DataSource = loadedData;
                    IndividualGridView.Refresh();
                }
            }
        }

        private void RegistrantsGridView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Any(f => f.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase)))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void RegistrantsGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string excelFile = files.FirstOrDefault(f => f.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase));

                if (excelFile != null)
                {
                    var loadedData = ExcelPlayerDataLoader.LoadRegisteredPlayers(excelFile);
                    dataStore.ProcessRegisteredPlayers(loadedData);
                    RegistrantsGridView.DataSource = loadedData;
                    RegistrantsGridView.Refresh();
                    CoalescedGridView.Refresh();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (IndividualGridView.DataSource == null)
            {
                MessageBox.Show("Please load individual player data first by dragging and dropping an Excel file onto the Individual Player Data grid.", "No Data Loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dataStore.CreateBackup();

            foreach (SeasonPlayerData playerData in IndividualGridView.DataSource as List<SeasonPlayerData>)
            {
                dataStore.ProcessIndividualPlayer(playerData, PlayerOperationType.CurrentSeason);
            }

            CoalescedGridView.DataSource = dataStore.CoalescedPlayerDataByName.Values.ToList();
            CoalescedGridView.Refresh();

            IndividualGridView.DataSource = null;
            IndividualGridView.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataStore.CreateBackup();

            foreach (SeasonPlayerData playerData in IndividualGridView.DataSource as List<SeasonPlayerData>)
            {
                dataStore.ProcessIndividualPlayer(playerData, PlayerOperationType.PreviousSeason);
            }

            CoalescedGridView.DataSource = dataStore.CoalescedPlayerDataByName.Values.ToList();
            CoalescedGridView.Refresh();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dataStore.RestoreFromBackup();
            CoalescedGridView.DataSource = dataStore.CoalescedPlayerDataByName.Values.ToList();
            CoalescedGridView.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataStore.RemoveTeam(cboSelectedTeam.SelectedItem.ToString());
            cboSelectedTeam.DataSource = dataStore.GetTeamNames();
            cboSelectedTeam.Refresh();
        }

        private void btnAddNewTeam_Click(object sender, EventArgs e)
        {
            dataStore.AddNewTeam();
            cboSelectedTeam.DataSource = dataStore.GetTeamNames();
            cboSelectedTeam.SelectedIndex = cboSelectedTeam.Items.Count - 1;
            cboSelectedTeam.Refresh();
        }

        private void cboSelectedTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSelectedTeam.SelectedItem != null)
            {
                TeamGridView.DataSource = dataStore.GetTeam(cboSelectedTeam.SelectedItem.ToString());
                TeamGridView.Refresh();
            }
        }

        private void CoalescedGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = CoalescedGridView.Columns[e.ColumnIndex].DataPropertyName;

            if (string.IsNullOrEmpty(columnName))
            {
                columnName = CoalescedGridView.Columns[e.ColumnIndex].Name;
            }

            // Toggle sort order if clicking the same column
            if (coalescedSortColumn == columnName)
            {
                coalescedSortOrder = coalescedSortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                coalescedSortColumn = columnName;
                coalescedSortOrder = SortOrder.Ascending;
            }

            SortCoalescedGridView(columnName, coalescedSortOrder);
        }

        private void SortCoalescedGridView(string columnName, SortOrder sortOrder)
        {
            var dataSource = CoalescedGridView.DataSource as List<CoalescedPlayerData>;

            if (dataSource == null)
            {
                return;
            }

            IEnumerable<CoalescedPlayerData> sortedData = dataSource;

            switch (columnName)
            {
                case "FullName":
                    sortedData = sortOrder == SortOrder.Ascending
                        ? dataSource.OrderBy(p => p.FullName)
                        : dataSource.OrderByDescending(p => p.FullName);
                    break;
                case "CurrentSeasonScore":
                    sortedData = sortOrder == SortOrder.Ascending
                        ? dataSource.OrderBy(p => p.CurrentSeasonScore)
                        : dataSource.OrderByDescending(p => p.CurrentSeasonScore);
                    break;
                case "PreviousSeasonScore":
                    sortedData = sortOrder == SortOrder.Ascending
                        ? dataSource.OrderBy(p => p.PreviousSeasonScore)
                        : dataSource.OrderByDescending(p => p.PreviousSeasonScore);
                    break;
                case "EvalScore":
                    sortedData = sortOrder == SortOrder.Ascending
                        ? dataSource.OrderBy(p => p.EvalScore)
                        : dataSource.OrderByDescending(p => p.EvalScore);
                    break;
                case "GradeLevel":
                    sortedData = sortOrder == SortOrder.Ascending
                        ? dataSource.OrderBy(p => p.GradeLevel)
                        : dataSource.OrderByDescending(p => p.GradeLevel);
                    break;
                case "HasRedFlag":
                    sortedData = sortOrder == SortOrder.Ascending
                        ? dataSource.OrderBy(p => p.HasRedFlag)
                        : dataSource.OrderByDescending(p => p.HasRedFlag);
                    break;
                case "HasAssociatedCoach":
                    sortedData = sortOrder == SortOrder.Ascending
                        ? dataSource.OrderBy(p => p.HasAssociatedCoach)
                        : dataSource.OrderByDescending(p => p.HasAssociatedCoach);
                    break;
                case "PreviousTeam":
                    sortedData = sortOrder == SortOrder.Ascending
                        ? dataSource.OrderBy(p => p.PreviousTeam)
                        : dataSource.OrderByDescending(p => p.PreviousTeam);
                    break;
            }

            CoalescedGridView.DataSource = sortedData.ToList();
            CoalescedGridView.Refresh();
        }

        private void CoalescedGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (CoalescedGridView.Rows[e.RowIndex].DataBoundItem is CoalescedPlayerData playerData)
            {
                if (playerData.HasRedFlag)
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
                }
                else if (!dataStore.PlayerIsRegistered(playerData))
                {
                    e.CellStyle.BackColor = Color.Orange;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void TeamGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (TeamGridView.Rows[e.RowIndex].DataBoundItem is CoalescedPlayerData playerData)
            {
                if (playerData.HasRedFlag)
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
                }
                else if (!dataStore.PlayerIsRegistered(playerData)) 
                {
                    e.CellStyle.BackColor = Color.Orange;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void btnSaveState_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                saveFileDialog.DefaultExt = "json";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = "PlayerRankingState";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dataStore.Serialize(saveFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving state: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnLoadState_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.DefaultExt = "json";
                openFileDialog.CheckFileExists = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dataStore = PlayerRankingDataStore.Deserialize(openFileDialog.FileName);

                        // Refresh all grids to display loaded data
                        CoalescedGridView.DataSource = dataStore.CoalescedPlayerDataByName.Values.ToList();
                        CoalescedGridView.Refresh();

                        cboSelectedTeam.DataSource = dataStore.GetTeamNames();
                        cboSelectedTeam.Refresh();

                        if (cboSelectedTeam.Items.Count > 0)
                        {
                            cboSelectedTeam.SelectedIndex = 0;
                        }

                        TeamGridView.DataSource = dataStore.GetTeam(cboSelectedTeam.SelectedItem.ToString());
                        TeamGridView.Refresh();

                        RegistrantsGridView.DataSource = dataStore.RegisteredPlayers.Values.ToList();
                        RegistrantsGridView.Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading state: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
