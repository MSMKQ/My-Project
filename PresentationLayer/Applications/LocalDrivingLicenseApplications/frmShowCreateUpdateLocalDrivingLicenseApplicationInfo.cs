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

namespace PresentationLayer.Applications.LocalDrivingLicenseApplications
{
    public partial class frmShowCreateUpdateLocalDrivingLicenseApplicationInfo : Form, ILoadableForm
    {
        private enum EnMood { Create , Update }
        private EnMood _Mood;

        private ClsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int? _LocalDrivingLicenseApplicationID;

        public frmShowCreateUpdateLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();

            _Mood = EnMood.Create;
        }

        public frmShowCreateUpdateLocalDrivingLicenseApplicationInfo(int? LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();

            _Mood = EnMood.Update;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }

        public void LoadInfo(int? LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            frmShowCreateUpdateLocalDrivingLicenseApplicationInfo_Load(null, null);
        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable _LicenseClasses = ClsLicenseClass.GetLicenseClasses();

            foreach (DataRow row in _LicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["Title"]);
            }
        }

        private void _ResetToDefault()
        {
            _FillLicenseClassesInComboBox();

            if ( _Mood == EnMood.Create )
            {
                Text = "Show Create New Local Driving License Application Info";
                btnSave.Text = "Create";

                lblLocalDrivingLicenseApplicationID.Text = "N / A";
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                cbLicenseClass.SelectedIndex = 2;
                lblPaidFees.Text = ClsApplicationType.GetInfoByID(ClsApplicationType.EnServices.NewLocalDriving).Fees.Value.ToString("N3");
                lblCreatedByUserID.Text = ClsGlobal.CurrentUser.Username;

                tpApplicationInfo.Enabled = false;
                _LocalDrivingLicenseApplication = new ClsLocalDrivingLicenseApplication();
            }
            else
            {
                Text = $"Show Update Local Driving License Application No.{_LocalDrivingLicenseApplicationID} Info";
                btnSave.Text = "Update";
            }
        }

        private void _LoadInfo()
        {
            ctrlShowFindPersonInfo1.FilterEnabled = false;

            _LocalDrivingLicenseApplication = ClsLocalDrivingLicenseApplication.GetInfoByID(_LocalDrivingLicenseApplicationID);

            if ( _LocalDrivingLicenseApplication == null )
            {
                MessageBox.Show($"This Local Driving License Application No.{_LocalDrivingLicenseApplicationID} was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            ctrlShowFindPersonInfo1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicationPersonID);
            lblApplicationDate.Text = _LocalDrivingLicenseApplication.ApplicationDate.Value.ToShortDateString();
            cbLicenseClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.Title;
            lblPaidFees.Text = _LocalDrivingLicenseApplication.PaidFees.Value.ToString("N3");
            lblCreatedByUserID.Text = ClsUser.GetInfoByID(_LocalDrivingLicenseApplication.CreatedByUserID).Username;
        }

        private void frmShowCreateUpdateLocalDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            _ResetToDefault();

            if ( _Mood == EnMood.Update )
            {
                _LoadInfo();
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            if ( MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManageLocalDrivingLicenseApplications);
            }
        }

        private void lblNext_Click(object sender, EventArgs e)
        {
            if ( _Mood == EnMood.Update )
            {
                tabControl1.SelectedTab = tabControl1.TabPages["tpApplicationInfo"];
                return;
            }

            if ( ctrlShowFindPersonInfo1.PersonID.HasValue )
            {
                tabControl1.SelectedTab = tabControl1.TabPages["tpApplicationInfo"];
                tpApplicationInfo.Enabled = true;

            }
            else
            {
                MessageBox.Show("Please selected a person.", "Select Person", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctrlShowFindPersonInfo1.FilterFocus();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int _SelectedPersonID = ctrlShowFindPersonInfo1.PersonID.Value;
            int _LicenseClassID = ClsLicenseClass.GetInfoByTitle(cbLicenseClass.Text).LicenseClassID.Value;

            bool IsThereAnActiveLicense = ClsLicense.IsThereLicense(_SelectedPersonID, _LicenseClassID);

            if (IsThereAnActiveLicense)
            {
                MessageBox.Show($"This Person already have Licensed for this class.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? ActiveApplicationID = ClsLocalDrivingLicenseApplication.IsThereAnActiveApplication(_SelectedPersonID, _LicenseClassID);

            if ( ActiveApplicationID != null )
            {
                MessageBox.Show($"There is Already active Application.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _LocalDrivingLicenseApplication.ApplicationPersonID = _SelectedPersonID;
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = (int)ClsApplicationType.EnServices.NewLocalDriving;
            _LocalDrivingLicenseApplication.ApplicationStatus = (int)ClsApplication.EnStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = ClsApplicationType.GetInfoByID(ClsApplicationType.EnServices.NewLocalDriving).Fees.Value;
            _LocalDrivingLicenseApplication.CreatedByUserID = ClsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID = _LicenseClassID;

            if ( _LocalDrivingLicenseApplication.Save() )
            {
                _Mood = EnMood.Update;
                int Id = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.Value;
                lblLocalDrivingLicenseApplicationID.Text = Id.ToString();
                Text = $"Show Update Local Driving License Application No.{Id} Info";
                btnSave.Text = "Update";
                ctrlShowFindPersonInfo1.FilterEnabled = false;

                MessageBox.Show($"This Local Driving License Application No.{Id} Data Saved Successfully.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data Not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
