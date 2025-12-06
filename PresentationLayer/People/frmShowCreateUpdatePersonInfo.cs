using BusinessLayer;
using PresentationLayer.Classes;
using PresentationLayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.People
{
    public partial class frmShowCreateUpdatePersonInfo : Form
    {
        
        private enum EnMood { Create , Update }
        private EnMood _Mood;
        private ClsPerson _Person;
        private static int? _PersonID;

        public frmShowCreateUpdatePersonInfo()
        {
            InitializeComponent();

            _Mood = EnMood.Create;
        }

        public frmShowCreateUpdatePersonInfo(int? PersonID)
        {
            InitializeComponent();

            _Mood = EnMood.Update;
            _PersonID = PersonID;
        }

        public void LoadInfo(int? PersonID)
        {
            _PersonID = PersonID;

            frmShowCreateUpdatePersonInfo_Load(null, null);
        }

        private bool _HandleImage()
        {
            if (_Person.ImagePath != pbImagePath.ImageLocation)
            {
                if (!string.IsNullOrWhiteSpace( _Person.ImagePath ))
                {
                    try
                    {
                        if (File.Exists( _Person.ImagePath ))
                        {
                            File.Delete(_Person.ImagePath);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Error Handle exists file.\n {e.Message}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!string.IsNullOrWhiteSpace( pbImagePath.ImageLocation ))
                {
                    string ImagePath = pbImagePath.ImageLocation;

                    if (ClsUtils.MoveImageToProjectFolder(ref ImagePath))
                    {
                        pbImagePath.ImageLocation = ImagePath;
                    }
                    else
                    {
                        MessageBox.Show($"Error Move Image To Project Folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            if (!_HandleImage())
                return;

            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationID = txtNationID.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Gender = (rbFemal.Checked) ? ClsPerson.EnGender.Female : ClsPerson.EnGender.Male;
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.CountryID = ClsCountry.GetInfoByCountry(cbCountry.Text).CountryID;
            _Person.Address = txtAddress.Text.Trim();
            
            if ( !string.IsNullOrWhiteSpace( pbImagePath.ImageLocation ) )
            {
                _Person.ImagePath = pbImagePath.ImageLocation;
            }

            if ( _Person.Save() )
            {
                _Mood = EnMood.Update;
                int Id = _Person.PersonID.Value;
                string Title = $"Show Update Person No.{Id} Info";
                Text = Title; lblPersonID.Text = Id.ToString();
                btnSave.Text = "Update";

                MessageBox.Show($"This Person No.{Id} Data Saved Successfully.", "Data Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Error: Data not saved successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManagePeople);
            }

            frmShowCreateUpdatePersonInfo_Load(null, null);
        }

        private void _LoadInfo()
        {
            _Person = ClsPerson.GetInfoByID(_PersonID);

            if (_Person == null)
            {
                _ResetToDefault();

                MessageBox.Show($"Error: This person no.{_PersonID} was not found.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblPersonID.Text = _Person.PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationID.Text = _Person.NationID;
            dtpDateOfBirth.Value = _Person.DateOfBirth.Value;
            rbMale.Checked = (_Person.Gender == ClsPerson.EnGender.Male);
            rbFemal.Checked = (_Person.Gender == ClsPerson.EnGender.Female);
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            cbCountry.Text = _Person.CountryInfo.CountryName;
            txtAddress.Text = _Person.Address;

            if (string.IsNullOrWhiteSpace(_Person.ImagePath))
            {
                pbImagePath.Image = (rbFemal.Checked) ? Resources.Female_512 : Resources.Male_512;
            }
            else
            {
                pbImagePath.ImageLocation = _Person.ImagePath;
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;


            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManagePeople);
            }

            frmShowCreateUpdatePersonInfo_Load(null, null);
        }

        private void _FillCountriesInComboBox()
        {
            DataTable dataTable = ClsCountry.GetCountries();

            foreach (DataRow row in dataTable.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _ResetToDefault()
        {
            _FillCountriesInComboBox();

            if (_Mood == EnMood.Create)
            {
                Text = "Show Create New Person Info";
                btnSave.Text = "Create";

                txtFirstName.Text = "First";
                txtSecondName.Text = "Second";
                txtThirdName.Text = "Third";
                txtLastName.Text = "Last";
                txtNationID.Text = ClsPerson.GetNationID();
                dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
                dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
                rbMale.Checked = true;
                txtPhone.Text = "Phone Number";
                txtEmail.Text = "Email Address";
                cbCountry.SelectedIndex = cbCountry.FindString("Bahrain");
                txtAddress.Text = "Address";
                pbImagePath.Image = Resources.Male_512;
                lblRemoveImage.Visible = false;

                _Person = new ClsPerson();
            }
            else
            {
                Text = $"Show Update Person No.{_PersonID} Info";
                btnSave.Text = "Update";
            }
        }

        private void frmShowCreateUpdatePersonInfo_Load(object sender, EventArgs e)
        {
            _ResetToDefault();

            if (_Mood == EnMood.Update)
            {
                _LoadInfo();
            }
        }

        private void _EnterTextBox(object sender, string Placedholder)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text) || string.Equals(textBox.Text, Placedholder, StringComparison.OrdinalIgnoreCase))
            {
                textBox.Clear();
                textBox.ForeColor = Color.Black;
                textBox.Focus();
            }
        }

        private void _LeaveTextBox(object sender, string Placedholder)
        {
            TextBox textBox = sender as TextBox;
            string LowerPlacedholder = Placedholder.ToLower();

            if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text.ToLower() == LowerPlacedholder)
            {
                textBox.ForeColor = Color.Gray;
                textBox.Text = char.ToUpper(LowerPlacedholder[0]) + LowerPlacedholder.Substring(1);
            }
        }

        private void txtFirstName_Enter(object sender, EventArgs e)
        {
            _EnterTextBox(sender, "First");
        }

        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            _LeaveTextBox(sender, "First");
        }

        private void txtSecondName_Enter(object sender, EventArgs e)
        {
            _EnterTextBox(sender, "Second");
        }

        private void txtSecondName_Leave(object sender, EventArgs e)
        {
            _LeaveTextBox(sender, "second");
        }

        private void txtThirdName_Enter(object sender, EventArgs e)
        {
            _EnterTextBox(sender, "Third");
        }

        private void txtThirdName_Leave(object sender, EventArgs e)
        {
            _LeaveTextBox(sender, "Third");
        }

        private void txtLastName_Enter(object sender, EventArgs e)
        {
            _EnterTextBox(sender, "Last");
        }

        private void txtLastName_Leave(object sender, EventArgs e)
        {
            _LeaveTextBox(sender, "Last");
        }

        private void txtNationID_Enter(object sender, EventArgs e)
        {
            _EnterTextBox(sender, "NationID");
        }

        private void txtNationID_Leave(object sender, EventArgs e)
        {
            _LeaveTextBox(sender, "NationID");
        }

        private void txtPhone_Enter(object sender, EventArgs e)
        {
            _EnterTextBox(sender, "Phone Number");
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            _LeaveTextBox(sender, "Phone Number");
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            _EnterTextBox(sender, "Email Address");
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            _LeaveTextBox(sender, "Email Address");
        }

        private void txtAddress_Enter(object sender, EventArgs e)
        {
            _EnterTextBox(sender, "Address");
        }

        private void txtAddress_Leave(object sender, EventArgs e)
        {
            _LeaveTextBox(sender, "Address");
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pbImagePath.ImageLocation))
            {
                pbImagePath.Image = Resources.Male_512;
            }
        }

        private void rbFemal_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pbImagePath.ImageLocation))
            {
                pbImagePath.Image = Resources.Female_512;
            }
        }

        private void lblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "PNG Images |*.png|JPG Images|*.jpg|JEPG Images |*.jepg|All Files|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ImagePath = openFileDialog1.FileName;

                if (!string.IsNullOrWhiteSpace( pbImagePath.ImageLocation ))
                {
                    try
                    {
                        if (File.Exists( pbImagePath.ImageLocation ))
                        {
                            File.Delete(pbImagePath.ImageLocation);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error Handle exists file.\n {ex.Message}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                pbImagePath.ImageLocation = ImagePath;
                lblRemoveImage.Visible = true;
            }
        }

        private void lblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (! string.IsNullOrWhiteSpace(pbImagePath.ImageLocation))
            {
                pbImagePath.ImageLocation = null;
                pbImagePath.Image = (rbFemal.Checked) ? Resources.Female_512 : Resources.Male_512;
            }
        }

        private void IsNullOrEmpty_Validating(object sender, CancelEventArgs e, string Placedholder)
        {
            TextBox textBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text) || string.Equals(textBox.Text, Placedholder, StringComparison.OrdinalIgnoreCase))
            {
                errorProvider1.SetError(textBox, "This Field is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(textBox, null);
            }
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            IsNullOrEmpty_Validating(sender, e, "First");
        }


    }
}
