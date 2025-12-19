using BusinessLayer;
using PresentationLayer.InterFaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Tests
{
    public partial class frmShowTakeTestInfo : Form
    {
        private int? _LocalDrivingLicenseApplicationID;
        private int? _TestAppointmentID;
        private ClsTestType.EnTestType _TestTypeID;
        public frmShowTakeTestInfo(int? LocalDrivingLicenseApplicationID, int? TestAppointmetID, ClsTestType.EnTestType TestTypeID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = TestAppointmetID;
            _TestTypeID = TestTypeID;

        }

        public void LoadInfo(int? LocalDrivingLicenseApplicationID, int? TestAppointmetID, ClsTestType.EnTestType TestTypeID)
        {
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = TestAppointmetID;
            _TestTypeID = TestTypeID;

            frmShowTakeTestInfo_Load(null, null);
        }

        private void frmShowTakeTestInfo_Load(object sender, EventArgs e)
        {
            Text = $"Show Test Appointment No.{_TestAppointmentID} Take Test Info";
            ctrlShowTakeTests1.LoadTestInfo(_LocalDrivingLicenseApplicationID, _TestAppointmentID, _TestTypeID);
        }
    }
}
