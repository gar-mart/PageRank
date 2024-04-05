using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageRankAlgo.Models
{
	public class WebNode
	{
		public WebNode(int index, double prScore, string url)
		{
			Index = index;
			PrScore = prScore;
			Url = url;
		}

		public int Index { get; set; }
		public double PrScore { get; set; }
		public string Url { get; set; }
		public List<Edge> Edges { get; set; } = new List<Edge>();


	}
}
