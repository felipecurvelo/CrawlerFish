using CrawlerFish.Exceptions;
using CrawlerFish.Helpers;
using CrawlerFish.Interfaces;
using System.Collections.Generic;
using System.Linq;
using CF = CrawlerFish.Models;

namespace CrawlerFish.Services {
	public class CrawlerService : ICrawlerService {

		/// <summary>
		/// Crawl a website, getting its assets and links to build a siteMap
		/// </summary>
		/// <param name="url">Url adress to crawl</param>
		/// <param name="maxDepth">Max depth of crawl iterations</param>
		/// <param name="actualDepth">Actual depth of crawling (Not necessary in the first call)</param>
		/// <param name="lastSiteMap">Last depth site map (Not necessary in the first call)</param>
		/// <returns>Site map object with links and assets</returns>
		public CF.SiteMap CrawlWebSite(string url, int maxDepth, int actualDepth = 0, CF.SiteMap lastSiteMap = null) {
			if (string.IsNullOrWhiteSpace(url)) {
				throw new ApiException(ErrorCode.EmptyUrl);
			}

			var normalizedUrl = LinkHelper.NormalizeUrl(url);

			lastSiteMap = lastSiteMap ?? new CF.SiteMap() { MainUrl = normalizedUrl };

			if (actualDepth > maxDepth) {
				return lastSiteMap;
			}

			var fetcher = new UrlFetcherService();
			var pageText = fetcher.RetrieveUrlAsPlainText(normalizedUrl);

			var item = new CF.SiteMapItem() {
				Url = LinkHelper.NormalizeUrl(normalizedUrl),
				Links = fetcher.ExtractLinks(pageText),
				Assets = fetcher.ExtractAssets(pageText)
			};

			lastSiteMap.Items.Add(item);

			var linksToNavigate = item.Links.Where(link => isValidUrl(link, lastSiteMap.MainUrl, lastSiteMap.Items)).ToList();
			linksToNavigate.ForEach(l => CrawlWebSite(l, maxDepth, actualDepth++, lastSiteMap));

			return lastSiteMap;
		}

		/// <summary>
		/// Check if url is to the main domain and it's not repeated
		/// </summary>
		/// <param name="linkUrl">Url from the link got in the website</param>
		/// <param name="mainUrl">The first called url</param>
		/// <param name="collectedItems">Items already collected by crawler</param>
		/// <returns></returns>
		private bool isValidUrl(string linkUrl, string mainUrl, List<CF.SiteMapItem> collectedItems) {
			if (!LinkHelper.UrlHasInvalidExtension(linkUrl)) {
				return false;
			}

			//If url was already collected
			if (collectedItems.Any(item => item.Url == linkUrl)) {
				return false;
			}

			var mainUrlHost = LinkHelper.GetUrlHost(mainUrl);
			var urlHost = LinkHelper.GetUrlHost(linkUrl);

			if (!urlHost.Equals(mainUrlHost) || linkUrl.Equals(mainUrl)) {
				return false;
			}
			
			return true;
		}
	}
}