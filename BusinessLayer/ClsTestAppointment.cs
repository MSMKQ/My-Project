using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ClsTestAppointment
    {
        private enum EnMood { Create , Update }
        private EnMood _Mood;

        public int? TestAppointmentID { get; set; }
        public int? LocalDrivingLicenseApplicationID { get; set; }
        public DateTime? AppointmetDate { get; set; }
        public decimal? PaidFees { get; set; }
        public int? CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; }

        public ClsTestAppointment()
        {
            TestAppointmentID = null;
            LocalDrivingLicenseApplicationID = null;
            TestAppointmentID = null;
            PaidFees = null;
            CreatedByUserID = null;
            IsLocked = false;
            RetakeTestApplicationID = null;

            _Mood = EnMood.Create;
        }

        public static DataTable GetAppointments(int? LocalDrivingLicenseApplicationID, ClsTestType.EnTestType TestTypeID)
        {
            return ClsTestAppointmentDataAccess.GetAppointments(LocalDrivingLicenseApplicationID, Convert.ToInt16(TestTypeID));
        }

        public static bool IsThereAnActiveAppointment(int? LocalDrivingLicenseApplicationID, int? TestAppointmentID)
        {
            return ClsTestAppointmentDataAccess.IsThereAnActiveAppointment(LocalDrivingLicenseApplicationID, TestAppointmentID);
        }
    }
}
