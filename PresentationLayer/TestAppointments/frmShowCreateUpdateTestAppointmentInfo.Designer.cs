namespace PresentationLayer.TestAppointments
{
    partial class frmShowCreateUpdateTestAppointmentInfo
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ctrlShowCreateUpdateTestAppointmentInfo1 = new PresentationLayer.TestAppointments.Controls.ctrlShowCreateUpdateTestAppointmentInfo();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 480F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 620F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(605, 643);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ctrlShowCreateUpdateTestAppointmentInfo1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(65, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 614);
            this.panel1.TabIndex = 0;
            // 
            // ctrlShowCreateUpdateTestAppointmentInfo1
            // 
            this.ctrlShowCreateUpdateTestAppointmentInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlShowCreateUpdateTestAppointmentInfo1.Location = new System.Drawing.Point(0, 0);
            this.ctrlShowCreateUpdateTestAppointmentInfo1.Name = "ctrlShowCreateUpdateTestAppointmentInfo1";
            this.ctrlShowCreateUpdateTestAppointmentInfo1.Padding = new System.Windows.Forms.Padding(10);
            this.ctrlShowCreateUpdateTestAppointmentInfo1.Size = new System.Drawing.Size(472, 612);
            this.ctrlShowCreateUpdateTestAppointmentInfo1.TabIndex = 2;
            this.ctrlShowCreateUpdateTestAppointmentInfo1.TestTypeID = BusinessLayer.ClsTestType.EnTestType.VisionTest;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(65, 624);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "<< Back";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // frmShowCreateUpdateTestAppointmentInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 643);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmShowCreateUpdateTestAppointmentInfo";
            this.Text = "Mood";
            this.Load += new System.EventHandler(this.frmShowCreateUpdateTestAppointmentInfo_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private Controls.ctrlShowCreateUpdateTestAppointmentInfo ctrlShowCreateUpdateTestAppointmentInfo1;
        private System.Windows.Forms.Label label1;
    }
}