using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wattanapanitkm
{
	public class clsQuestion
	{
        public int ID { get; set; }
        public string title { get; set; }
        public string question { get; set; }
        public DateTime datetime { get; set; }
        public int employeeID { get; set; }
        public string fullName { get; set; }
        public int count { get; set; }
	}
}