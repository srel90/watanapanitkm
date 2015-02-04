using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wattanapanitkm
{
	public class clsCompany
	{
        public int companyID { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public int? status { get; set; }
	}
}