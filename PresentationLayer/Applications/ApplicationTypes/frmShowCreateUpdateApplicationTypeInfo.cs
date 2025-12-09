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

namespace PresentationLayer.Applications.ApplicationTypes
{
    public partial class frmShowCreateUpdateApplicationTypeInfo : Form, ILoadableForm
    {
        private enum EnMood { Create , Update }
        private EnMood _Mood;
        private ClsApplicationType _ApplicationType;
        private int? _ApplicationTypeID;

        public frmShowCreateUpdateApplicationTypeInfo()
        {
            InitializeComponent();

            _Mood = EnMood.Create;
        }

        public frmShowCreateUpdateApplicationTypeInfo(int? ApplicationTypeID)
        {
            InitializeComponent();

            _Mood = EnMood.Update;
            _ApplicationTypeID = ApplicationTypeID;
        }


        public void LoadInfo(int? ApplicationTypeID)
        {
            _ApplicationTypeID = ApplicationTypeID;
            frmShowCreateUpdateApplicationTypeInfo_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManageApplicationType);    
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace( textBox.Text ))
            {
                errorProvider1.SetError(tlpTitle, "This feild is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tlpTitle, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace( textBox.Text ) || textBox.Text == "0.000")
            {
                errorProvider1.SetError(tlpFees, "This feild is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tlpFees, null);
            }
        }

        private void _ResetToDefault()
        {
            if (_Mood == EnMood.Create)
            {
                Text = "Show Create New Application Type Info";
                btnSave.Text = "Create";

                lblApplicationTypeID.Text = "ApplicationTypeID";
                txtTitle.Clear();
                txtFees.Clear();

                _ApplicationType = new ClsApplicationType();
            }
            else
            {
                Text = $"Show Update Application Type No.{_ApplicationTypeID} Info";
                btnSave.Text = "Update";
            }
        }

        private void _LoadInfo()
        {
            _ApplicationType = ClsApplicationType.GetInfoByID(_ApplicationTypeID);

            if ( _ApplicationType == null)
            {
                _ResetToDefault();

                MessageBox.Show($"This Application Type No.{_ApplicationTypeID} was not found.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblApplicationTypeID.Text = _ApplicationType.ApplicationTypeID.ToString();
            txtTitle.Text = _ApplicationType.Title;
            txtFees.Text = _ApplicationType.Fees.Value.ToString("N3");
        }

        private void frmShowCreateUpdateApplicationTypeInfo_Load(object sender, EventArgs e)
        {
            _ResetToDefault();

            if (_Mood == EnMood.Update)
            {
                _LoadInfo();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            _ApplicationType.Title = txtTitle.Text.Trim();
            _ApplicationType.Fees = Convert.ToDecimal(txtFees.Text.Trim());

            if (_ApplicationType.Save())
            {
                _Mood = EnMood.Update;
                int Id = _ApplicationType.ApplicationTypeID.Value;
                Text = $"Show Update Application Type No.{Id} Info";
                btnSave.Text = "Update";

                MessageBox.Show($"This Application Type No.{Id} Data Saved Successfully.", "Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error: Data not save successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
