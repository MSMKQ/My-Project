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

namespace PresentationLayer.Applications.LocalDrivingLicenseApplications
{
    public partial class frmShowLocalDrivingLicenseApplicationInfo : Form, ILoadableForm
    {
        public frmShowLocalDrivingLicenseApplicationInfo(int? LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            Text = $"Show Local Driving License Application No.{LocalDrivingLicenseApplicationID} Info";
            ctrlShowLocalDrivingLicenseApplicationInfo1.LoadInfo(LocalDrivingLicenseApplicationID);
        }

        public void LoadInfo(int? LocalDrivingLicenseApplicationID)
        {
            Text = $"Show Local Driving License Application No.{LocalDrivingLicenseApplicationID} Info";
            ctrlShowLocalDrivingLicenseApplicationInfo1.LoadInfo(LocalDrivingLicenseApplicationID);
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManageLocalDrivingLicenseApplications);
            }
        }
    }
}
