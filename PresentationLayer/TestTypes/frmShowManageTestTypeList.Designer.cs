namespace PresentationLayer.TestTypes
{
    partial class frmShowManageTestTypeList
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
            this.lblRecords = new System.Windows.Forms.Label();
            this.dgvTestType = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showCreateNewTestTypeInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showUpdateTestTypeInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCreate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestType)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRecords
            // 
            this.lblRecords.AutoSize = true;
            this.lblRecords.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecords.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.lblRecords.Location = new System.Drawing.Point(0, 432);
            this.lblRecords.Name = "lblRecords";
            this.lblRecords.Padding = new System.Windows.Forms.Padding(25, 0, 0, 5);
            this.lblRecords.Size = new System.Drawing.Size(186, 18);
            this.lblRecords.TabIndex = 10;
            this.lblRecords.Text = "# Records [0] TestType [0]";
            // 
            // dgvTestType
            // 
            this.dgvTestType.AllowUserToAddRows = false;
            this.dgvTestType.AllowUserToDeleteRows = false;
            this.dgvTestType.AllowUserToOrderColumns = true;
            this.dgvTestType.AllowUserToResizeRows = false;
            this.dgvTestType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTestType.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTestType.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTestType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTestType.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvTestType.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTestType.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTestType.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTestType.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTestType.Location = new System.Drawing.Point(23, 64);
            this.dgvTestType.Name = "dgvTestType";
            this.dgvTestType.ReadOnly = true;
            this.dgvTestType.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvTestType.RowHeadersVisible = false;
            this.dgvTestType.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTestType.Size = new System.Drawing.Size(758, 347);
            this.dgvTestType.TabIndex = 9;
            this.dgvTestType.SelectionChanged += new System.EventHandler(this.dgvTestType_SelectionChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showCreateNewTestTypeInfoToolStripMenuItem,
            this.toolStripSeparator1,
            this.showUpdateTestTypeInfoToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(260, 86);
            // 
            // showCreateNewTestTypeInfoToolStripMenuItem
            // 
            this.showCreateNewTestTypeInfoToolStripMenuItem.Image = global::PresentationLayer.Properties.Resources.ApplicationTitle;
            this.showCreateNewTestTypeInfoToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showCreateNewTestTypeInfoToolStripMenuItem.Name = "showCreateNewTestTypeInfoToolStripMenuItem";
            this.showCreateNewTestTypeInfoToolStripMenuItem.Size = new System.Drawing.Size(259, 38);
            this.showCreateNewTestTypeInfoToolStripMenuItem.Text = "Show Create New Test Type Info";
            this.showCreateNewTestTypeInfoToolStripMenuItem.Click += new System.EventHandler(this.showCreateNewTestTypeInfoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(256, 6);
            // 
            // showUpdateTestTypeInfoToolStripMenuItem
            // 
            this.showUpdateTestTypeInfoToolStripMenuItem.Image = global::PresentationLayer.Properties.Resources.edit_32;
            this.showUpdateTestTypeInfoToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showUpdateTestTypeInfoToolStripMenuItem.Name = "showUpdateTestTypeInfoToolStripMenuItem";
            this.showUpdateTestTypeInfoToolStripMenuItem.Size = new System.Drawing.Size(259, 38);
            this.showUpdateTestTypeInfoToolStripMenuItem.Text = "Show Update Test Type Info";
            this.showUpdateTestTypeInfoToolStripMenuItem.Click += new System.EventHandler(this.showUpdateTestTypeInfoToolStripMenuItem_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Image = global::PresentationLayer.Properties.Resources.ApplicationTitle;
            this.btnCreate.Location = new System.Drawing.Point(740, 12);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(45, 44);
            this.btnCreate.TabIndex = 11;
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // frmShowManageTestTypeList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.lblRecords);
            this.Controls.Add(this.dgvTestType);
            this.Name = "frmShowManageTestTypeList";
            this.Text = "Show Manage Test Types List";
            this.Load += new System.EventHandler(this.frmShowManageTestTypeList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestType)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label lblRecords;
        private System.Windows.Forms.DataGridView dgvTestType;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showCreateNewTestTypeInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem showUpdateTestTypeInfoToolStripMenuItem;
    }
}