using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PageRankAlgo.Models
{
	public class AlgoPara
	{
		public Uri StartAddr { get; set; }
		public string LinkList { get; set; }
		public List<Uri> UrlList { get; set; }
		public string WebGraph { get; set; }
		public Regex Include { get; set; }
		public Regex Exclude { get; set; }
		public int RLimit { get; set; }
		public double D { get; set; }
		public int Preference { get; set; }
	}
}
