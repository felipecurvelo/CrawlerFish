using CrawlerFish.Interfaces;
using HtmlAgilityPack;
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
		/// Extract links from a html page
		/// </summary>
		public List<string> ExtractLinks(string pageText) {
			var linksReturnList = new List<string>();
			var document = new HtmlDocument();
			document.LoadHtml(pageText);

			var linkNodes = document.DocumentNode.SelectNodes("//a");
			foreach (HtmlNode node in linkNodes) {
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
			if (String.IsNullOrEmpty(url)) {
				throw new Exception("Empty Url");
			}

			WebClient client = new System.Net.WebClient();
			byte[] pageBytes = client.DownloadData(url);
			return Encoding.UTF8.GetString(pageBytes);
		}

		/// <summary>
		/// Extract assets from a html page
		/// </summary>
		public List<string> ExtractAssets(string pageText) {
			var assetsReturnList = new List<string>();
			var document = new HtmlDocument();
			document.LoadHtml(pageText);

			assetsReturnList.AddRange(extractCss(document));
			assetsReturnList.AddRange(extractJs(document));
			assetsReturnList.AddRange(extractImages(document));

			return assetsReturnList;
		}

		private List<string> extractCss(HtmlDocument document) {
			var linkNodes = document.DocumentNode.SelectNodes("//head/link");
			return extractAssetFromNodelist(linkNodes, "href", ".css");
		}

		private List<string> extractJs(HtmlDocument document) {
			var linkNodes = document.DocumentNode.SelectNodes("//head/script");
			return extractAssetFromNodelist(linkNodes, "src", ".js");
		}

		private List<string> extractImages(HtmlDocument document) {
			var linkNodes = document.DocumentNode.SelectNodes("//img");
			return extractAssetFromNodelist(linkNodes, "src");
		}

		private List<string> extractAssetFromNodelist(HtmlNodeCollection list, string assetTagName, string partOfAssetValue = "") {
			var cssReturnList = new List<string>();
			if (list != null) {
				foreach (HtmlNode node in list) {
					var attributeList = node.Attributes.GetAttributesDictionary();
					if (attributeList.Any(a => a.Name == assetTagName && (a.Value.Contains(partOfAssetValue)))) {
						cssReturnList.Add(attributeList.FirstOrDefault(a => a.Name == assetTagName).Value);
					}
				}
			}
			return cssReturnList;
		}
	}
}