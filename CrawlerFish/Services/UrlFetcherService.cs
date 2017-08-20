using CrawlerFish.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace CrawlerFish.Services {
	public class UrlFetcherService : IFetcherService {
		/// <summary>
		/// Retrive a url client page as plain text 
		/// </summary>
		public string RetrieveUrlAsPlainText(string url) {
			WebClient client = new System.Net.WebClient();
			byte[] pageBytes = client.DownloadData(url);
			return Encoding.UTF8.GetString(pageBytes);
		}
	}
}