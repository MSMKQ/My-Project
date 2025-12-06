using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("People")]
    public class ClsPerson
    {
        private static string _table = "People";

        private enum EnMood { Create , Update }
        private static EnMood _Mood;
        public enum EnGender { Female = 0 , Male = 1, Unknown }

        [ClsKey("PersonID")]
        public int? PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string NationID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public EnGender? Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? CountryID { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }

        public ClsCountry CountryInfo;

        [ClsIgnore]
        public string FullName
        {
            get
            {
                return string.Join(" ", new string[] { FirstName, SecondName, ThirdName, LastName }.Where(name => !string.IsNullOrWhiteSpace(name)));
            }
        }

        public ClsPerson()
        {
            PersonID = null;
            FirstName = null;
            SecondName = null;
            ThirdName = null;
            LastName = null;
            NationID = null;
            DateOfBirth = null;
            Gender = null;
            Phone = null;
            Email = null;
            CountryID = null;
            Address = null;
            ImagePath = null;

            _Mood = EnMood.Create;
        }

        public static ClsPerson GetInfoByID(int? PersonID)
        {
            ClsPerson _Person = ClsFunctions.GetInfoByID<ClsPerson>(PersonID);

            if (_Person != null)
            {
                _Mood = EnMood.Update;
            }

            if (_Person != null && _Person.CountryID.HasValue)
            {
                _Person.CountryInfo = ClsCountry.GetInfoByID(_Person.CountryID);
            }

            return _Person;
        }

        public static ClsPerson GetInfoByID(string NationID)
        {
            ClsPerson _Person = ClsFunctions.GetInfoByString<ClsPerson>(_table, NationID, nameof(NationID));

            if (_Person != null)
            {
                _Mood = EnMood.Update;

                _Person.CountryInfo = ClsCountry.GetInfoByID(_Person.CountryID);
            }

            return _Person;
        }

        public static DataTable GetPeople()
        {
            return ClsPersonDataAccess.GetPeople(_table);
        }

        private bool Create()
        {
            PersonID = ClsFunctions.Create(_table, nameof(PersonID), this);

            return (PersonID.HasValue);
        }

        private bool Update()
        {
            return ClsFunctions.Update(_table, nameof(PersonID), this);
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

        public static bool Delete(int? PersonID)
        {
            return ClsFunctions.Delete(_table, PersonID, nameof(PersonID));
        }

        public static string GetNationID()
        {
            string NationID = ClsPersonDataAccess.GetLastNationID(_table).Substring(3, 3);

            int Digi = Convert.ToInt32(NationID);

            return $"NAT{(Digi + 1)}";
        }
    }
}
