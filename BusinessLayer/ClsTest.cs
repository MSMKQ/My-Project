using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("Tests")]
    public class ClsTest
    {
        private enum EnMood { Create , Update }
        private EnMood _Mood;

        [ClsKey("TestID")]
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

        public static ClsTest GetInfoByID(int? TestID)
        {
            ClsTest _Test = ClsFunctions.GetInfoByID<ClsTest>(TestID);

            if ( _Test != null )
            {
                _Test._Mood = EnMood.Update;
            }

            return _Test;
        }

        public static byte TotalTests(int? LocalDrivingLicenseApplicationID)
        {
            return ClsTestDataAccess.TotalTestsPassed(LocalDrivingLicenseApplicationID);
        }

        public static bool DoesAttendedTest(int? LocalDrivingLicenseApplicationID, ClsTestType.EnTestType TestTypeID)
        {
            return ClsTestDataAccess.DoesAttendedTest(LocalDrivingLicenseApplicationID, Convert.ToInt16(TestTypeID));
        }

        private bool Create()
        {
            TestID = ClsFunctions.Create(this);

            return (TestID.HasValue);
        }

        private bool Update()
        {
            return ClsFunctions.Update(this);
        }

        public bool Save()
        {
            switch ( _Mood )
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
                    throw new InvalidOperationException($"Unsupport Mood: {_Mood}");
            }
        }
    }
}
