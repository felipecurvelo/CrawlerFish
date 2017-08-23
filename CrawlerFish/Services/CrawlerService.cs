using CrawlerFish.Exceptions;
using CrawlerFish.Helpers;
using CrawlerFish.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CF = CrawlerFish.Models;

namespace CrawlerFish.Services {
	public class CrawlerService : ICrawlerService {

		public CF.SiteMap CrawlWebSite(string url, int maxDepth) {
			if (string.IsNullOrWhiteSpace(url)) {
				throw new ApiException(ErrorCode.EmptyUrl);
			}

			var normalizedUrl = LinkHelper.NormalizeUrl(url);
			var lastSiteMap = new CF.SiteMap() { MainUrl = normalizedUrl };
			var navigatedLinks = new List<string>();
			
			return CrawlWebSite(normalizedUrl, maxDepth, 0, lastSiteMap, navigatedLinks, normalizedUrl, normalizedUrl);
		}


		/// <summary>
		/// Crawl a website, getting its assets and links to build a siteMap
		/// </summary>
		/// <param name="url">Url adress to crawl</param>
		/// <param name="maxDepth">Max depth of crawl iterations</param>
		/// <param name="currentDepth">Actual depth of crawling (Not necessary in the first call)</param>
		/// <param name="lastSiteMap">Last depth site map (Not necessary in the first call)</param>
		/// <returns>Site map object with links and assets</returns>
		public CF.SiteMap CrawlWebSite(string url, int maxDepth, int currentDepth,
			CF.SiteMap lastSiteMap, List<string> navigatedLinks, string parentUrl, string mainUrl) {

			var nextDepth = currentDepth + 1;

			var alreadyNavigatedUrl = navigatedLinks.Any(l => l == url);
			if (alreadyNavigatedUrl) {
				return lastSiteMap;
			}
			navigatedLinks.Add(url);

			var item = extractUrlInformation(url, parentUrl, mainUrl);
			lastSiteMap.Items.Add(item);

			var linksToNavigate = new List<string>();
			if (item.Links != null && nextDepth <= maxDepth) {
				linksToNavigate = item.Links.Distinct().Where(link => mustNavigateToUrl(link, lastSiteMap.MainUrl)).ToList();
				linksToNavigate.ForEach(l => lastSiteMap = CrawlWebSite(l, maxDepth, nextDepth, lastSiteMap, navigatedLinks, url, mainUrl));
				
				//Parallel.ForEach(linksToNavigate, (l) => lastSiteMap = CrawlWebSite(l, maxDepth, nextDepth, lastSiteMap, navigatedLinks, url, mainUrl));
			}

			return lastSiteMap;
		}

		private CF.SiteMapItem extractUrlInformation(string currentUrl, string parentUrl, string mainUrl) {
			var fetcher = new UrlFetcherService();
			CF.ApiError error;
			var pageText = fetcher.RetrieveUrlAsPlainText(currentUrl, out error);
			var item = new CF.SiteMapItem() {
				ParentUrl = parentUrl,
				Url = currentUrl,
				Links = fetcher.ExtractLinks(pageText, mainUrl ?? currentUrl),
				Assets = fetcher.ExtractAssets(pageText),
				Error = error
			};
			return item;
		}



		/// <summary>
		/// Check if url is to the main domain and it's not repeated
		/// </summary>
		/// <param name="linkUrl">Url from the link got in the website</param>
		/// <param name="mainUrl">The first called url</param>
		/// <param name="collectedItems">Items already collected by crawler</param>
		/// <returns></returns>
		private bool mustNavigateToUrl(string linkUrl, string mainUrl) {
			var urlHasOneOfInvalidExtensions = LinkHelper.UrlHasOneOfInvalidExtensions(linkUrl);
			if (urlHasOneOfInvalidExtensions) {
				return false;
			}

			var mainUrlHost = LinkHelper.GetUrlHost(mainUrl);
			var urlHost = LinkHelper.GetUrlHost(linkUrl);
			var urlHasDifferentHostOrIsEqualsMainUrl = !urlHost.Equals(mainUrlHost) || linkUrl.Equals(mainUrl);
			if (urlHasDifferentHostOrIsEqualsMainUrl) {
				return false;
			}

			return true;
		}
	}
}