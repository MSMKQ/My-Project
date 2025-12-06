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


    }
}
