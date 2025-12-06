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
    public partial class frmShowFindPersonInfo : Form
    {
        public frmShowFindPersonInfo(int? PersonID)
        {
            InitializeComponent();
            Text = $"Show Person No.{PersonID} Info";
            ctrlShowFindPersonInfo1.LoadPersonInfo(PersonID);
        }

        public void LoadInfo(int? PersonID)
        {
            Text = $"Show Person No.{PersonID} Info";
            ctrlShowFindPersonInfo1.LoadPersonInfo(PersonID);
        }
    }
}
