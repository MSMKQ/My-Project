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
        public EnMood Mood;

        public enum EnStatus { New = 1, Cancelled = 2, Compeleted = 3 }

        [ClsKey("ApplicationID")]
        public int? ApplicationID { get; set; }
        public int? ApplicationPersonID { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public int? ApplicationTypeID { get; set; }
        public EnStatus? ApplicationStatus { get; set; }
        public string ApplicationStatusText 
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case EnStatus.New:
                        return "New";

                    case EnStatus.Cancelled:
                        return "Cancelled";

                    case EnStatus.Compeleted:
                        return "Compeleted";

                    default:
                        return "Unknown";
                }
            }
        }
        public DateTime? LastStatusDate { get; set; }
        public decimal? PaidFees { get; set; }
        public int? CreatedByUserID { get; set; }

        public ClsPerson PersonInfo;
        public ClsUser CreatedByInfo;
        public ClsApplicationType ApplicationTypeInfo;

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

            if ( _Application != null )
            {
                _Application.Mood = EnMood.Update;
            }

            if ( _Application.ApplicationPersonID.HasValue )
            {
                _Application.PersonInfo = ClsPerson.GetInfoByID(_Application.ApplicationPersonID);
            }

            if ( _Application.CreatedByUserID.HasValue )
            {
                _Application.CreatedByInfo = ClsUser.GetInfoByID(_Application.CreatedByUserID);
            }

            if ( _Application.ApplicationTypeID.HasValue )
            {
                _Application.ApplicationTypeInfo = ClsApplicationType.GetInfoByID(_Application.ApplicationTypeID);
            }

            return _Application;
        }

        private bool Create()
        {
            ApplicationID = ClsFunctions.Create(this);

            return (ApplicationID.HasValue);
        }

        private bool Update()
        {
            return ClsFunctions.Update(this);
        }

        public bool Save()
        {
            switch (Mood)
            {
                case EnMood.Create:
                    if (!Create())
                        return false;

                    Mood = EnMood.Update;
                    return true;

                case EnMood.Update:
                    return Update();

                default:
                    throw new InvalidOperationException($"Unsupport Mood: {Mood}");
            }
        }

        public bool Cancel()
        {
            return ClsApplicationDataAccess.UpdateStatus(ApplicationID, (byte)EnStatus.Cancelled);
        }


    }
}
