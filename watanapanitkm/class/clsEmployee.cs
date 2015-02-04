using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wattanapanitkm
{
	public class clsEmployee
	{
        public int employeeID { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int? refUserID { get; set; }
        public int? status { get; set; }
	}
}