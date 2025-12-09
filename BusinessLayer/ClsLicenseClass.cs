using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("LicenseClasses")]
    public class ClsLicenseClass
    {
        private enum EnMood { Create , Update }
        private static EnMood _Mood;

        [ClsKey("LicenseClassID")]
        public int? LicenseClassID { get; set; }
        [ClsString("Title")]
        public string Title { get; set; }
        public string Description { get; set; }
        public byte? MinimumAllowedAge { get; set; }
        public byte? DefaultValiditryLength { get; set; }
        public decimal? Fees { get; set; }

        public ClsLicenseClass()
        {
            LicenseClassID = null;
            Title = null;
            Description = null;
            MinimumAllowedAge = null;
            DefaultValiditryLength = null;
            Fees = null;

            _Mood = EnMood.Create;
        }

        public static ClsLicenseClass GetInfoByID(int? LicenseClassID)
        {
            ClsLicenseClass _LicenseClass = ClsFunctions.GetInfoByID<ClsLicenseClass>(LicenseClassID);

            if ( _LicenseClass != null )
            {
                _Mood = EnMood.Update;
            }

            return _LicenseClass;
        }

        public static ClsLicenseClass GetInfoByTitle(string Title)
        {
            ClsLicenseClass _LicenseClass = ClsFunctions.GetInfoByString<ClsLicenseClass>(Title);

            if ( _LicenseClass != null )
            {
                _Mood = EnMood.Update;
            }

            return _LicenseClass;
        }

        public static DataTable GetLicenseClasses()
        {
            return ClsFunctions.GetDataTable<ClsLicenseClass>();
        }
    }
}
