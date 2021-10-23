namespace MoneyArchiveApp {
    partial class FormMain {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listAccounts = new System.Windows.Forms.ListBox();
            this.gridTransactions = new System.Windows.Forms.DataGridView();
            this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.ChequeNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Payee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunningTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Transfer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Memo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransactions)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listAccounts);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridTransactions);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 120;
            this.splitContainer1.TabIndex = 0;
            // 
            // listAccounts
            // 
            this.listAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listAccounts.FormattingEnabled = true;
            this.listAccounts.ItemHeight = 15;
            this.listAccounts.Location = new System.Drawing.Point(0, 0);
            this.listAccounts.Name = "listAccounts";
            this.listAccounts.Size = new System.Drawing.Size(120, 450);
            this.listAccounts.TabIndex = 0;
            this.listAccounts.SelectedIndexChanged += new System.EventHandler(this.listAccounts_SelectedIndexChanged);
            // 
            // gridTransactions
            // 
            this.gridTransactions.AllowUserToAddRows = false;
            this.gridTransactions.AllowUserToDeleteRows = false;
            this.gridTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTransactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChequeNumber,
            this.Date,
            this.Payee,
            this.Amount,
            this.RunningTotal,
            this.Category,
            this.Transfer,
            this.Memo});
            this.gridTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTransactions.Location = new System.Drawing.Point(0, 0);
            this.gridTransactions.Name = "gridTransactions";
            this.gridTransactions.ReadOnly = true;
            this.gridTransactions.RowTemplate.Height = 25;
            this.gridTransactions.Size = new System.Drawing.Size(676, 450);
            this.gridTransactions.TabIndex = 0;
            // 
            // dlgFolder
            // 
            this.dlgFolder.Description = "Select the Qif Folder";
            this.dlgFolder.ShowNewFolderButton = false;
            this.dlgFolder.UseDescriptionForTitle = true;
            // 
            // ChequeNumber
            // 
            this.ChequeNumber.DataPropertyName = "ChequeNumber";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ChequeNumber.DefaultCellStyle = dataGridViewCellStyle1;
            this.ChequeNumber.HeaderText = "Cheque";
            this.ChequeNumber.Name = "ChequeNumber";
            this.ChequeNumber.ReadOnly = true;
            // 
            // Date
            // 
            this.Date.DataPropertyName = "Date";
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // Payee
            // 
            this.Payee.DataPropertyName = "Payee";
            this.Payee.HeaderText = "Payee";
            this.Payee.Name = "Payee";
            this.Payee.ReadOnly = true;
            // 
            // Amount
            // 
            this.Amount.DataPropertyName = "Amount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle2;
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            // 
            // RunningTotal
            // 
            this.RunningTotal.DataPropertyName = "RunningTotal";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.RunningTotal.DefaultCellStyle = dataGridViewCellStyle3;
            this.RunningTotal.HeaderText = "Total";
            this.RunningTotal.Name = "RunningTotal";
            this.RunningTotal.ReadOnly = true;
            // 
            // Category
            // 
            this.Category.DataPropertyName = "Category";
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            // 
            // Transfer
            // 
            this.Transfer.DataPropertyName = "Transfer";
            this.Transfer.HeaderText = "Transfer";
            this.Transfer.Name = "Transfer";
            this.Transfer.ReadOnly = true;
            // 
            // Memo
            // 
            this.Memo.DataPropertyName = "Memo";
            this.Memo.HeaderText = "Memo";
            this.Memo.Name = "Memo";
            this.Memo.ReadOnly = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormMain";
            this.Text = "Money Archive";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransactions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listAccounts;
        private System.Windows.Forms.DataGridView gridTransactions;
        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChequeNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Payee;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunningTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Transfer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Memo;
    }
}
