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
    public partial class ctrlShowFindPersonInfo : UserControl
    {
        public int? PersonID { get { return ctrlShowPersonInfo1.PersonID; } }

        public void FilterFocus() { txtFilterValue.Focus(); }

        public ctrlShowFindPersonInfo()
        {
            InitializeComponent();
        }

        private void _FindPersonInfo()
        {
            if ( !string.IsNullOrWhiteSpace( txtFilterValue.Text ) )
            {
                if (int.TryParse( txtFilterValue.Text.Trim(), out int PersonID))
                {
                    ctrlShowPersonInfo1.LoadInfo(PersonID);
                }
                else
                {
                    ctrlShowPersonInfo1.LoadInfo(txtFilterValue.Text.Trim());
                }
            }
        }

        public void LoadPersonInfo(int? PersonID)
        {
            txtFilterValue.Text = PersonID.ToString();
            _FindPersonInfo();
        }

        public void LoadPersonInfo(string NationID)
        {
            txtFilterValue.Text = NationID;
            _FindPersonInfo();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            _FindPersonInfo();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                btnFind.PerformClick();
        }
    }
}
