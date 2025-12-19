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

namespace PresentationLayer.TestAppointments.Controls
{
    public partial class ctrlShowCreateUpdateTestAppointmentInfo : UserControl
    {
        private ClsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private ClsTestAppointment _TestAppointment;
        private int? _LocalDrivingLicenseApplicationID;
        private int? _TestAppointmentID;
        private enum EnMood { Create , Update }
        private EnMood _Mood;
        private enum EnCreation { FirstTime , RetakeTest }
        private EnCreation _Creation;

        public string Title { get { return _Title; } }
        private string _Title;

        private ClsTestType.EnTestType _TestTypeID = ClsTestType.EnTestType.VisionTest;

        public ClsTestType.EnTestType TestTypeID
        {
            get { return _TestTypeID;  }

            set
            {
                _TestTypeID = value;
                
                switch (_TestTypeID)
                {
                    case ClsTestType.EnTestType.VisionTest:
                        lblTestType.Text = "Vision Test";
                        pbTestType.Image = Resources.Vision_512;
                        break;

                    case ClsTestType.EnTestType.WrittenTest:
                        lblTestType.Text = "Written Test";
                        pbTestType.Image = Resources.Written_Test_512;
                        break;

                    case ClsTestType.EnTestType.StreetTest:
                        lblTestType.Text = "Street Test";
                        pbTestType.Image = Resources.driving_test_512;
                        break;
                }
            }
        }

        public ctrlShowCreateUpdateTestAppointmentInfo()
        {
            InitializeComponent();
        }

        private void _ResetToDefault()
        {
            lblLocalDrivingLicenseApplicationID.Text = "[ ?? ]";
            lblDrivingLicense.Text = "[ ?????? ]";
            lblName.Text = "[ ?????? ]";
            lblTrails.Text = "[ ?/? ]";
            dtpAppointmentDate.Value = DateTime.Now;
            lblFees.Text = "[ $$$ ]";
        }


        private void _LoadLocalDrivingLicenseApplicationInfo()
        {
            _LocalDrivingLicenseApplication = ClsLocalDrivingLicenseApplication.GetInfoByID(_LocalDrivingLicenseApplicationID);

            if ( _LocalDrivingLicenseApplication == null )
            {
                _ResetToDefault();
                MessageBox.Show($"This Local Driving License Application No.{_LocalDrivingLicenseApplicationID} was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingLicense.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.Title;
            lblName.Text = _LocalDrivingLicenseApplication.FullName;
            lblTrails.Text = "[ ?/? ]";
            lblFees.Text = ClsTestType.GetInfoByID(_TestTypeID).Fees.Value.ToString();
            lblTrails.Text = ClsTestAppointment.TotalTrails(_LocalDrivingLicenseApplicationID, _TestTypeID).ToString();
        }

        public void LoadInfo(int? LocalDrivingLicenseApplicationID, int? TestAppointmentID)
        {
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = TestAppointmentID;
            _LoadLocalDrivingLicenseApplicationInfo();

            if ( TestAppointmentID.HasValue )
                _Mood = EnMood.Update;
            else
                _Mood = EnMood.Create;

            if ( _LocalDrivingLicenseApplication.DoesAttendedTest(_TestTypeID) )
                _Creation = EnCreation.RetakeTest;
            else
                _Creation = EnCreation.FirstTime;

            if ( _Creation == EnCreation.FirstTime )
            {
                groupBox1.Enabled = false;
                lblReatkeTestFees.Text = "0.000";
                lblRetakeTestApplicationNo.Text = "N / A";
            }
            else
            {
                groupBox1.Enabled = true;
                lblRetakeTestApplicationNo.Text = _TestAppointmentID.ToString();
                lblReatkeTestFees.Text = ClsApplicationType.GetInfoByID(ClsApplicationType.EnServices.RetakeTest).Fees.Value.ToString("N3");
            }

            if ( _Mood == EnMood.Create )
            {
                _Title = "Show Create New Test Appointment Info";
                dtpAppointmentDate.MinDate = DateTime.Now;
                btnSave.Text = "Create";
                _TestAppointment = new ClsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentInfo())
                    return;
            }

            lblTotal.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblReatkeTestFees.Text)).ToString("N3"); lblTotal.Enabled = false;

            if (!_HandlerAppointmentLocked())
                return;
        }

        private bool _HandlerAppointmentLocked()
        {
            if (_TestAppointment.IsLocked)
            {
                dtpAppointmentDate.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
            {
                dtpAppointmentDate.Enabled = true;
                btnSave.Enabled = true;
                return true;
            }
        }

        private bool _LoadTestAppointmentInfo()
        {
            _TestAppointment = ClsTestAppointment.GetInfoByID(_TestAppointmentID);

            if  ( _TestAppointment == null )
            {
                MessageBox.Show($"This Test Appointment No.{_TestAppointmentID} was not found", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate.Value) > 0)
                dtpAppointmentDate.MinDate = _TestAppointment.AppointmentDate.Value;
            else
                dtpAppointmentDate.MinDate = DateTime.Now;

            dtpAppointmentDate.Value = _TestAppointment.AppointmentDate.Value;
            lblFees.Text = _TestAppointment.PaidFees.Value.ToString("N3");
            lblReatkeTestFees.Text = (_TestAppointment.RetakeTestApplicationInfo?.PaidFees ?? 0).ToString("N3");
            lblRetakeTestApplicationNo.Text = (_TestAppointment.RetakeTestApplicationID.HasValue) ? _TestAppointment.RetakeTestApplicationID.ToString() : "N / A";
            _Title = $"Show Update Test Appointment No.{_TestAppointmentID.Value} Info";
            btnSave.Text = "Update";

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpAppointmentDate.Value;
            _TestAppointment.PaidFees = Convert.ToDecimal(lblTotal.Text);
            _TestAppointment.CreatedByUserID = ClsGlobal.CurrentUser.UserID;
            _TestAppointment.IsLocked = false;
            _TestAppointment.RetakeTestApplicationID = _TestAppointmentID;

            if (_TestAppointment.Save())
            {
                int Id = _TestAppointment.TestAppointmentID.Value;
                _Title = $"Show Update Test Appointment No.{Id} Info";
                btnSave.Text = "Update";

                MessageBox.Show($"This Test Appointment No.{Id} Data Saved Successfully.", "Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: failed to save data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
