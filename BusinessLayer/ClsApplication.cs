using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("Applications")]
    public class ClsApplication
    {
        public enum EnMood { Create , Update }
        public static EnMood Mood;

        [ClsKey("ApplicationID")]
        public int? ApplicationID { get; set; }
        public int? ApplicationPersonID { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public int? ApplicationTypeID { get; set; }
        public byte? ApplicationStatus { get; set; }
        public DateTime? LastStatusDate { get; set; }
        public decimal? PaidFees { get; set; }
        public int? CreatedByUserID { get; set; }

        public ClsUser CreatedByInfo;

        public ClsApplication()
        {
            ApplicationID = null;
            ApplicationPersonID = null;
            ApplicationDate = null;
            ApplicationTypeID = null;
            ApplicationStatus = null;
            LastStatusDate = null;
            PaidFees = null;
            CreatedByUserID = null;

            Mood = EnMood.Create;
        }

        public static ClsApplication GetInfoByID(int? ApplicationID)
        {
            ClsApplication _Application = ClsFunctions.GetInfoByID<ClsApplication>(ApplicationID);

            if ( _Application != null)
            {
                Mood = EnMood.Update;
            }

            if ( _Application.CreatedByUserID.HasValue )
            {
                _Application.CreatedByInfo = ClsUser.GetInfoByID(_Application.CreatedByUserID);
            }

            return _Application;
        }
    }
}
