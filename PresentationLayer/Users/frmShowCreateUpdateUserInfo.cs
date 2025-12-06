using BusinessLayer;
using PresentationLayer.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Users
{
    public partial class frmShowCreateUpdateUserInfo : Form
    {
        private enum EnMood { Create , Update }
        private EnMood _Mood;

        private ClsUser _User;
        private int? _UserID;

        public frmShowCreateUpdateUserInfo()
        {
            InitializeComponent();

            _Mood = EnMood.Create;
        }

        public frmShowCreateUpdateUserInfo(int? UserID)
        {
            InitializeComponent();

            _Mood = EnMood.Update;
            _UserID = UserID;
        }

        public void LoadInfo(int? UserID)
        {
            _UserID = UserID;
            frmShowCreateUpdateUserInfo_Load(null, null);
        }

        private void _ResetToDefault()
        {
            if ( _Mood == EnMood.Create )
            {
                Text = "Show Create New User Info";
                button1.Text = "Create";
                tpUserInfo.Enabled = false;
                lblUserID.Text = "UserID";
                txtUsername.Clear();
                txtPassword.Clear();
                txtConfirmPassword.Clear();
                ckbIsActive.Checked = true;

                _User = new ClsUser();
            }
            else
            {
                Text = $"Show Update User No{_UserID} Info";
                button1.Text = "Update";
            }
        }

        private void _LoadUserInfo()
        {
            _User = ClsUser.GetInfoByID(_UserID);

            if ( _User == null )
            {
                _ResetToDefault();

                MessageBox.Show($"This User No.{_UserID} Info was not found.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblUserID.Text = _User.UserID.ToString();
            ctrlShowFindPersonInfo1.LoadPersonInfo(_User.PersonID);
            txtUsername.Text = _User.Username;
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            ckbIsActive.Checked = _User.IsActive;
        }

        private void frmShowCreateUpdateUserInfo_Load(object sender, EventArgs e)
        {
            _ResetToDefault();

            if ( _Mood == EnMood.Update )
            {
                _LoadUserInfo();
            }
        }

        private void lblNext_Click(object sender, EventArgs e)
        {
            if (_Mood == EnMood.Update)
            {
                tabControl1.SelectedTab = tabControl1.TabPages["tpUserInfo"];
                return;
            }

            if (ctrlShowFindPersonInfo1.PersonID.HasValue)
            {
                if (ClsUser.IsExists(ctrlShowFindPersonInfo1.PersonID))
                {
                    MessageBox.Show("This Person already have userd.", "Person Userd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                tabControl1.SelectedTab = tabControl1.TabPages["tpUserInfo"];
                tpUserInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please select a person.", "Select a person", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctrlShowFindPersonInfo1.FilterFocus();
            }
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace( textBox.Text ))
            {
                errorProvider1.SetError(tlpUsername, "This field is required.");
                e.Cancel = true;
            }
            else if (textBox.Text.Length < 3)
            {
                errorProvider1.SetError(tlpUsername, "User Name must be at least 3 character.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(textBox, null);
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if ( string.IsNullOrWhiteSpace( textBox.Text ))
            {
                errorProvider1.SetError(tlpPassword, "This field is required.");
                e.Cancel = true;
            }
            else if ( !char.IsLetter(textBox.Text[0] ))
            {
                errorProvider1.SetError(tlpPassword, "First character must be big letter.");
                e.Cancel = true;
            }
            else if ( textBox.Text.Length < 4)
            {
                errorProvider1.SetError(tlpPassword, "Password must be at least 4 digit.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tlpPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace( textBox.Text ))
            {
                errorProvider1.SetError(tlpConfirmPassword, "This field is required.");
                e.Cancel = true;
            }
            else if (!textBox.Text.Equals(txtPassword.Text.Trim()))
            {
                errorProvider1.SetError(tlpConfirmPassword, "Passwords must be matched.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tlpConfirmPassword, null);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            _User.PersonID = ctrlShowFindPersonInfo1.PersonID;
            _User.Username = txtUsername.Text.Trim();
            _User.Password = ClsValidations.HashPasswords(txtConfirmPassword.Text.Trim());
            _User.IsActive = ckbIsActive.Checked;

            if ( _User.Save() )
            {
                _Mood = EnMood.Update;
                int Id = _User.UserID.Value;
                Text = $"Show Update User No.{Id} Info";
                lblUserID.Text = Id.ToString();
                button1.Text = "Update";

                MessageBox.Show($"This User No.{Id} Data Saved Successfully.", "Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManageUsers);
            }
        }
    }
}
