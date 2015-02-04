using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wattanapanitkm
{
	public class clsDepartment
	{
        public int departmentID { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public int? sectorID { get; set; }
        public int? headID { get; set; }
        public int? status { get; set; }
	}
}