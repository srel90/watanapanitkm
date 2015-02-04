using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wattanapanitkm
{
	public class clsSubPart
	{
        public int subPartID { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public int? partID { get; set; }
        public int? status { get; set; }
	}
}