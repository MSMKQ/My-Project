using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("ApplicationTypes")]
    public class ClsApplicationType
    {
        private enum EnMood { Create , Update }
        private static EnMood _Mood;

        public enum EnServices 
        { 
            NewLocalDriving = 1, RenewDrivingLicense = 2, ReplacementForLost = 3, ReplacementForDamage = 4, ReleaseForDetained = 5, NewInternationalLicense = 6, RetakeTest = 7
        }

        [ClsKey("ApplicationTypeID")]
        public int? ApplicationTypeID { get; set; }
        public string Title { get; set; }
        public decimal? Fees { get; set; }

        public ClsApplicationType()
        {
            ApplicationTypeID = null;
            Title = null;
            Fees = null;

            _Mood = EnMood.Create;
        }

        public static DataTable GetApplicationTypes()
        {
            return ClsFunctions.GetDataTable<ClsApplicationType>();
        }

        public static ClsApplicationType GetInfoByID(int? AppliationTypeID)
        {
            ClsApplicationType _ApplicationType = ClsFunctions.GetInfoByID<ClsApplicationType>(AppliationTypeID);

            if ( _ApplicationType != null )
            {
                _Mood = EnMood.Update;
            }

            return _ApplicationType;
        }

        public static ClsApplicationType GetInfoByID(EnServices enServices)
        {
            ClsApplicationType _ApplicationType = ClsFunctions.GetInfoByID<ClsApplicationType>((int)enServices);

            if ( _ApplicationType != null )
            {
                _Mood = EnMood.Update;
            }

            return _ApplicationType;
        }

        private bool Create()
        {
            ApplicationTypeID = ClsFunctions.Create(this);

            return (ApplicationTypeID.HasValue);
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
                    return false;
            }
        }
    }
}
