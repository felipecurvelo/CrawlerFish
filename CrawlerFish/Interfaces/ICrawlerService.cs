using System.Collections.Generic;
using CF = CrawlerFish.Models;

namespace CrawlerFish.Interfaces {
	public interface ICrawlerService {
		CF.SiteMap CrawlWebSite(string url, int maxDepth, int currentDepth = 0,
			CF.SiteMap lastSiteMap = null, List<string> navigatedLinks = null, string parentUrl = null, string mainUrl = null);
	}
}