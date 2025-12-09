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
    public class ClsLocalDrivingLicenseApplication : ClsApplication
    {
        private enum EnMood { Create , Update }
        private static EnMood _Mood;

        [ClsKey("LocalDrivingLicenseApplicationID")]
        public int? LocalDrivingLicenseApplicationID { get; set; }
        public int? LicenseClassID { get; set; }

        public ClsLicenseClass LicenseClassInfo;
        public ClsApplication ApplicationInfo;

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

        public static ClsLocalDrivingLicenseApplication GetInfoByID(int? LocalDrivingLicenseApplicationID)
        {
            ClsLocalDrivingLicenseApplication _Local = ClsFunctions.GetInfoByID<ClsLocalDrivingLicenseApplication>(LocalDrivingLicenseApplicationID);

            if ( _Local != null )
            {
                _Mood = EnMood.Update;
            }

            if ( _Local.LicenseClassID.HasValue)
            {
                _Local.LicenseClassInfo = ClsLicenseClass.GetInfoByID(_Local.LicenseClassID);
            }

            if ( _Local.ApplicationID.HasValue )
            {
                _Local.ApplicationInfo = ClsApplication.GetInfoByID(_Local.ApplicationID);
            }

            return _Local;
        }

        public static int? IsThereAnActiveApplication(int? ApplicationPersonID, int? LicenseClassID)
        {
            return ClsLocalDrivingLicenseApplicationDataAccess.IsThereAnActiveApplication(ApplicationPersonID, LicenseClassID);
        }
    }
}
