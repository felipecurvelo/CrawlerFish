using CF = CrawlerFish.Models;

namespace CrawlerFish.Interfaces {
	public interface ICrawlerService {
		CF.SiteMap CrawlWebSite(CF.SiteMap siteMap, string url, int maxDepth, int actualDepth = 0);
	}
}