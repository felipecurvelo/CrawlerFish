using CrawlerFish.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace CrawlerFish.Services {
	public class UrlFetcherService : IFetcherService {
		public List<string> ExtractAssets(string pageText) {
			return new List<string>();
		}

		/// <summary>
		/// Extract links from a html page
		/// </summary>
		public List<string> ExtractLinks(string pageText) {

			var linksReturnList = new List<string>();
			var document = new XmlDocument();
			document.LoadXml(pageText);

			var linkNodes = document.GetElementsByTagName("a");
			foreach (XmlNode n in linkNodes) {
				foreach (XmlAttribute a in n.Attributes) {
					if (a.Name == "href") {
						linksReturnList.Add(n.Attributes["href"].Value);
					}
				}
			}

			return linksReturnList;
		}

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