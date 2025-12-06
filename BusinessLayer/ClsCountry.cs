using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("Countries")]
    public class ClsCountry
    {
        private static string _table = "Countries";


        private enum EnMood { Create, Update }
        private static EnMood _Mood;

        [ClsKey("CountryID")]
        public int? CountryID { get; set; }
        public string CountryName { get; set; }


        public ClsCountry()
        {
            CountryID = null;
            CountryName = null;

            _Mood = EnMood.Create;
        }

        public static ClsCountry GetInfoByID(int? CountryID)
        {
            ClsCountry _Country = ClsFunctions.GetInfoByID<ClsCountry>(CountryID);

            if (_Country != null)
            {
                _Mood = EnMood.Update;
            }

            return _Country;
        }

        public static ClsCountry GetInfoByCountry(string CountryName)
        {
            ClsCountry _Country = ClsFunctions.GetInfoByString<ClsCountry>(_table, CountryName, nameof(CountryName));

            if (_Country != null)
            {
                _Mood = EnMood.Update;
            }

            return _Country;
        }

        public static DataTable GetCountries()
        {
            return ClsFunctions.GetDataTable<ClsCountry>();
        }

        private bool Create()
        {
            CountryID = ClsFunctions.Create(_table, nameof(CountryID), this);

            return (CountryID.HasValue);
        }

        private bool Update()
        {
            return ClsFunctions.Update(_table, nameof(CountryID), this);
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
