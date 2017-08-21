using CrawlerFish.Helpers;
using CrawlerFish.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CF = CrawlerFish.Models;

namespace CrawlerFish.Services {
	public class CrawlerService : ICrawlerService {
		public CF.SiteMap CrawlWebSite(string url, int maxDepth, int actualDepth = 0, CF.SiteMap siteMap = null) {

			var normalizedUrl = LinkHelper.NormalizeUrl(url);

			siteMap = siteMap ?? new CF.SiteMap() { MainUrl = normalizedUrl };

			if (actualDepth > maxDepth) {
				return siteMap;
			}

			var fetcher = new UrlFetcherService();
			var pageText = fetcher.RetrieveUrlAsPlainText(normalizedUrl);

			var item = new CF.SiteMapItem() {
				Url = LinkHelper.NormalizeUrl(normalizedUrl),
				Links = fetcher.ExtractLinks(pageText),
				Assets = fetcher.ExtractAssets(pageText)
			};

			siteMap.Items.Add(item);

			var linksToNavigate = item.Links.Where(link => isValidUrl(link, siteMap.MainUrl, siteMap.Items)).ToList();
			linksToNavigate.ForEach(l => CrawlWebSite(l, maxDepth, actualDepth++, siteMap));

			return siteMap;
		}

		private bool isValidUrl(string linkUrl, string mainUrl, List<CF.SiteMapItem> collectedItems) {
			if (!LinkHelper.IsUrlValidToNavigate(linkUrl)) {
				return false;
			}

			//If url was already collected
			if (collectedItems.Any(item => item.Url == linkUrl)) {
				return false;
			}

			var mainUrlNormalized = LinkHelper.NormalizeUrl(mainUrl);
			var linkUrlNormalized = LinkHelper.NormalizeUrl(linkUrl);

			var mainUrlHost = LinkHelper.GetUrlHost(mainUrlNormalized);
			var urlHost = LinkHelper.GetUrlHost(linkUrlNormalized);

			if (!urlHost.Equals(mainUrlHost) || linkUrlNormalized.Equals(mainUrlNormalized)) {
				return false;
			}
			
			return true;
		}
	}
}