using BusinessLayer;
using PresentationLayer.InterFaces;
using PresentationLayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.TestAppointments
{
    public partial class frmShowManageTestAppointmentsList : Form, ILoadableForm
    {
        private int? _LocalDrivingLicenseApplicationID;
        private ClsTestType.EnTestType _TestTypeID;
        private DataTable _TestTypes;
        public static int SelectedRow;
        public frmShowManageTestAppointmentsList(int? LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _HandleTestType();
        }

        public int? _SelectedRow()
        {
            if (dgvTestAppointments.CurrentRow?.Cells[0].Value == null || dgvTestAppointments.CurrentRow?.Cells[0].Value == DBNull.Value)
                return 0;

            if (int.TryParse(dgvTestAppointments.CurrentRow.Cells[0].Value.ToString(), out int SelectedRow))
                return SelectedRow;

            return 0;
        }

        private void _Records()
        {
            int Id = _SelectedRow().Value; SelectedRow = Id;
            lblRecords.Text = $"# Records [{dgvTestAppointments.RowCount}] TestApp [{Id}]";
        }

        private void _HandleTestType()
        {
            ClsLocalDrivingLicenseApplication _Local = ClsLocalDrivingLicenseApplication.GetInfoByID(_LocalDrivingLicenseApplicationID);

            if (_Local == null)
            {
                MessageBox.Show($"This Local Driving License Application NO.{_LocalDrivingLicenseApplicationID} was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool VisionTest = _Local.DoesPassedTest(ClsTestType.EnTestType.VisionTest);
            bool WrittenTest = _Local.DoesPassedTest(ClsTestType.EnTestType.WrittenTest);
            bool StreetTest = _Local.DoesPassedTest(ClsTestType.EnTestType.StreetTest);

            if (!VisionTest)
                _TestTypeID = ClsTestType.EnTestType.VisionTest;
            else if (VisionTest && !WrittenTest)
                _TestTypeID = ClsTestType.EnTestType.WrittenTest;
            else if (VisionTest && WrittenTest && !StreetTest)
                _TestTypeID = ClsTestType.EnTestType.StreetTest;

            ctrlShowLocalDrivingLicenseApplicationInfo1.LoadInfo(_LocalDrivingLicenseApplicationID);
        }

        public void LoadInfo(int? LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            _HandleTestType();
            frmShowManageTestAppointmentsList_Load(null, null);
        }

        private void _LoadPic()
        {
            switch (_TestTypeID)
            {
                case ClsTestType.EnTestType.VisionTest:
                    pbTestTypes.Image = Resources.Vision_512;
                    break;

                case ClsTestType.EnTestType.WrittenTest:
                    pbTestTypes.Image = Resources.Written_Test_512;
                    break;

                case ClsTestType.EnTestType.StreetTest:
                    pbTestTypes.Image = Resources.driving_test_512;
                    break;
            }
        }

        private void _table()
        {
            if (dgvTestAppointments.RowCount > 0)
            {
                var headers = new (string name, string header, DataGridViewAutoSizeColumnMode Mode)[]
                {
                    ("TestAppointmentID","TestApp No", DataGridViewAutoSizeColumnMode.AllCells ),
                    ("AppointmentDate", "Date", DataGridViewAutoSizeColumnMode.NotSet),
                    ("PaidFees", "Paid Fees", DataGridViewAutoSizeColumnMode.NotSet),
                    ("Username", "User Name", DataGridViewAutoSizeColumnMode.NotSet),
                    ("IsLocked", "Is Locked", DataGridViewAutoSizeColumnMode.AllCells)
                };

                foreach (var head in headers)
                {
                    var Col = dgvTestAppointments.Columns[head.name];

                    if (Col != null)
                    {
                        Col.HeaderText = head.header;
                        Col.AutoSizeMode = head.Mode;
                    }
                }
            }

            _Records();
        }

        private void frmShowManageTestAppointmentsList_Load(object sender, EventArgs e)
        {
            _LoadPic();
            ctrlShowLocalDrivingLicenseApplicationInfo1.LoadInfo(_LocalDrivingLicenseApplicationID);
            dgvTestAppointments.ClearSelection();
            _TestTypes = ClsTestAppointment.GetAppointments(_LocalDrivingLicenseApplicationID, _TestTypeID);
            dgvTestAppointments.DataSource = _TestTypes;
            _table();
        }

        private void dgvTestAppointments_SelectionChanged(object sender, EventArgs e)
        {
            _Records();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ClsTestAppointment.IsThereAnActiveAppointment(_LocalDrivingLicenseApplicationID, _SelectedRow().Value))
            {
                MessageBox.Show("There is already an active appointment.", "Already ther appointment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MdiParent is frmShowNewMainMenue parent)
                {
                    parent.ShowForm(frmShowNewMainMenue.EnForm.CreateTestAppointment);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManageLocalDrivingLicenseApplications);
            }
        }
    }
}
