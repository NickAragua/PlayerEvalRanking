using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WYSAPlayerRanker
{
    public partial class Form1 : Form
    {
        PlayerRankingDataStore dataStore = new PlayerRankingDataStore();

        public Form1()
        {
            InitializeComponent();
            EnableIndividualGridViewDragAndDrop();
            InitializeTeamGridViewDragDrop();
            InitializeCoalescedGridViewDragDrop();
            EnableRegistrantsGridViewDragDrop();
        }

        private void EnableIndividualGridViewDragAndDrop()
        {
            IndividualGridView.AllowDrop = true;
            IndividualGridView.DragEnter += IndividualGridView_DragEnter;
            IndividualGridView.DragDrop += IndividualGridView_DragDrop;
        }

        private void InitializeCoalescedGridViewDragDrop()
        {
            /*TeamGridView.MouseDown += TeamGridView_MouseDown;

            // Configure CoalescedGridView as drop target
            CoalescedGridView.AllowDrop = true;
            CoalescedGridView.DragEnter += CoalescedGridView_DragEnter;
            CoalescedGridView.DragDrop += CoalescedGridView_DragDrop;*/
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
                    TeamGridView.DoDragDrop(selectedPlayer, DragDropEffects.Move);
                }
            }
        }

        private void CoalescedGridView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(CoalescedPlayerData)))
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
            if (sender != TeamGridView ||
                !e.Data.GetDataPresent(typeof(CoalescedPlayerData)))
            {
                return;
            }

            CoalescedPlayerData droppedPlayer = e.Data.GetData(typeof(CoalescedPlayerData)) as CoalescedPlayerData;
            string selectedTeam = cboSelectedTeam.SelectedItem.ToString();

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
                    CoalescedGridView.DoDragDrop(selectedPlayer, DragDropEffects.Move);
                }
            }
        }

        private void TeamGridView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(CoalescedPlayerData)))
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
            if (!e.Data.GetDataPresent(typeof(CoalescedPlayerData)))
            {
                return;
            }

            // Verify a team is selected
            if (cboSelectedTeam.SelectedItem == null)
            {
                MessageBox.Show("Please select a team first.", "No Team Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CoalescedPlayerData droppedPlayer = e.Data.GetData(typeof(CoalescedPlayerData)) as CoalescedPlayerData;
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
    }
}
