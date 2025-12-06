using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.People
{
    public partial class frmShowPersonInfo : Form
    {
        private int? _PersonID;

        public frmShowPersonInfo(int? PersonID)
        {
            InitializeComponent();
            
            _PersonID = PersonID;
            ctrlShowPersonInfo1.ResetToDefault();
        }

        public void LoadInfo(int? PersonID)
        {
            _PersonID = PersonID;
            frmShowPersonInfo_Load(null, null);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (MdiParent is frmShowNewMainMenue parent)
                parent.ShowForm(frmShowNewMainMenue.EnForm.ManagePeople);
        }

        private void frmShowPersonInfo_Load(object sender, EventArgs e)
        {
            Text = $"Show Person No.{_PersonID} Information";
            ctrlShowPersonInfo1.LoadInfo(_PersonID);
        }
    }
}
