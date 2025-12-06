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
    public partial class ctrlShowPersonRowInfo : UserControl
    {
        private int? _PersonID;
        private string _FullName;

        public int? PersonID { get { return _PersonID; } set { _PersonID = value; } }
        public string FullName { get { return _FullName; } set { _FullName = value; } }


        public ctrlShowPersonRowInfo()
        {
            InitializeComponent();
        }

        
    }
}
