using System.Collections.Generic;
using CF = CrawlerFish.Models;

namespace CrawlerFish.Interfaces {
	public interface ICrawlerService {
		CF.SiteMap CrawlWebSite(string url, int maxDepth);
	}
}