using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wattanapanitkm
{
    public class clsUser
    {
        public int usersID { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int? companyID { get; set; }
        public int? sectorID { get; set; }
        public int? departmentID { get; set; }
        public int? partID { get; set; }
        public int? supPartID { get; set; }
        public int? positionID { get; set; }
        public int? headUserID { get; set; }
        public int status { get; set; }

    }
}