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
            this.CoalescedGridView = new System.Windows.Forms.DataGridView();
            this.IndividualGridView = new System.Windows.Forms.DataGridView();
            this.btnMergeEval = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CoalescedGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndividualGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CoalescedGridView
            // 
            this.CoalescedGridView.AllowUserToAddRows = false;
            this.CoalescedGridView.AllowUserToDeleteRows = false;
            this.CoalescedGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CoalescedGridView.Location = new System.Drawing.Point(2, 0);
            this.CoalescedGridView.Name = "CoalescedGridView";
            this.CoalescedGridView.ReadOnly = true;
            this.CoalescedGridView.Size = new System.Drawing.Size(638, 390);
            this.CoalescedGridView.TabIndex = 1;
            this.CoalescedGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CoalescedPlayerView_CellContentClick);
            // 
            // IndividualGridView
            // 
            this.IndividualGridView.AllowUserToAddRows = false;
            this.IndividualGridView.AllowUserToDeleteRows = false;
            this.IndividualGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.IndividualGridView.Location = new System.Drawing.Point(647, 0);
            this.IndividualGridView.Name = "IndividualGridView";
            this.IndividualGridView.ReadOnly = true;
            this.IndividualGridView.Size = new System.Drawing.Size(624, 390);
            this.IndividualGridView.TabIndex = 2;
            this.IndividualGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnMergeEval
            // 
            this.btnMergeEval.Location = new System.Drawing.Point(598, 396);
            this.btnMergeEval.Name = "btnMergeEval";
            this.btnMergeEval.Size = new System.Drawing.Size(75, 24);
            this.btnMergeEval.TabIndex = 3;
            this.btnMergeEval.Text = "Merge Eval";
            this.btnMergeEval.UseVisualStyleBackColor = true;
            this.btnMergeEval.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(598, 427);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Merge Previous Season Eval";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(489, 396);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Undo Last Merge";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 500);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnMergeEval);
            this.Controls.Add(this.IndividualGridView);
            this.Controls.Add(this.CoalescedGridView);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.CoalescedGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndividualGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView CoalescedGridView;
        private System.Windows.Forms.DataGridView IndividualGridView;
        private System.Windows.Forms.Button btnMergeEval;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

