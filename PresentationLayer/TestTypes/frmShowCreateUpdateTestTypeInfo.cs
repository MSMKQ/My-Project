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

namespace PresentationLayer.TestTypes
{
    public partial class frmShowCreateUpdateTestTypeInfo : Form, ILoadableForm
    {
        private enum EnMood { Create , Update }
        private EnMood _Mood;
        private ClsTestType _TestType;
        private int? _TestTypeID;

        public frmShowCreateUpdateTestTypeInfo()
        {
            InitializeComponent();

            _Mood = EnMood.Create;
        }

        public frmShowCreateUpdateTestTypeInfo(int? TestTypeID)
        {
            InitializeComponent();

            _Mood = EnMood.Update;
            _TestTypeID = TestTypeID;
        }

        public void LoadInfo(int? TestTypeID)
        {
            _TestTypeID = TestTypeID;

            frmShowCreateUpdateTestTypeInfo_Load(null, null);
        }

        private void _ResetToDefault()
        {
            if ( _Mood == EnMood.Create )
            {
                Text = "Show Create New Test Type Info";
                btnSave.Text = "Create";

                lblTestTypeID.Text = "TestTypeID";
                txtTitle.Clear();
                txtDescription.Clear();
                txtFees.Text = "0.000";

                _TestType = new ClsTestType();
            }
            else
            {
                Text = $"Show Update Test Type No.{_TestTypeID} Info";
                btnSave.Text = "Update";
            }
        }

        private void _LoadInfo()
        {
            _TestType = ClsTestType.GetInfoByID((ClsTestType.EnTestType)_TestTypeID);

            if ( _TestType == null )
            {
                _ResetToDefault();
                MessageBox.Show($"This Test Type No.{_TestTypeID} was not found.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblTestTypeID.Text = _TestType.TestTypeID.ToString();
            txtTitle.Text = _TestType.Title;
            txtDescription.Text = _TestType.Description;
            txtFees.Text = _TestType.Fees.Value.ToString("N3");
        }

        private void frmShowCreateUpdateTestTypeInfo_Load(object sender, EventArgs e)
        {
            _ResetToDefault();

            if ( _Mood == EnMood.Update )
            {
                _LoadInfo();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManageTestTypes);
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace( textBox.Text ))
            {
                errorProvider1.SetError(tlpTitle, "This field is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tlpTitle, null);
            }
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace( textBox.Text ))
            {
                errorProvider1.SetError(tlpDescription, "This field is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tlpDescription, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace( textBox.Text ) || textBox.Text.Equals("0.000"))
            {
                errorProvider1.SetError(tlpFees, "This field is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tlpFees, null);
            }
        }

        private void txtFees_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (int.TryParse(textBox.Text.ToString(), out int value))
            {

                txtFees.Text = value.ToString("N3");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            _TestType.Title = txtTitle.Text.Trim();
            _TestType.Description = txtDescription.Text.Trim();
            _TestType.Fees = Convert.ToDecimal(txtFees.Text.Trim());

            if (_TestType.Save())
            {
                _Mood = EnMood.Update;
                int Id = _TestType.TestTypeID.Value;
                Text = $"Show Update Test Type No.{_TestType.TestTypeID} Info";
                btnSave.Text = "Update";

                MessageBox.Show($"This Test Type No.{Id} Data Saved Successfully.", "Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error: Data not saved successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
