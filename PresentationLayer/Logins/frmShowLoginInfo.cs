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

namespace PresentationLayer.Logins
{
    public partial class frmShowLoginInfo : Form
    {
        private ClsUser _User;

        public frmShowLoginInfo()
        {
            InitializeComponent();
        }

        private void frmShowLoginInfo_Load(object sender, EventArgs e)
        {
            lblMessageError.Visible = false;

            checkBox1.Checked = ClsRegistry.GetValue("RememberMe").ToString().Equals("true", StringComparison.OrdinalIgnoreCase);
            
            if ( checkBox1.Checked )
            {
                txtUsername.Text = ClsRegistry.GetValue("Username").ToString();
                txtPassword.Text = ClsValidations.DecryptPassword(ClsRegistry.GetValue("Password").ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Username = txtUsername.Text.Trim(), Password = txtPassword.Text.Trim();

            _User = ClsUser.GetInfoByUsernameAndPassword(Username, ClsValidations.HashPasswords(Password));

            if ( _User == null )
            {
                lblMessageError.Visible = true;
                lblMessageError.Text = "Invalid Username Or Password";
                return;
            }

            if ( !_User.IsActive )
            {
                lblMessageError.Visible = true;
                lblMessageError.Text = "This Username is Locked";
                return;
            }

            if ( checkBox1.Checked )
            {
                ClsRegistry.SetValue("Username", Username);
                ClsRegistry.SetValue("Password", ClsValidations.EncryptPassword(Password));
                ClsRegistry.SetValue("RememberMe", true);
            }
            else
            {
                ClsRegistry.SetValue("Username", string.Empty);
                ClsRegistry.SetValue("Password", string.Empty);
                ClsRegistry.SetValue("RememberMe", false);
            }

            ClsGlobal.CurrentUser = _User;

            Messages.frmShowMessageInfo frm = new Messages.frmShowMessageInfo(Messages.frmShowMessageInfo.EnMood.Successfully, "Login In", $"Welcome Back {_User.Username}");
            frm.Show();

            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManagePeople);
                parent.ReLoadForm();
            }
        }
    }
}
