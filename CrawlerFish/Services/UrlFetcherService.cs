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
			var assetsReturnList = new List<string>();
			var document = new XmlDocument();
			document.LoadXml(pageText);

			assetsReturnList.AddRange(extractCss(document));
			assetsReturnList.AddRange(extractJs(document));
			assetsReturnList.AddRange(extractImages(document));

			return assetsReturnList;
		}

		private List<string> extractCss(XmlDocument document) {
			var cssReturnList = new List<string>();

			var linkNodes = document.GetElementsByTagName("link");
			foreach (XmlNode node in linkNodes) {
				var attributeList = node.Attributes.GetAttributesDictionary();
				if (attributeList.Any(a => a.Name == "href") && attributeList.Any(a => a.Name == "type")) {
					cssReturnList.Add(attributeList.FirstOrDefault(a => a.Name == "href").Value);
				}
			}

			return cssReturnList;
		}

		private List<string> extractJs(XmlDocument document) {
			return new List<string>();
		}

		private List<string> extractImages(XmlDocument document) {
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
			foreach (XmlNode node in linkNodes) {
				var attributes = node.Attributes.GetAttributesDictionary();
				var links = attributes.Where(a => a.Name == "href").ToList();
				links.ForEach(l => linksReturnList.Add(l.Value));
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