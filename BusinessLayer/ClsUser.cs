using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [ClsTable("Users")]
    public class ClsUser
    {
        private static string _table = "Users";

        
        private enum EnMood { Create , Update }
        private static EnMood _Mood;

        [ClsKey("UserID")]
        public int? UserID { get; set; }
        public int? PersonID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public ClsPerson PersonInfo;

        public ClsUser()
        {
            UserID = null;
            PersonID = null;
            Username = null;
            Password = null;
            IsActive = true;

            _Mood = EnMood.Create;
        }

        public static ClsUser GetInfoByID(int? UserID)
        {
            ClsUser _User = ClsFunctions.GetInfoByID<ClsUser>(UserID);

            if (_User != null)
            {
                _Mood = EnMood.Update;
            }

            if (_User != null && _User.PersonID.HasValue)
            {
                _User.PersonInfo = ClsPerson.GetInfoByID(_User.PersonID);
            }

            return _User;
        }

        public static ClsUser GetInfoByUsernameAndPassword(string username, string password)
        {
            ClsUser _User = ClsFunctions.GetInfoByString<ClsUser>(username, nameof(username), password, nameof(password));

            if (_User != null)
            {
                _Mood = EnMood.Update;
            }

            if (_User != null && _User.PersonID.HasValue)
            {
                _User.PersonInfo = ClsPerson.GetInfoByID(_User.PersonID);
            }


            return _User;
        }

        public static DataTable GetUsers()
        {
            return ClsUserDataAccess.GetUsers(_table);
        }

        public static bool Delete(int UserID)
        {
            return ClsFunctions.DeleteById <ClsUser> (UserID);
        }

        public static bool IsExists(int? PersonID)
        {
            return ClsFunctions.IsExists<ClsUser>(new Dictionary<string, object> { { "PersonID", PersonID } });
        }

        public static bool IsExists(int? UserID, string Password)
        {
            return ClsFunctions.IsExists<ClsUser>(new Dictionary<string, object> { { "UserID", UserID }, { "Password", Password } });
        }

        private bool Create()
        {
            UserID = ClsFunctions.Create(this);

            return (UserID.HasValue);
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

        public static bool Update(int? UserID, string Password)
        {
            return ClsFunctions.Update<ClsUser>(new Dictionary<string, object> { { "UserID", UserID } , { "Password", Password } });
        }
    }
}
