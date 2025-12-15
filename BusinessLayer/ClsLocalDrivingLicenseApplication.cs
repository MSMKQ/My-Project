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
        private EnMood _Mood;

        [ClsKey("LocalDrivingLicenseApplicationID")]
        public int? LocalDrivingLicenseApplicationID { get; set; }
        public int? LicenseClassID { get; set; }

        public ClsLicenseClass LicenseClassInfo;

        public string FullName {  get { return ClsPerson.GetInfoByID(ApplicationPersonID).FullName; } }

        public ClsLocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationID = null;
            ApplicationID = null;
            LicenseClassID = null;

            _Mood = EnMood.Create;
        }

        public ClsLocalDrivingLicenseApplication(ClsLocalDrivingLicenseApplication _Local, ClsApplication _App)
        {
            LocalDrivingLicenseApplicationID = _Local.LocalDrivingLicenseApplicationID;
            ApplicationID = _App.ApplicationID;
            ApplicationPersonID = _App.ApplicationPersonID;
            ApplicationDate = _App.ApplicationDate;
            ApplicationTypeID = _App.ApplicationTypeID;
            ApplicationStatus = _App.ApplicationStatus;
            LastStatusDate = _App.LastStatusDate;
            PaidFees = _App.PaidFees;
            CreatedByUserID = _App.CreatedByUserID;
            LicenseClassID = _Local.LicenseClassID;

            LicenseClassInfo = ClsLicenseClass.GetInfoByID(_Local.LicenseClassID);
            _Mood = EnMood.Update;
        }

        public static DataTable GetApplications()
        {
            return ClsLocalDrivingLicenseApplicationDataAccess.GetApplications<ClsLocalDrivingLicenseApplication>();
        }

        public static ClsLocalDrivingLicenseApplication GetInfoByID(int? LocalDrivingLicenseApplicationID)
        {
            ClsLocalDrivingLicenseApplication _Local = ClsFunctions.GetInfoByID<ClsLocalDrivingLicenseApplication>(LocalDrivingLicenseApplicationID);

            if (_Local != null && _Local.ApplicationID.HasValue)
            {
                _Local._Mood = EnMood.Update;

                ClsApplication _App = ClsApplication.GetInfoByID(_Local.ApplicationID);


                return new ClsLocalDrivingLicenseApplication(_Local, _App);

            }
            else
                return null;
        }

        public static int? IsThereAnActiveApplication(int? ApplicationPersonID, int? LicenseClassID)
        {
            return ClsLocalDrivingLicenseApplicationDataAccess.IsThereAnActiveApplication(ApplicationPersonID, LicenseClassID);
        }

        private bool Create()
        {
            LocalDrivingLicenseApplicationID = ClsFunctions.Create(this);

            return (LocalDrivingLicenseApplicationID.HasValue);
        }

        private bool Update()
        {
            return ClsFunctions.Update(this);
        }

        public bool Save()
        {
            Mood = (ClsApplication.EnMood)_Mood;

            if (!base.Save())
                return false;

            switch ( _Mood )
            {
                case EnMood.Create:
                    if (!Create())
                        return false;
                    
                    _Mood = EnMood.Update;
                    return true;

                case EnMood.Update:
                    return Update();

                default:
                    throw new InvalidOperationException($"Unsupport Mood: {_Mood}.");
            }
        }

        public static bool Delete(int? LocalDrivingLicenseApplicationID)
        {
            return ClsFunctions.Delete<ClsLocalDrivingLicenseApplication>(LocalDrivingLicenseApplicationID);
        }

        public bool DoesPassedTest(ClsTestType.EnTestType TestTypeID)
        {
            return ClsLocalDrivingLicenseApplicationDataAccess.DoesPassedTest(LocalDrivingLicenseApplicationID, (byte)TestTypeID);
        }


    }
}
