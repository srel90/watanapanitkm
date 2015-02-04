using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wattanapanitkm
{
	public class clsJTDA
	{
        public int jtdaID { get; set; }
        public string docNo { get; set; }
        public string title { get; set; }
        public Nullable<DateTime> informedDateTime { get; set; }
        public Nullable<DateTime> requiringDateTime { get; set; }
        public Nullable<DateTime> successDateTime { get; set; }
        public int? reporterID { get; set; }
        public string systemName { get; set; }
        public string detail { get; set; }
        public string jobType { get; set; }
        public string solution { get; set; }
        public int? practitionersID { get; set; }
        public Double? workProgress { get; set; }
        public int? employeeID { get; set; }
	}
}