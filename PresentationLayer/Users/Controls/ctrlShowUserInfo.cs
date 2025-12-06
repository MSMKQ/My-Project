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

namespace PresentationLayer.Users.Controls
{
    public partial class ctrlShowUserInfo : UserControl
    {
        private ClsUser _User;

        public ctrlShowUserInfo()
        {
            InitializeComponent();
        }

        private void _ResetToDefault()
        {
            lblUserID.Text = "UserID";
            lblUserName.Text = "Username";
            lblIsActive.Text = "IsActive";
        }

        private void _LoadUser()
        {
            lblUserID.Text = _User.UserID.ToString();
            ctrlShowPersonInfo1.LoadInfo(_User.PersonID);
            lblUserName.Text = _User.Username;
            lblIsActive.Text = (_User.IsActive == true) ? "True" : "False";
        }

        public void LoadUser (int? UserID)
        {
            _User = ClsUser.GetInfoByID(UserID);

            if ( _User == null )
            {
                _ResetToDefault();

                MessageBox.Show($"This Person No.{UserID} was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LoadUser();
        }
    }
}
