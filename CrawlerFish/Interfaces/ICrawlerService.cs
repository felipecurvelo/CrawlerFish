using CF = CrawlerFish.Models;

namespace CrawlerFish.Interfaces {
	public interface ICrawlerService {
		CF.SiteMap CrawlWebSite(string url, int maxDepth, int actualDepth = 0, CF.SiteMap siteMap = null);
	}
}