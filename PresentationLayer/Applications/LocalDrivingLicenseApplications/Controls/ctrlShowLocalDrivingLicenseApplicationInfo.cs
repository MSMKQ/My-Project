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

namespace PresentationLayer.Applications.LocalDrivingLicenseApplications.Controls
{
    public partial class ctrlShowLocalDrivingLicenseApplicationInfo : UserControl
    {
        private ClsLocalDrivingLicenseApplication _Local;

        public ctrlShowLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private void _ResetToDefault()
        {
            lblLocalDrivingLicenseApplicationID.Text = "LocalAppID";
            lblLicenseClass.Text = "LicenseClass";
            lblTestsPassed.Text = "TestsPassed";
        }

        private void _LoadInfo()
        {
            lblLocalDrivingLicenseApplicationID.Text = _Local.LocalDrivingLicenseApplicationID.ToString();
            ctrlShowApplicationInfo2.LoadInfo(_Local.ApplicationID);
            lblLicenseClass.Text = _Local.LicenseClassInfo.Title;
            lblTestsPassed.Text = $" [ 3/{ClsTest.TotalTests(_Local.LocalDrivingLicenseApplicationID).ToString()} ]";
        }

        public void LoadInfo(int? LocalDrivingLicenseApplicationID)
        {
            _Local = ClsLocalDrivingLicenseApplication.GetInfoByID(LocalDrivingLicenseApplicationID);

            if ( _Local == null )
            {
                _ResetToDefault();

                MessageBox.Show($"This Local Driving License Application No.{LocalDrivingLicenseApplicationID} was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LoadInfo();
        }
    }
}
