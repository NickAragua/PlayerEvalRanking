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
            EnableDragAndDrop();
        }

        private void EnableDragAndDrop()
        {
            AllowDrop = true;
            DragEnter += Form1_DragEnter;
            DragDrop += Form1_DragDrop;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
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

        private void Form1_DragDrop(object sender, DragEventArgs e)
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
    }
}
