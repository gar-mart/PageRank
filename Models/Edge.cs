using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageRankAlgo.Models
{
	public class Edge
	{
		public Edge(int end, int cost, Edge? next)
		{
			End = end;
			Cost = cost;
			Next = next;
		}
		public Edge(int end, int cost)
		{
			End = end;
			Cost = cost;
		}
		public int End { get; set; }
		public int Cost { get; set; }
		public Edge? Next { get; set; }
	}
}
