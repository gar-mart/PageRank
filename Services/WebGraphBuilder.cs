using PageRankAlgo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageRankAlgo.Services
{
	public class WebGraphBuilder
	{
		private List<WebNode> nodeList = new List<WebNode>();
		private string webGraphFilePath;

		public WebGraphBuilder(string webGraphFilePath)
		{
			this.webGraphFilePath = webGraphFilePath;
		}

		public bool ConvertUrlListToGraph(string filename)
		{
			List<string> urlList = new List<string>();
			if (!ReadUrlText(urlList, filename))
			{
				return false;
			}

			Console.WriteLine(new string('*', 60));
			Console.WriteLine("The list of web nodes:");

			nodeList.Clear();

			for (int i = 0; i < urlList.Count / 2; i++)
			{
				string from = urlList[2 * i];
				string to = urlList[2 * i + 1];
				int fromIndex = FindOrCreateNode(from);
				int toIndex = FindOrCreateNode(to);

				// Create a new edge and add it to the fromNode's edge list
				Edge edge = new Edge(toIndex, 0); // Assuming cost is 0 as in the original example
				nodeList[fromIndex].Edges.Add(edge);
			}

			DebugOutputGraph();

			return true;
		}

		private int FindOrCreateNode(string url)
		{
			int index = nodeList.FindIndex(n => n.Url == url);
			if (index == -1)
			{
				index = nodeList.Count;
				nodeList.Add(new WebNode(index, 0, url));
				Console.WriteLine($"Node {index} : {url}");
			}
			return index;
		}

		private void DebugOutputGraph()
		{
			Console.WriteLine(new string('*', 60));
			Console.WriteLine("The adjacency list of web graph:");

			using (StreamWriter writer = new StreamWriter(webGraphFilePath))
			{
				foreach (var node in nodeList)
				{
					Console.Write(node.Index);
					writer.Write(node.Index);

					foreach (var edge in node.Edges)
					{
						Console.Write($" -> {edge.End}");
						writer.Write($" -> {edge.End}");
					}

					Console.WriteLine();
					writer.WriteLine();
				}
			}
		}

		public bool ReadUrlText(List<string> urlList, string filename)
		{
			try
			{
				// Use File.ReadAllLines for simplicity and efficiency.
				var lines = File.ReadAllLines(filename);
				foreach (var line in lines)
				{
					urlList.Add(line);
					Console.WriteLine(line);
				}
				return true;
			}
			catch (IOException ex)
			{
				// Console.WriteLine($"Error opening data file: {ex.Message}");
				return false;
			}
		}

	}

}
