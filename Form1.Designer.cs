namespace WYSAPlayerRanker
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.CoalescedGridView = new System.Windows.Forms.DataGridView();
            this.IndividualGridView = new System.Windows.Forms.DataGridView();
            this.btnMergeEval = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.TeamGridView = new System.Windows.Forms.DataGridView();
            this.cboSelectedTeam = new System.Windows.Forms.ComboBox();
            this.btnAddNewTeam = new System.Windows.Forms.Button();
            this.btnRemoveTeam = new System.Windows.Forms.Button();
            this.RegistrantsGridView = new System.Windows.Forms.DataGridView();
            this.btnSaveState = new System.Windows.Forms.Button();
            this.btnLoadState = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnTeamView = new System.Windows.Forms.Button();
            this.contextMenuTeam = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.generateTeamMailingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.CoalescedGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndividualGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TeamGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegistrantsGridView)).BeginInit();
            this.contextMenuTeam.SuspendLayout();
            this.SuspendLayout();
            // 
            // CoalescedGridView
            // 
            this.CoalescedGridView.AllowUserToAddRows = false;
            this.CoalescedGridView.AllowUserToOrderColumns = true;
            this.CoalescedGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = "N/A";
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CoalescedGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.CoalescedGridView.Location = new System.Drawing.Point(2, 0);
            this.CoalescedGridView.Name = "CoalescedGridView";
            this.CoalescedGridView.Size = new System.Drawing.Size(638, 390);
            this.CoalescedGridView.TabIndex = 1;
            // 
            // IndividualGridView
            // 
            this.IndividualGridView.AllowUserToAddRows = false;
            this.IndividualGridView.AllowUserToDeleteRows = false;
            this.IndividualGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "N/A";
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.IndividualGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.IndividualGridView.Location = new System.Drawing.Point(647, 0);
            this.IndividualGridView.Name = "IndividualGridView";
            this.IndividualGridView.ReadOnly = true;
            this.IndividualGridView.Size = new System.Drawing.Size(624, 390);
            this.IndividualGridView.TabIndex = 2;
            // 
            // btnMergeEval
            // 
            this.btnMergeEval.Location = new System.Drawing.Point(647, 397);
            this.btnMergeEval.Name = "btnMergeEval";
            this.btnMergeEval.Size = new System.Drawing.Size(75, 24);
            this.btnMergeEval.TabIndex = 3;
            this.btnMergeEval.Text = "Merge Eval";
            this.btnMergeEval.UseVisualStyleBackColor = true;
            this.btnMergeEval.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(728, 398);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Merge Previous Season Eval";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(894, 398);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Undo Last Merge";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // TeamGridView
            // 
            this.TeamGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = "N/A";
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.TeamGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.TeamGridView.Location = new System.Drawing.Point(2, 427);
            this.TeamGridView.Name = "TeamGridView";
            this.TeamGridView.Size = new System.Drawing.Size(638, 431);
            this.TeamGridView.TabIndex = 6;
            // 
            // cboSelectedTeam
            // 
            this.cboSelectedTeam.FormattingEnabled = true;
            this.cboSelectedTeam.Location = new System.Drawing.Point(2, 397);
            this.cboSelectedTeam.Name = "cboSelectedTeam";
            this.cboSelectedTeam.Size = new System.Drawing.Size(121, 21);
            this.cboSelectedTeam.TabIndex = 7;
            this.cboSelectedTeam.SelectedIndexChanged += new System.EventHandler(this.cboSelectedTeam_SelectedIndexChanged);
            // 
            // btnAddNewTeam
            // 
            this.btnAddNewTeam.Location = new System.Drawing.Point(130, 398);
            this.btnAddNewTeam.Name = "btnAddNewTeam";
            this.btnAddNewTeam.Size = new System.Drawing.Size(93, 23);
            this.btnAddNewTeam.TabIndex = 8;
            this.btnAddNewTeam.Text = "Add New Team";
            this.btnAddNewTeam.UseVisualStyleBackColor = true;
            this.btnAddNewTeam.Click += new System.EventHandler(this.btnAddNewTeam_Click);
            // 
            // btnRemoveTeam
            // 
            this.btnRemoveTeam.Location = new System.Drawing.Point(229, 397);
            this.btnRemoveTeam.Name = "btnRemoveTeam";
            this.btnRemoveTeam.Size = new System.Drawing.Size(101, 23);
            this.btnRemoveTeam.TabIndex = 9;
            this.btnRemoveTeam.Text = "Remove Team";
            this.btnRemoveTeam.UseVisualStyleBackColor = true;
            this.btnRemoveTeam.Click += new System.EventHandler(this.button3_Click);
            // 
            // RegistrantsGridView
            // 
            this.RegistrantsGridView.AllowUserToAddRows = false;
            this.RegistrantsGridView.AllowUserToDeleteRows = false;
            this.RegistrantsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RegistrantsGridView.Location = new System.Drawing.Point(647, 428);
            this.RegistrantsGridView.Name = "RegistrantsGridView";
            this.RegistrantsGridView.ReadOnly = true;
            this.RegistrantsGridView.Size = new System.Drawing.Size(624, 430);
            this.RegistrantsGridView.TabIndex = 10;
            // 
            // btnSaveState
            // 
            this.btnSaveState.Location = new System.Drawing.Point(337, 397);
            this.btnSaveState.Name = "btnSaveState";
            this.btnSaveState.Size = new System.Drawing.Size(75, 23);
            this.btnSaveState.TabIndex = 12;
            this.btnSaveState.Text = "Save";
            this.btnSaveState.UseVisualStyleBackColor = true;
            this.btnSaveState.Click += new System.EventHandler(this.btnSaveState_Click);
            // 
            // btnLoadState
            // 
            this.btnLoadState.Location = new System.Drawing.Point(419, 398);
            this.btnLoadState.Name = "btnLoadState";
            this.btnLoadState.Size = new System.Drawing.Size(75, 23);
            this.btnLoadState.TabIndex = 13;
            this.btnLoadState.Text = "Load";
            this.btnLoadState.UseVisualStyleBackColor = true;
            this.btnLoadState.Click += new System.EventHandler(this.btnLoadState_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(501, 397);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 14;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(1184, 399);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSettings.TabIndex = 15;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnTeamView
            // 
            this.btnTeamView.Location = new System.Drawing.Point(1000, 399);
            this.btnTeamView.Name = "btnTeamView";
            this.btnTeamView.Size = new System.Drawing.Size(75, 23);
            this.btnTeamView.TabIndex = 16;
            this.btnTeamView.Text = "Team View";
            this.btnTeamView.UseVisualStyleBackColor = true;
            this.btnTeamView.Click += new System.EventHandler(this.btnTeamView_Click);
            // 
            // contextMenuTeam
            // 
            this.contextMenuTeam.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateTeamMailingListToolStripMenuItem});
            this.contextMenuTeam.Name = "contextMenuTeam";
            this.contextMenuTeam.Size = new System.Drawing.Size(217, 48);
            // 
            // generateTeamMailingListToolStripMenuItem
            // 
            this.generateTeamMailingListToolStripMenuItem.Name = "generateTeamMailingListToolStripMenuItem";
            this.generateTeamMailingListToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.generateTeamMailingListToolStripMenuItem.Text = "Generate Team Mailing List";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 870);
            this.Controls.Add(this.btnTeamView);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnLoadState);
            this.Controls.Add(this.btnSaveState);
            this.Controls.Add(this.RegistrantsGridView);
            this.Controls.Add(this.btnRemoveTeam);
            this.Controls.Add(this.btnAddNewTeam);
            this.Controls.Add(this.cboSelectedTeam);
            this.Controls.Add(this.TeamGridView);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnMergeEval);
            this.Controls.Add(this.IndividualGridView);
            this.Controls.Add(this.CoalescedGridView);
            this.Name = "Form1";
            this.Text = "Player Evaluation Manager";
            ((System.ComponentModel.ISupportInitialize)(this.CoalescedGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndividualGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TeamGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegistrantsGridView)).EndInit();
            this.contextMenuTeam.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView CoalescedGridView;
        private System.Windows.Forms.DataGridView IndividualGridView;
        private System.Windows.Forms.Button btnMergeEval;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView TeamGridView;
        private System.Windows.Forms.ComboBox cboSelectedTeam;
        private System.Windows.Forms.Button btnAddNewTeam;
        private System.Windows.Forms.Button btnRemoveTeam;
        private System.Windows.Forms.DataGridView RegistrantsGridView;
        private System.Windows.Forms.Button btnSaveState;
        private System.Windows.Forms.Button btnLoadState;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnTeamView;
        private System.Windows.Forms.ContextMenuStrip contextMenuTeam;
        private System.Windows.Forms.ToolStripMenuItem generateTeamMailingListToolStripMenuItem;
    }
}

