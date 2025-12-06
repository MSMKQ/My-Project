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
    public partial class frmShowUserChangePassword : Form
    {
        private int? _UserID;

        private void _ResetToDefault()
        {
            txtCurrentPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
        }

        public frmShowUserChangePassword(int? UserID)
        {
            InitializeComponent();

            Text = $"Show User No.{UserID} Change Password";
            _UserID = UserID;
            ctrlShowUserInfo1.LoadUser(UserID);
            _ResetToDefault();
        }

        public void LoadInfo(int? UserID)
        {
            Text = $"Show User No.{UserID} Change Password";
            _UserID = UserID;
            ctrlShowUserInfo1.LoadUser(UserID);
            _ResetToDefault();
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManageUsers);
            }
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace( textBox.Text ))
            {
                errorProvider1.SetError(tlpCurrentPassword, "This field is required.");
                e.Cancel = true;
            }
            else if ( !ClsUser.IsExists(_UserID, ClsValidations.HashPasswords(textBox.Text) ))
            {
                errorProvider1.SetError(tlpCurrentPassword, "Wroung Password.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tlpCurrentPassword, null);
            }
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace( textBox.Text ))
            {
                errorProvider1.SetError(tlpNewPassword, "This field is required.");
                e.Cancel = true;
            }
            else if (!char.IsLetter(textBox.Text[0]))
            {
                errorProvider1.SetError(tlpNewPassword, "First character must be Letter.");
                e.Cancel = true;
            }
            else if (textBox.Text.Length < 4)
            {
                errorProvider1.SetError(tlpNewPassword, "Password must be at least 4 character.");
                e.Cancel = true;
            }
            else if (ClsUser.IsExists(_UserID, ClsValidations.HashPasswords( textBox.Text.Trim() )))
            {
                errorProvider1.SetError(tlpNewPassword, "Choose anther Password.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tlpNewPassword, null);
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
            else if (textBox.Text.Trim() != txtNewPassword.Text.Trim())
            {
                errorProvider1.SetError(tlpConfirmPassword, "Passwords are not matched.");
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

            if (ClsUser.Update(_UserID, ClsValidations.HashPasswords( txtConfirmPassword.Text )))
            {
                MessageBox.Show($"This User No.{_UserID} Data Saved Successfully.", $"Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data was not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
