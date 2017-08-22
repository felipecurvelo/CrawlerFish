using CrawlerFish.Helpers;
using CrawlerFish.Interfaces;
using CrawlerFish.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CrawlerFish.Services {
	public class UrlFetcherService : IFetcherService {
		/// <summary>
		/// Extract links from a html page
		/// </summary>
		public List<string> ExtractLinks(string pageText, string mainUrl) {
			if (String.IsNullOrEmpty(pageText)) {
				return null;
			}

			var linksReturnList = new List<string>();
			var document = new HtmlDocument();
			document.LoadHtml(pageText);

			var linkNodes = document.DocumentNode.SelectNodes("//a");
			if (linkNodes != null) {
				foreach (HtmlNode node in linkNodes) {
					var attributes = node.Attributes.GetAttributesDictionary();
					var links = attributes.Where(a => a.Name == "href").ToList();
					links.ForEach(l => linksReturnList.Add(LinkHelper.NormalizeUrl(l.Value, mainUrl)));
				}
			}

			return linksReturnList;
		}

		/// <summary>
		/// Retrive a url client page as plain text 
		/// </summary>
		public string RetrieveUrlAsPlainText(string url, out ApiError error) {
			error = null;
			string returnData = null;

			if (String.IsNullOrEmpty(url)) {
				error = new ApiError(ErrorCode.EmptyUrl);
				return returnData;
			}

			WebClient client = new System.Net.WebClient();
			try {
				byte[] pageBytes = client.DownloadData(url);
				returnData = Encoding.UTF8.GetString(pageBytes);
			} catch (Exception e) {
				error = new ApiError(ErrorCode.CannotReachWebSite, e);
			}

			return returnData;
		}

		/// <summary>
		/// Extract assets from a html page
		/// </summary>
		public List<string> ExtractAssets(string pageText) {
			if (String.IsNullOrEmpty(pageText)) {
				return null;
			}

			var assetsReturnList = new List<string>();
			var document = new HtmlDocument();
			document.LoadHtml(pageText);

			assetsReturnList.AddRange(extractCss(document));
			assetsReturnList.AddRange(extractJs(document));
			assetsReturnList.AddRange(extractImages(document));

			return assetsReturnList;
		}

		/// <summary>
		/// Extracts document css elements 
		/// </summary>
		private List<string> extractCss(HtmlDocument document) {
			var linkNodes = document.DocumentNode.SelectNodes("//head/link");
			return extractAssetFromNodelist(linkNodes, "href", ".css");
		}

		/// <summary>
		/// Extracts document js elements 
		/// </summary>
		private List<string> extractJs(HtmlDocument document) {
			var headLinkNodes = document.DocumentNode.SelectNodes("//head/script");
			var bodyLinkNodes = document.DocumentNode.SelectNodes("//body/script");

			var assets = extractAssetFromNodelist(headLinkNodes, "src", ".js");
			assets.AddRange(extractAssetFromNodelist(bodyLinkNodes, "src", ".js"));

			return assets;
		}

		/// <summary>
		/// Extracts document image elements 
		/// </summary>
		private List<string> extractImages(HtmlDocument document) {
			var linkNodes = document.DocumentNode.SelectNodes("//img");
			return extractAssetFromNodelist(linkNodes, "src");
		}

		/// <summary>
		/// Extract assets from a html node list based on asset tag name and part of expected value like extension
		/// </summary>
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