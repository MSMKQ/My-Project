using BusinessLayer;
using PresentationLayer.Classes;
using PresentationLayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.People.Controls
{
    public partial class ctrlShowPersonInfo : UserControl
    {
        public int? PersonID { get { return _Person?.PersonID; } }

        private ClsPerson _Person = new ClsPerson();
        public ctrlShowPersonInfo()
        {
            InitializeComponent();
        }

        public void ResetToDefault()
        {
            lblPersonID.Text = "PersonID";
            lblFullName.Text = "FullName";
            lblNationID.Text = "NationID";
            lblDateOfBirth.Text = "dateOfBirth";
            lblGender.Text = "Gender";
            lblEmail.Text = "Email";
            lblPhone.Text = "Phone";
            lblCountry.Text = "Country";
            lblAddress.Text = "Address";
            pbImagePath.Image = ClsUtils.ResizeAndCorpToCircle(Resources.Male_512, 289, 186, Color.Black);
        }

        public void LoadInfo(int? PersonID)
        {
            _Person = ClsPerson.GetInfoByID(PersonID);

            if (_Person == null)
            {
                ResetToDefault();
                MessageBox.Show($"This Person No.{PersonID} was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LoadInfo();
        }

        public void LoadInfo(string NationID)
        {
            _Person = ClsPerson.GetInfoByID(NationID);

            if (_Person == null)
            {
                ResetToDefault();
                MessageBox.Show($"This Person Nation No.{NationID} was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LoadInfo();
        }

        private void _LoadInfo()
        {
            lblPersonID.Text = _Person.PersonID.ToString();
            lblFullName.Text = _Person.FullName;
            lblNationID.Text = _Person.NationID;
            lblDateOfBirth.Text = _Person.DateOfBirth.Value.ToShortDateString();
            lblGender.Text = (_Person.Gender == ClsPerson.EnGender.Female) ? ClsPerson.EnGender.Female.ToString() : (_Person.Gender == ClsPerson.EnGender.Male) ? ClsPerson.EnGender.Male.ToString() : ClsPerson.EnGender.Unknown.ToString();
            lblPhone.Text = _Person.Phone;
            lblEmail.Text = _Person.Email;
            lblCountry.Text = _Person.CountryInfo.CountryName;
            lblAddress.Text = _Person.Address;

            if (!string.IsNullOrWhiteSpace( _Person.ImagePath ))
            {
                pbImagePath.Image = ClsUtils.ResizeAndCorpToCircle(Image.FromFile(_Person.ImagePath), 289, 186, Color.Black);
            }
        }
    }
}
