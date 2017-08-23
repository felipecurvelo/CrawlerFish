using System.Collections.Generic;
using CF = CrawlerFish.Models;

namespace CrawlerFish.Interfaces {
	public interface ICrawlerService {
		CF.SiteMap CrawlWebSite(string url, int maxDepth);

		CF.SiteMap CrawlWebSite(string url, int maxDepth, int currentDepth,
			CF.SiteMap lastSiteMap, List<string> navigatedLinks, string parentUrl, string mainUrl);
	}
}