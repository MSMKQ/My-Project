using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("TestAppointments")]
    public class ClsTestAppointment
    {
        private enum EnMood { Create , Update }
        private EnMood _Mood;

        [ClsKey("TestAppointmentID")]
        public int? TestAppointmentID { get; set; }
        public ClsTestType.EnTestType? TestTypeID { get; set; }
        public int? LocalDrivingLicenseApplicationID { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public decimal? PaidFees { get; set; }
        public int? CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; }

        [ClsIgnore]
        public int? TestID { get { return GetTestID(); } }

        public ClsApplication RetakeTestApplicationInfo;
        

        public ClsTestAppointment()
        {
            TestAppointmentID = null;
            TestTypeID = null;
            LocalDrivingLicenseApplicationID = null;
            TestAppointmentID = null;
            AppointmentDate = null;
            PaidFees = null;
            CreatedByUserID = null;
            IsLocked = false;
            RetakeTestApplicationID = null;

            _Mood = EnMood.Create;
        }

        public static ClsTestAppointment GetInfoByID(int? TestAppointmentID)
        {
            ClsTestAppointment _TestAppointment = ClsFunctions.GetInfoByID<ClsTestAppointment>(TestAppointmentID);

            if ( _TestAppointment != null )
            {
                _TestAppointment._Mood = EnMood.Update;
            }

            if ( _TestAppointment.RetakeTestApplicationID.HasValue )
            {
                _TestAppointment.RetakeTestApplicationInfo = ClsApplication.GetInfoByID(_TestAppointment.RetakeTestApplicationID);
            }

            return _TestAppointment;
        }

        public static DataTable GetAppointments(int? LocalDrivingLicenseApplicationID, ClsTestType.EnTestType TestTypeID)
        {
            return ClsTestAppointmentDataAccess.GetAppointments(LocalDrivingLicenseApplicationID, Convert.ToInt16(TestTypeID));
        }

        public static bool IsThereAnActiveAppointment(int? LocalDrivingLicenseApplicationID, int? TestAppointmentID)
        {
            return ClsTestAppointmentDataAccess.IsThereAnActiveAppointment(LocalDrivingLicenseApplicationID, TestAppointmentID);
        }

        public static byte TotalTrails(int? LocalDrivingLicenseApplicationID, ClsTestType.EnTestType TestTypeID)
        {
            return ClsTestAppointmentDataAccess.TotalTrails(LocalDrivingLicenseApplicationID, Convert.ToInt16(TestTypeID));
        }

        private bool Create()
        {
            TestAppointmentID = ClsFunctions.Create(this);

            return (TestAppointmentID.HasValue);
        }

        private bool Update()
        {
            return ClsFunctions.Update(this);
        }

        public bool Save()
        {
            switch (_Mood)
            {
                case EnMood.Create:
                    if (Create())
                    {
                        _Mood = EnMood.Update;
                        return true;
                    }
                    else
                        return false;

                case EnMood.Update:
                    return Update();

                default:
                    throw new InvalidOperationException($"Unsupoort this Mood: {_Mood}");
            }
        }

        private int? GetTestID()
        {
            return ClsTestDataAccess.GetTestID(TestAppointmentID);
        }
    }
}
