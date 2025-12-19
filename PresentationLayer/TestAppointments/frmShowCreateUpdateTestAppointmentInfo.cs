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

namespace PresentationLayer.TestAppointments
{
    public partial class frmShowCreateUpdateTestAppointmentInfo : Form
    {
        private ClsTestType.EnTestType _TestTypeID;
        private int? _LocalDrivingLicenseApplicationID;
        private int? _TestAppointmentID;

        public frmShowCreateUpdateTestAppointmentInfo(int? LocalDrivingLicenseApplicationID,  ClsTestType.EnTestType TestTypeID, int? TestAppointmentID = null)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeID = TestTypeID;
            _TestAppointmentID = TestAppointmentID;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.VisionTestAppointments);
            }
        }

        public void LoadInfo(int? LocalDrivingLicenseApplicationID, ClsTestType.EnTestType TestTypeID, int? TestAppointmentID = null)
        {
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeID = TestTypeID;
            _TestAppointmentID = TestAppointmentID;

            frmShowCreateUpdateTestAppointmentInfo_Load(null, null);
        }

        private void frmShowCreateUpdateTestAppointmentInfo_Load(object sender, EventArgs e)
        {
            ctrlShowCreateUpdateTestAppointmentInfo1.TestTypeID = _TestTypeID;
            ctrlShowCreateUpdateTestAppointmentInfo1.LoadInfo(_LocalDrivingLicenseApplicationID, _TestAppointmentID);
            Text = ctrlShowCreateUpdateTestAppointmentInfo1.Title;
        }
    }
}
