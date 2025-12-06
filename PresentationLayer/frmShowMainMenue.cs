using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class frmShowMainMenue : Form
    {
        public frmShowMainMenue()
        {
            InitializeComponent();
        }

        private Dictionary<string, Form> _ChildForm = new Dictionary<string, Form>();

        private enum EnForm { ManagePeople }

        private void _ShowForm(EnForm enForm)
        {
            foreach (var item in _ChildForm)
            {
                item.Value.Hide();
            }

            if (_ChildForm.TryGetValue(enForm.ToString(), out Form ExistsForm))
            {
                Text = ExistsForm.Text;
                lblMood.Text = ExistsForm.Text;
                ExistsForm.Show();
                ExistsForm.BringToFront();
            }
            else
            {
                Form newForm = _PushForm(enForm);

                if (newForm != null)
                {
                    newForm.MdiParent = this;
                    Text = newForm.Text;
                    lblMood.Text = newForm.Text;
                    newForm.Dock = DockStyle.Fill;
                    newForm.FormBorderStyle = FormBorderStyle.None;
                    newForm.WindowState = FormWindowState.Normal;

                    newForm.Show();
                    _ChildForm[newForm.ToString()] = newForm;
                }
                else
                {
                    Text = "Show Main Menue";
                    lblMood.Text = "Show Main Menue";
                }
            }
        }

        private Form _PushForm(EnForm enForm)
        {
            switch (enForm)
            {
                case EnForm.ManagePeople:
                    return new People.frmShowManagePeopleList();

                default:
                    return null;
            }
        }

        private void frmShowMainMenue_Load(object sender, EventArgs e)
        {

        }

        private void tsmiPeople_Click(object sender, EventArgs e)
        {
            _ShowForm(EnForm.ManagePeople);
        }
    }
}
