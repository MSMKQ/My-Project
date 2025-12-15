using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ClsTest
    {
        private enum EnMood { Create , Update }
        private EnMood _Mood;

        public int? TestID { get; set; }
        public int? TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int? CreatedByUserID { get; set; }

        public ClsTest()
        {
            TestID = null;
            TestAppointmentID = null;
            TestResult = false;
            Notes = null;
            CreatedByUserID = null;

            _Mood = EnMood.Create;
        }

        public static byte TotalTests(int? LocalDrivingLicenseApplicationID)
        {
            return ClsTestDataAccess.TotalTestsPassed(LocalDrivingLicenseApplicationID);
        }

    }
}
