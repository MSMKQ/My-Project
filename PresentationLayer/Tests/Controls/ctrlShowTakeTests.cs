using BusinessLayer;
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

namespace PresentationLayer.Tests.Controls
{
    public partial class ctrlShowTakeTests : UserControl
    {
        private ClsLocalDrivingLicenseApplication _Local;
        private ClsTestAppointment _TestAppointment;
        private ClsTest _Test;
        private int? _LocalDrivingLicenseApplicationID;
        private int? _TestAppointmentID;
        private ClsTestType.EnTestType _TestTypeID;

        public ctrlShowTakeTests()
        {
            InitializeComponent();
        }

        private void _ResetToDefault()
        {
            lblLocalDrivingLicenseApplicationID.Text = "[ ?? ]";
            lblLicenseClass.Text = "[ ?????? ]";
            lblName.Text = "[ ?????? ]";
            lblAppointmentDate.Text = "[ ??/??/???? ]";
            lblFees.Text = "[ $$$ ]";
            lblTestID.Text = "Not Taken Still";
        }

        private void _LoadLocalDrivingLicenseApplicationInfo()
        {
            _Local = ClsLocalDrivingLicenseApplication.GetInfoByID(_LocalDrivingLicenseApplicationID);

            if (_Local == null)
            {
                _ResetToDefault();

                MessageBox.Show($"This Local Driving License Application No.{_LocalDrivingLicenseApplicationID} was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLocalDrivingLicenseApplicationID.Text = _Local.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = _Local.LicenseClassInfo.Title;
            lblName.Text = _Local.FullName;
            lblTrails.Text = ClsTestAppointment.TotalTrails(_LocalDrivingLicenseApplicationID, _TestTypeID).ToString();
        }

        private void _LoadTestAppointmentInfo()
        {
            _TestAppointment = ClsTestAppointment.GetInfoByID(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                _ResetToDefault();
                MessageBox.Show($"This Test Appointment No.{_TestAppointmentID} was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblFees.Text = _TestAppointment.PaidFees.Value.ToString("N3");
            lblAppointmentDate.Text = _TestAppointment.AppointmentDate.Value.ToShortDateString();
            lblTestID.Text = (_TestAppointment.TestID.HasValue) ? _TestAppointment.TestID.Value.ToString() : "Not Taken Still";
        }

        private void _LoadTestTypeInfo()
        {
            switch ( _TestTypeID )
            {
                case ClsTestType.EnTestType.VisionTest:
                    gbTestType.Text = "Vision Test";
                    pbTestTypeID.Image = Resources.Vision_512;
                    break;

                case ClsTestType.EnTestType.WrittenTest:
                    gbTestType.Text = "Written Test";
                    pbTestTypeID.Image = Resources.Written_Test_512;
                    break;

                case ClsTestType.EnTestType.StreetTest:
                    gbTestType.Text = "Street Test";
                    pbTestTypeID.Image = Resources.driving_test_512;
                    break;
            }
        }

        public void LoadTestInfo(int? LocalDrivingLicenseApplicationID, int? TestAppointmentID, ClsTestType.EnTestType TestTypeID)
        {
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = TestAppointmentID;
            _TestTypeID = TestTypeID;

            _LoadLocalDrivingLicenseApplicationInfo();

            _LoadTestAppointmentInfo();

            _LoadTestTypeInfo();

            if (_TestAppointment.TestID.HasValue)
            {
                _Test = ClsTest.GetInfoByID(_TestAppointment.TestID);
                
                if ( _Test == null )
                {
                    _ResetToDefault();
                    MessageBox.Show($"This Test No.{_TestAppointment.TestID} was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                lblTitle.Text = $"Show Update Take Test No.{_Test.TestID.Value} Info";
                btnSave.Text = "Update";

                if (_Test.TestResult == true)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;

                rbFail.Enabled = false;
                rbPass.Enabled = false;
                txtNotes.Text = _Test.Notes;
            }
            else
            {
                rbFail.Enabled = true;
                rbPass.Enabled = true;
                rbPass.Checked = true;
                rbFail.Checked = false;
                txtNotes.Clear();
                lblTitle.Text = "Show Create New Test Info";
                btnSave.Text = "Create";

                _Test = new ClsTest();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (FindForm().MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.VisionTestAppointments);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace( txtNotes.Text.Trim() ))
            {
                MessageBox.Show($"You have to insert notes at least 1 word.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _Test.TestAppointmentID = _TestAppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = txtNotes.Text.Trim();
            _Test.CreatedByUserID = ClsGlobal.CurrentUser.UserID;

            if ( _Test.Save() )
            {
                int Id = _Test.TestID.Value;
                lblTitle.Text = $"Show Update Take Test No.{Id} Info";
                btnSave.Text = "Update";
                rbFail.Enabled = false;
                rbPass.Enabled = false;

                MessageBox.Show($"This Test No.{Id} Data Saved successfully.", "Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error: Data not saved successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
