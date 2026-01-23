using System;
using System.Windows.Forms;

namespace WYSAPlayerRanker
{
    public partial class SettingsDialog : Form
    {
        private PlayerRankingDataStore DataStore { get; set; }
        private ApplicationSettings appSettings => DataStore.ApplicationSettings;

        public SettingsDialog()
        {
            InitializeComponent();
        }

        public void UpdateDataStore(PlayerRankingDataStore dataStore)
        {
            DataStore = dataStore;
            txtCurrentSeasonWeight.Text = appSettings.CurrentSeasonWeight.ToString();
            txtPrevSeasonWeight.Text = appSettings.PreviousSeasonWeight.ToString();
            txtEvalWeight.Text = appSettings.EvalWeight.ToString();
            txtDiv1Weight.Text = appSettings.DivisionWeights[0].ToString();
            txtDiv2Weight.Text = appSettings.DivisionWeights[1].ToString();
            txtDiv3Weight.Text = appSettings.DivisionWeights[2].ToString();
            txtDiv4Weight.Text = appSettings.DivisionWeights[3].ToString();
            txtDiv5Weight.Text = appSettings.DivisionWeights[4].ToString();
            chkAutosave.Checked = appSettings.AutoSave;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateWeight(txtCurrentSeasonWeight, lblCurrentSeasonWeight) ||
                !ValidateWeight(txtPrevSeasonWeight, lblPreviousSeasonWeight) ||
                !ValidateWeight(txtEvalWeight, lblEvalWeight) ||
                !ValidateWeight(txtDiv1Weight, lblDiv1Weight) ||
                !ValidateWeight(txtDiv2Weight, lblDiv2Weight) ||
                !ValidateWeight(txtDiv3Weight, lblDiv3Weight) ||
                !ValidateWeight(txtDiv4Weight, lblDiv4Weight) ||
                !ValidateWeight(txtDiv5Weight, lblDiv5Weight)) 
            { 
                return; 
            }

            appSettings.CurrentSeasonWeight = Double.Parse(txtCurrentSeasonWeight.Text);
            appSettings.PreviousSeasonWeight = Double.Parse(txtPrevSeasonWeight.Text);
            appSettings.EvalWeight = Double.Parse(txtEvalWeight.Text);
            appSettings.DivisionWeights[0] = Double.Parse(txtDiv1Weight.Text);
            appSettings.DivisionWeights[1] = Double.Parse(txtDiv2Weight.Text);
            appSettings.DivisionWeights[2] = Double.Parse(txtDiv3Weight.Text);
            appSettings.DivisionWeights[3] = Double.Parse(txtDiv4Weight.Text);
            appSettings.DivisionWeights[4] = Double.Parse(txtDiv5Weight.Text);

            appSettings.AutoSave = chkAutosave.Checked;

            DataStore.RecalculatePlayerScores();

            ((Form1) Owner).RefreshViews();

            Close();
        }

        private bool ValidateWeight(TextBox source, Label associatedLabel)
        {
            if (!Double.TryParse(source.Text, out _))
            {
                MessageBox.Show($"{associatedLabel.Text} must be a decimal number.");
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
