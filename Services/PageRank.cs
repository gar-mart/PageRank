using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PageRankAlgo.Models;

namespace PageRankAlgo.Services
{
	public class PageRank
	{
		private const double DampingFactor = 0.85;
		private const double ConvergenceThreshold = 1e-4;

		public void ComputePageRank(List<WebNode> nodeList)
		{
			int nodeCount = nodeList.Count;
			double[] oldScores = new double[nodeCount];
			double[] newScores = new double[nodeCount];

			// Initialize nodes
			for (int i = 0; i < nodeCount; i++)
			{
				oldScores[i] = 1.0 / nodeCount;
			}

			bool hasConverged = false;
			while (!hasConverged)
			{
				for (int i = 0; i < nodeCount; i++)
				{
					double rankFromIncomingLinks = 0.0;

					// Sum of PageRank contributions from incoming links
					foreach (var edge in nodeList.Where(n => n.Edges.Any(e => e.End == i)))
					{
						int outDegree = nodeList[edge.Index].Edges.Count;
						rankFromIncomingLinks += oldScores[edge.Index] / outDegree;
					}

					newScores[i] = (1 - DampingFactor) / nodeCount + DampingFactor * rankFromIncomingLinks;
				}

				hasConverged = true;
				for (int i = 0; i < nodeCount; i++)
				{
					if (Math.Abs(newScores[i] - oldScores[i]) > ConvergenceThreshold)
					{
						hasConverged = false;
					}
					oldScores[i] = newScores[i];
				}
			}

			// Update nodes with the final PageRank scores
			for (int i = 0; i < nodeCount; i++)
			{
				nodeList[i].PrScore = oldScores[i];
			}
		}



		public void SortPrScore(List<WebNode> nodeList)
		{
			string outputPath = "output.txt";


			// Sorting nodeList based on PrScore in descending order
			var sortedNodes = nodeList.OrderByDescending(node => node.PrScore).ToList();

			// Write sorted PageRank scores to a file
			using (StreamWriter writer = new StreamWriter(outputPath))
			{
				foreach (var node in sortedNodes)
				{
					string outputLine = $"Node {node.Index} PageRank {node.PrScore} URL: {node.Url}";
					writer.WriteLine(outputLine);
				}
			}
		}
		public async Task CrawlAsync(string startUrl, string fileName, int rLimit)
		{
			HttpClient _httpClient = new HttpClient();
			Regex _urlRegex = new Regex(@"href=""(?<url>https?://[^""]*)""", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			HashSet<string> markedUrls = new HashSet<string>();
			Queue<string> urlsToVisit = new Queue<string>();
			urlsToVisit.Enqueue(startUrl);
			markedUrls.Add(startUrl);

			int visitedUrlCount = 0;

			while (urlsToVisit.Count > 0 && visitedUrlCount < rLimit)
			{
				var currentUrl = urlsToVisit.Dequeue();
				try
				{
					string content = await _httpClient.GetStringAsync(currentUrl);
					visitedUrlCount++;

					foreach (Match match in _urlRegex.Matches(content))
					{
						var url = match.Groups["url"].Value;
						if (!markedUrls.Contains(url))
						{
							markedUrls.Add(url);
							urlsToVisit.Enqueue(url);
						}
					}
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine($"Failed to fetch {currentUrl}: {e.Message}");
				}

				// Write the URLs to a file.
				await WriteToFileAsync(fileName, currentUrl, markedUrls);
			}
		}

		private async Task WriteToFileAsync(string fileName, string currentUrl, HashSet<string> urls)
		{
			using (StreamWriter writer = new StreamWriter(fileName, true))
			{
				await writer.WriteLineAsync($"Visited: {currentUrl}");
				foreach (var url in urls)
				{
					await writer.WriteLineAsync($"  Found: {url}");
				}
			}
		}
	}

}


