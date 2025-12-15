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

        private int? _LocalDrivingLicenseApplicationID;
        private enum EnMood { Create , Update }
        private EnMood _Mood;

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
            lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.Value.ToString();
        }

        public void LoadInfo(int? LocalDrivingLicenseApplicationID, int? TestAppointmentID)
        {
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _LoadLocalDrivingLicenseApplicationInfo();

            if (TestAppointmentID.HasValue)
                _Mood = EnMood.Update;
            else
                _Mood = EnMood.Create;

            

        }
    }
}
