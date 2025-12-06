using BusinessLayer;
using PresentationLayer.Applications.ApplicationTypes;
using PresentationLayer.Classes;
using PresentationLayer.People;
using PresentationLayer.Properties;
using PresentationLayer.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class frmShowNewMainMenue : Form
    {
        private bool _sidebarExpanded = false;
        private bool _homeCollapsible = true;
        private Dictionary<string, Form> _ChildForm = new Dictionary<string, Form>();
        public enum  EnForm { ManagePeople , Dashboard , PersonInfo , CreatePerson , UpdatePerson , ManageUsers , Login , UserInfo , CreateUser , UpdateUser , UserChangePassword, FindPerson , ManageApplicationType , CreateApplicationTypeInfo }

        private Form _PushForm(EnForm enform)
        {
            switch (enform)
            {
                case EnForm.ManagePeople:
                    return new People.frmShowManagePeopleList();

                case EnForm.PersonInfo:
                    return new People.frmShowPersonInfo(frmShowManagePeopleList.SelectedRow);

                case EnForm.CreatePerson:
                    return new People.frmShowCreateUpdatePersonInfo();

                case EnForm.UpdatePerson:
                    return new People.frmShowCreateUpdatePersonInfo(frmShowManagePeopleList.SelectedRow);

                case EnForm.ManageUsers:
                    return new Users.frmShowManageUsersList();

                case EnForm.UserInfo:
                    return new Users.frmShowUserInfo(frmShowManageUsersList.SelectedRow);

                case EnForm.CreateUser:
                    return new Users.frmShowCreateUpdateUserInfo();

                case EnForm.UpdateUser:
                    return new Users.frmShowCreateUpdateUserInfo(frmShowManageUsersList.SelectedRow);

                case EnForm.UserChangePassword:
                    return new Users.frmShowUserChangePassword(frmShowManageUsersList.SelectedRow);

                case EnForm.FindPerson:
                    return new People.frmShowFindPersonInfo(frmShowManagePeopleList.SelectedRow);

                case EnForm.ManageApplicationType:
                    return new Applications.ApplicationTypes.frmShowManageApplicationTypesList();

                case EnForm.CreateApplicationTypeInfo:
                    return new Applications.ApplicationTypes.frmShowCreateUpdateApplicationTypeInfo();

                case EnForm.Login:
                    return new Logins.frmShowLoginInfo();

                default:
                    return new Logins.frmShowLoginInfo();
            }
        }

        public void ShowForm(EnForm enForm)
        {
            foreach (var ExistsForm in _ChildForm.Values)
            {
                ExistsForm.Hide();
            }

            if (_ChildForm.TryGetValue(enForm.ToString(), out Form OutForm))
            {
                if (OutForm is People.frmShowPersonInfo child)
                {
                    child.LoadInfo(frmShowManagePeopleList.SelectedRow);
                }
                else if (OutForm is People.frmShowCreateUpdatePersonInfo childOne)
                {
                    childOne.LoadInfo(frmShowManagePeopleList.SelectedRow);
                }
                else if (OutForm is Users.frmShowUserInfo childTwo)
                {
                    childTwo.LoadInfo(frmShowManageUsersList.SelectedRow);
                }
                else if (OutForm is People.frmShowFindPersonInfo childThree)
                {
                    childThree.LoadInfo(frmShowManagePeopleList.SelectedRow);
                }
                else if (OutForm is Users.frmShowCreateUpdateUserInfo ChildFour)
                {
                    ChildFour.LoadInfo(frmShowManageUsersList.SelectedRow);
                }
                else if (OutForm is Users.frmShowUserChangePassword childFive)
                {
                    childFive.LoadInfo(frmShowManageUsersList.SelectedRow);
                }

                Text = OutForm.Text;
                lblMood.Text = OutForm.Text;

                OutForm.BringToFront();
                OutForm.Show();
            }
            else
            {
                Form NewForm = _PushForm(enForm);

                if (NewForm != null)
                {
                    NewForm.MdiParent = this;
                    NewForm.FormBorderStyle = FormBorderStyle.None;
                    NewForm.Dock = DockStyle.Fill;

                    NewForm.Show();

                    Text = NewForm.Text;
                    lblMood.Text = NewForm.Text;

                    _ChildForm[enForm.ToString()] = NewForm;
                }
            }
        }

        public frmShowNewMainMenue()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ( _sidebarExpanded)
            {
                flpSideBar.Width += 10;

                if (flpSideBar.Width == flpSideBar.MaximumSize.Width)
                {
                    lblExpandedClose.Text = "<<   Close";
                    _sidebarExpanded = false;
                    timer1.Stop();
                }
            }
            else
            {
                flpSideBar.Width -= 10;

                if (flpSideBar.Width == flpSideBar.MinimumSize.Width)
                {
                    lblExpandedClose.Text = ">>   Open";
                    _sidebarExpanded = true;
                    timer1.Stop();
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Start();

            if (flpSideBar.Width == 250)
            {
                pictureBox1.Image = Resources.arrow_right_32;
            }
            else
            {
                pictureBox1.Image = Resources.arrow_left_32;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (_homeCollapsible)
            {
                pHome.Height += 10;

                if (pHome.Height == pHome.MaximumSize.Height)
                {
                    _homeCollapsible = false;
                    timer2.Stop();
                }            
            }
            else
            {
                pHome.Height -= 10;

                if (pHome.Height == pHome.MinimumSize.Height)
                {
                    _homeCollapsible = true;
                    timer2.Stop();
                }
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            timer2.Start();
        }

        private void btnPeople_Click(object sender, EventArgs e)
        {
            ShowForm(EnForm.ManagePeople);
        }

        private void UserLoad(bool IsEnabled)
        {
            if (IsEnabled)
                ShowForm(EnForm.ManagePeople);
            else
                ShowForm(EnForm.Login);

            btnPeople.Enabled = IsEnabled;
            btnUsers.Enabled = IsEnabled;
            lblUserName.Text = (IsEnabled) ? ClsGlobal.CurrentUser?.Username : "User Name";
            btnSignOut.Text = (IsEnabled) ? "                    Sign Out" : "                    Sign In";
            btnSignOut.Image = (IsEnabled) ? Resources.logout_32 : Resources.log_in_32;
            btnSignOut.ForeColor = (IsEnabled) ? Color.Red : SystemColors.Control;
            pbUserName.Image = (IsEnabled && File.Exists(ClsGlobal.CurrentUser.PersonInfo.ImagePath)) ? ClsUtils.ResizeAndCorpToCircle(Image.FromFile(ClsGlobal.CurrentUser.PersonInfo.ImagePath), 32, 32, Color.White) : Resources.login_64;
            btnApplications.Enabled = IsEnabled;
        }

        public void ReLoadForm()
        {
            frmShowNewMainMenue_Load(null, null);
        }

        private void frmShowNewMainMenue_Load(object sender, EventArgs e)
        {
            UserLoad(ClsGlobal.CurrentUser != null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void lblMenue_Click(object sender, EventArgs e)
        {
            
        }

        private void lblExpanded_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowForm(EnForm.ManageUsers);
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            ClsGlobal.CurrentUser = null;

            ShowForm(EnForm.Login);
            UserLoad(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowForm(EnForm.ManageApplicationType);
        }
    }
}
