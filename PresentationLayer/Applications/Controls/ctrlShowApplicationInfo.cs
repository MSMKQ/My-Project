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

namespace PresentationLayer.Applications.Controls
{
    public partial class ctrlShowApplicationInfo : UserControl
    {
        private ClsApplication _Application;

        public ctrlShowApplicationInfo()
        {
            InitializeComponent();
        }

        private void _ResetToDefault()
        {
            lblApplicationID.Text = "ApplicationID";
            lblApplicationPersonID.Text = "FullName";
            lblApplicationDate.Text = "Date";
            lblApplicationTypeID.Text = "Type";
            lblApplicationStatus.Text = "Status";
            lblLastStatusDate.Text = "LastStatus";
            lblPaidFees.Text = "PaidFees";
            lblCreatedByUserID.Text = "CreatedBy";
        }

        private void _LoadInfo()
        {
            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblApplicationPersonID.Text = _Application.PersonInfo.FullName;
            lblApplicationDate.Text = _Application.ApplicationDate.Value.ToShortDateString();
            lblApplicationTypeID.Text = _Application.ApplicationTypeInfo.Title;
            lblApplicationStatus.Text = _Application.ApplicationStatusText.ToString();
            lblLastStatusDate.Text = _Application.LastStatusDate.Value.ToShortDateString();
            lblPaidFees.Text = _Application.PaidFees.Value.ToString("N3");
            lblCreatedByUserID.Text = _Application.CreatedByInfo.Username;
        }

        public void LoadInfo(int? ApplicationID)
        {
            _Application = ClsApplication.GetInfoByID(ApplicationID);

            if (_Application == null)
            {
                _ResetToDefault();
            }

            _LoadInfo();
        }
    }
}
