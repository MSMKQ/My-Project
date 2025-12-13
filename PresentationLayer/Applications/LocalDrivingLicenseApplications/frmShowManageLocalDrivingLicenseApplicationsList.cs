using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Applications.LocalDrivingLicenseApplications
{
    public partial class frmShowManageLocalDrivingLicenseApplicationsList : Form
    {
        private DataTable _Locals;

        public static int SelectedRow;

        public frmShowManageLocalDrivingLicenseApplicationsList()
        {
            InitializeComponent();
        }

        private int? _SelectedRow()
        {
            if (dgvApplications.CurrentRow?.Cells[0].Value == null || dgvApplications.CurrentRow?.Cells[0].Value == DBNull.Value)
                return 0;

            if (int.TryParse(dgvApplications.CurrentRow.Cells[0].Value.ToString(), out int value))
                return value;

            return 0;
        }

        private void _Records()
        {
            lblRecords.Text = $"# Records [{dgvApplications.RowCount}] L.D.L.App No [{_SelectedRow().Value}]";
            SelectedRow = _SelectedRow().Value;
        }

        private void _table()
        {
            if (dgvApplications.RowCount > 0)
            {
                var headers = new (string colName, string colText, DataGridViewAutoSizeColumnMode Mode)[]
                {
                    ("LocalDrivingLicenseApplicationID", "L.D.L.App No", DataGridViewAutoSizeColumnMode.AllCells),
                    ("Title", "License Class", DataGridViewAutoSizeColumnMode.NotSet),
                    ("FullName", "Full Name", DataGridViewAutoSizeColumnMode.NotSet),
                    ("NationID", "Nation ID", DataGridViewAutoSizeColumnMode.AllCells),
                    ("ApplicationDate", "Date", DataGridViewAutoSizeColumnMode.AllCells),
                    ("ApplicationStatus", "Status", DataGridViewAutoSizeColumnMode.AllCells),
                    ("LastStatusDate", "Last Status", DataGridViewAutoSizeColumnMode.AllCells),
                    ("Username", "Created By", DataGridViewAutoSizeColumnMode.AllCells)
                };

                foreach ( var header in headers )
                {
                    var col = dgvApplications.Columns[header.colName];

                    if ( col != null )
                    {
                        col.HeaderText = header.colText;
                        col.AutoSizeMode = header.Mode;
                    }
                }

                cbFilter.SelectedIndex = 0;
            }

            _Records();
        }

        private void frmShowManageLocalDrivingLicenseApplicationsList_Load(object sender, EventArgs e)
        {
            dgvApplications.ClearSelection();
            _Locals = ClsLocalDrivingLicenseApplication.GetApplications();
            dgvApplications.DataSource = _Locals;
            _table();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilter.Text != "None");

            if (cbFilter.Text == "None")
            {
                _Locals.DefaultView.RowFilter = null;
            }
            else if (txtFilterValue.Visible)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }

            _Records();
        }

        private void dgvApplications_SelectionChanged(object sender, EventArgs e)
        {
            _Records();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if ( MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.CreateLocalDrivingLicenseApplicationInfo);
            }

            frmShowManageLocalDrivingLicenseApplicationsList_Load(null, null);
        }

        private void showUpdateApplicationInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ( MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.UpdateLocalDrivingLicenseApplicationInfo);
            }

            frmShowManageLocalDrivingLicenseApplicationsList_Load(null, null);
        }

        private void showDeleteApplicationInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Id = _SelectedRow().Value;

            if (MessageBox.Show($"Are you sure you want delete this Application No.{Id} ?", $"Delete Application No.{Id}", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if ( ClsLocalDrivingLicenseApplication.Delete(Id) )
            {
                MessageBox.Show($"This Local Driving License Application No.{Id} Deleted successfully.", $"Application Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmShowManageLocalDrivingLicenseApplicationsList_Load(null, null);
            }
        }

        private void showCancelApplicationInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Id = _SelectedRow().Value;

            if (MessageBox.Show($"Are you sure you want cancel this Application No.{Id} ?", $"Cancel Application No.{Id}", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            ClsLocalDrivingLicenseApplication _Local = ClsLocalDrivingLicenseApplication.GetInfoByID(Id);

            if (_Local != null)
            {
                if (_Local.Cancel())
                {
                    MessageBox.Show($"This Application No.{Id} was cancelled successfully.", $"Application Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmShowManageLocalDrivingLicenseApplicationsList_Load(null, null);
                }
            }
            else
            {
                MessageBox.Show($"Error cannot cancel this application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int Id = _SelectedRow().Value;

            ClsLocalDrivingLicenseApplication _Local = ClsLocalDrivingLicenseApplication.GetInfoByID(Id);

            if ( _Local == null )
            {
                MessageBox.Show($"Error: This Local Driving License Application No.{Id} was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool IsEnabled = (_Local.ApplicationStatus == (byte)ClsApplication.EnStatus.New);

            tsmiUpdateApplication.Enabled = IsEnabled;
            tsmiShowDelete.Enabled = IsEnabled;
            tsmiShowCancel.Enabled = IsEnabled;

            bool VisionTestPassed = _Local.DoesPassedTest(ClsTestType.EnTestType.VisionTest);
            bool WrittenTestPassed = _Local.DoesPassedTest(ClsTestType.EnTestType.WrittenTest);
            bool StreetTestPassed = _Local.DoesPassedTest(ClsTestType.EnTestType.StreetTest);

            tsmiScheduleTests.Enabled = !VisionTestPassed && !WrittenTestPassed && !StreetTestPassed && IsEnabled;

            tsmiVisionTest.Enabled = !VisionTestPassed;
            tsmiWrittenTest.Enabled = VisionTestPassed && !WrittenTestPassed;
            tsmiStreetTest.Enabled = VisionTestPassed && WrittenTestPassed && !StreetTestPassed;


        }

        private void showLocalDrivingLicenseApplicationInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ShowLocalDrivingLicenseApplicationInfo);
            }
        }
    }
}
