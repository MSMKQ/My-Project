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
    public partial class frmShowUserInfo : Form
    {
        public frmShowUserInfo(int? UserID)
        {
            InitializeComponent();

            Text = $"Show User No.{UserID} Info";
            ctrlShowUserInfo1.LoadUser(UserID);
        }

        public void LoadInfo(int? UserID)
        {
            Text = $"Show User No.{UserID} Info";
            ctrlShowUserInfo1.LoadUser(UserID);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
            {
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManageUsers);
            }
        }
    }
}
