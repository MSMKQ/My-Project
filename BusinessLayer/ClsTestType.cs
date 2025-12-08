using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("TestTypes")]
    public class ClsTestType
    {
        private enum EnMood { Create , Update }
        private static EnMood _Mood;

        public enum EnTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }

        [ClsKey("TestTypeID")]
        public int? TestTypeID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Fees { get; set; }

        public ClsTestType()
        {
            TestTypeID = null;
            Title = null;
            Description = null;
            Fees = null;

            _Mood = EnMood.Create;
        }

        public static DataTable GetTestTypes()
        {
            return ClsFunctions.GetDataTable<ClsTestType>();
        }

        public static ClsTestType GetInfoByID(EnTestType enTestType)
        {
            ClsTestType _TestType = ClsFunctions.GetInfoByID<ClsTestType>(Convert.ToInt32(enTestType));

            if (_TestType != null)
            {
                _Mood = EnMood.Update;
            }

            return _TestType;
        }

        private bool Create()
        {
            TestTypeID = ClsFunctions.Create(this);

            return (TestTypeID.HasValue);
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
