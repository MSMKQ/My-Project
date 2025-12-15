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
        public frmShowCreateUpdateTestAppointmentInfo(int? LocalDrivingLicenseApplicationID,  ClsTestType.EnTestType TestTypeID, int? TestAppointmentID = null)
        {
            InitializeComponent();

            ctrlShowCreateUpdateTestAppointmentInfo1.LoadInfo(LocalDrivingLicenseApplicationID, TestAppointmentID);
            ctrlShowCreateUpdateTestAppointmentInfo1.TestTypeID = TestTypeID;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.VisionTestAppointments);
            }
        }
    }
}
