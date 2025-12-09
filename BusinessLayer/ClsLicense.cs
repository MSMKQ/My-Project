using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("Licenses")]
    public class ClsLicense
    {
        private enum EnMood { Create , Update }
        private EnMood _Mood;

        

        [ClsKey("LicenseID")]
        public int? LicenseID { get; set; }
        public int? ApplicationID { get; set; }
        public int? DriverID { get; set; }
        public int? LicenseClassID { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal? PaidFees { get; set; }
        public bool IsActive { get; set; }
        public byte? IssueResean { get; set; }
        public int? CreatedByUserID { get; set; }

        public ClsLicense()
        {
            LicenseID = null;
            ApplicationID = null;
            DriverID = null;
            LicenseClassID = null;
            IssueDate = null;
            ExpirationDate = null;
            Notes = null;
            PaidFees = null;
            IsActive = true;
            IssueResean = null;
            CreatedByUserID = null;

            _Mood = EnMood.Create;
        }

        public static bool IsThereLicense(int? PersonID, int? LicenseClassID)
        {
            return ClsLicenseDataAccess.IsThereLicense(PersonID, LicenseClassID);
        }
    }
}
