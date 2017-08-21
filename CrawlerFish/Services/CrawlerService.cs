using CrawlerFish.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CF = CrawlerFish.Models;

namespace CrawlerFish.Services {
	public class CrawlerService : ICrawlerService {
		public CF.SiteMap CrawlWebSite(string url, int maxDepth, int actualDepth = 0, CF.SiteMap siteMap = null) {

			siteMap = siteMap ?? new CF.SiteMap();

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

			//TODO: Do the depth navigation
			//item.Links.ForEach(l => siteMap.Join(CrawlWebSite(l, maxDepth, actualDepth++, siteMap)));

			siteMap.Items.Add(item);

			return siteMap;
		}
	}
}