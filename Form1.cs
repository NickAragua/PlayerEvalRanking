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
        PlayerRankingDataStore dataStore = new PlayerRankingDataStore();

        public Form1()
        {
            InitializeComponent();
            EnableIndividualGridViewDragAndDrop();
            InitializeTeamGridViewDragDrop();
            EnableRegistrantsGridViewDragDrop();
        }

        private void EnableIndividualGridViewDragAndDrop()
        {
            IndividualGridView.AllowDrop = true;
            IndividualGridView.DragEnter += IndividualGridView_DragEnter;
            IndividualGridView.DragDrop += IndividualGridView_DragDrop;
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
                dataStore.AddPlayerToTeam(selectedTeam, droppedPlayer);
            }

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CoalescedPlayerView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataStore.CreateBackup();

            foreach (SeasonPlayerData playerData in IndividualGridView.DataSource as List<SeasonPlayerData>)
            {
                dataStore.ProcessIndividualPlayer(playerData, PlayerOperationType.CurrentSeason);
            }

            CoalescedGridView.DataSource = dataStore.CoalescedPlayerDataByName.Values.ToList();
            CoalescedGridView.Refresh();
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
            TeamGridView.DataSource = dataStore.GetTeam(cboSelectedTeam.SelectedItem.ToString());
            TeamGridView.Refresh();
        }
    }
}
