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
		/// <param name="currentDepth">Actual depth of crawling (Not necessary in the first call)</param>
		/// <param name="lastSiteMap">Last depth site map (Not necessary in the first call)</param>
		/// <returns>Site map object with links and assets</returns>
		public CF.SiteMap CrawlWebSite(string url, int maxDepth, int currentDepth = 0,
			CF.SiteMap lastSiteMap = null, List<string> navigatedLinks = null, string parentUrl = null, string mainUrl = null) {

			if (string.IsNullOrWhiteSpace(url)) {
				throw new ApiException(ErrorCode.EmptyUrl);
			}

			var currentUrl = url;
			if (currentDepth == 0) {
				currentUrl = LinkHelper.NormalizeUrl(url);
				mainUrl = currentUrl;
			}

			lastSiteMap = lastSiteMap ?? new CF.SiteMap() { MainUrl = currentUrl };

			navigatedLinks = navigatedLinks ?? new List<string>();
			var alreadyNavigatedUrl = navigatedLinks.Any(l => l == currentUrl);

			if (alreadyNavigatedUrl) {
				return lastSiteMap;
			}

			var fetcher = new UrlFetcherService();
			CF.ApiError error;
			var pageText = fetcher.RetrieveUrlAsPlainText(currentUrl, out error);
			navigatedLinks.Add(currentUrl);

			var item = new CF.SiteMapItem() {
				ParentUrl = parentUrl,
				Url = currentUrl,
				Links = fetcher.ExtractLinks(pageText, mainUrl),
				Assets = fetcher.ExtractAssets(pageText),
				Error = error
			};

			lastSiteMap.Items.Add(item);

			var nextDepth = currentDepth + 1;
			var linksToNavigate = new List<string>();
			if (item.Links != null && nextDepth <= maxDepth) {
				linksToNavigate = item.Links.Where(link => mustNavigateToUrl(link, lastSiteMap.MainUrl)).ToList();
				linksToNavigate.ForEach(l => CrawlWebSite(l, maxDepth, nextDepth, lastSiteMap, navigatedLinks, currentUrl, mainUrl));
			}

			return lastSiteMap;
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