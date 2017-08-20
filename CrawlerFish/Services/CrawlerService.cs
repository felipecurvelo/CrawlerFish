using CrawlerFish.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CF = CrawlerFish.Models;

namespace CrawlerFish.Services {
	public class CrawlerService : ICrawlerService {
		public CF.SiteMap CrawlWebSite(CF.SiteMap siteMap, string url, int maxDepth, int actualDepth = 0) {

			if (actualDepth > maxDepth) {
				return siteMap;
			}

			var fetcher = new UrlFetcherService();
			var pageText = fetcher.RetrieveUrlAsPlainText(url);

			var item = new CF.SiteMapItem() {
				Url = url,
				Links = fetcher.ExtractLinks(pageText),
				Assets = fetcher.ExtractAssets(pageText)
			};

			item.Links.ForEach(l => CrawlWebSite(siteMap, l, maxDepth, actualDepth++));

			siteMap.Items.Add(item);

			return siteMap;
		}
	}
}