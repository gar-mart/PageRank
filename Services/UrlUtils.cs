using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.IO;

namespace PageRankAlgo.Services
{


	public class UrlUtils
	{
		// Regex pattern to match URLs 
		private const string UrlPattern = @"href\s*=\s*(?:""(?<url>https?://[^""]+)""|'(?<url>https?://[^']+)')";
		private readonly Regex _urlRegex = new Regex(UrlPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public List<string> ExtractUrlsFromHtml(string htmlContent, Uri baseUrl)
		{
			var urlList = new List<string>();

			foreach (Match match in _urlRegex.Matches(htmlContent))
			{
				string url = match.Groups["url"].Value;
				if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
				{
					url = new Uri(baseUrl, url).AbsoluteUri;
				}

				urlList.Add(url);
			}

			return urlList;
		}


		public async Task<string> LoadWebpageToStr(string url)
		{
			HttpClient _httpClient = new HttpClient();
			string content = await _httpClient.GetStringAsync(url);
			return content;
		}


		// Fetch the content of the webpage and extract URLs.
		public async Task ExtractUrlFromWebpageAsync(List<string> urlList, string url, string fileName)
		{
			HttpClient _httpClient = new HttpClient();

			string pageContent = await _httpClient.GetStringAsync(url);

			// Extract URLs from the content.
			foreach (Match match in _urlRegex.Matches(pageContent))
			{
				string extractedUrl = match.Groups["url"].Value;
				urlList.Add(extractedUrl);
			}

			// Write the extracted URLs to a file.
			using (StreamWriter streamWriter = new StreamWriter(fileName, true)) 
			{
				foreach (string extractedUrl in urlList)
				{
					streamWriter.WriteLine($"{url} {extractedUrl}");
				}
				streamWriter.Flush();
			}
		}

		public bool SearchUrl(List<string> urlList, string str)
		{
			return urlList.Contains(str);
		}

	}


}

