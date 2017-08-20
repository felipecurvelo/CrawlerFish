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
		/// <summary>
		/// Extract assets from a html page
		/// </summary>
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
			var linkNodes = document.GetElementsByTagName("link");
			return extractAssetFromNodelist(linkNodes, "href", ".css");
		}

		private List<string> extractAssetFromNodelist(XmlNodeList list, string assetTagName, string partOfAssetValue = "") {
			var cssReturnList = new List<string>();
			foreach (XmlNode node in list) {
				var attributeList = node.Attributes.GetAttributesDictionary();
				if (attributeList.Any(a => a.Name == assetTagName && (a.Value.Contains(partOfAssetValue)))) {
					cssReturnList.Add(attributeList.FirstOrDefault(a => a.Name == assetTagName).Value);
				}
			}
			return cssReturnList;
		}

		private List<string> extractJs(XmlDocument document) {
			var linkNodes = document.GetElementsByTagName("script");
			return extractAssetFromNodelist(linkNodes, "src", ".js");
		}

		private List<string> extractImages(XmlDocument document) {
			var linkNodes = document.GetElementsByTagName("img");
			return extractAssetFromNodelist(linkNodes, "src");
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