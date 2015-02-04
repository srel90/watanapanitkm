using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wattanapanitkm
{
	public class clsAnswer
	{
        public int ID { get; set; }
        public int qid { get; set; }
        public string answer { get; set; }
        public DateTime datetime { get; set; }
        public int employeeID { get; set; }
        public string fullName { get; set; }
	}
}