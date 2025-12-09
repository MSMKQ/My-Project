using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("LocalDrivingLicenseApplications")]
    public class ClsLocalDrivingLicenseApplication
    {
        private enum EnMood { Create , Update }
        private static EnMood _Mood;

        [ClsKey("LocalDrivingLicenseApplicationID")]
        public int? LocalDrivingLicenseApplicationID { get; set; }
        public int? LicenseClassID { get; set; }

        public ClsLicenseClass LicenseClassInfo;

        public ClsLocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationID = null;
            LicenseClassID = null;

            _Mood = EnMood.Create;
        }

        public static DataTable GetApplications()
        {
            return ClsLocalDrivingLicenseApplicationDataAccess.GetApplications<ClsLocalDrivingLicenseApplication>();
        }


    }
}
