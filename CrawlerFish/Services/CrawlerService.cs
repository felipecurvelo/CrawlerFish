using CrawlerFish.Exceptions;
using CrawlerFish.Helpers;
using CrawlerFish.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CF = CrawlerFish.Models;

namespace CrawlerFish.Services {
	public class CrawlerService : ICrawlerService {

		object _threadLock = new object();

		private CF.SiteMap _siteMap = new CF.SiteMap();
		private CF.SiteMap siteMap {
			get {
				lock (_threadLock) {
					return _siteMap;
				}
			}
			set {
				lock (_threadLock) {
					_siteMap = value;
				}
			}
		}

		private List<string> _navigatedLinks = new List<string>();
		private List<string> navigatedLinks {
			get {
				lock (_threadLock) {
					return _navigatedLinks;
				}
			}
			set {
				lock (_threadLock) {
					_navigatedLinks = value;
				}
			}
		}

		public CF.SiteMap CrawlWebSite(string url, int maxDepth) {
			if (string.IsNullOrWhiteSpace(url)) {
				throw new ApiException(ErrorCode.EmptyUrl);
			}

			var normalizedUrl = LinkHelper.NormalizeUrl(url);
			siteMap.MainUrl = normalizedUrl;

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			CrawlWebSite(normalizedUrl, maxDepth, 0, normalizedUrl, normalizedUrl);
			stopwatch.Stop();

			siteMap.TotalTime = stopwatch.Elapsed.TotalSeconds.ToString("0.000");

			return siteMap;
		}

		/// <summary>
		/// Crawl a website, getting its assets and links to build a siteMap
		/// </summary>
		/// <param name="url">Url adress to crawl</param>
		/// <param name="maxDepth">Max depth of crawl iterations</param>
		/// <param name="currentDepth">Actual depth of crawling (Not necessary in the first call)</param>
		/// <param name="lastSiteMap">Last depth site map (Not necessary in the first call)</param>
		/// <returns>Site map object with links and assets</returns>
		private void CrawlWebSite(string url, int maxDepth, int currentDepth, string parentUrl, string mainUrl) {

			var nextDepth = currentDepth + 1;

			var alreadyNavigatedUrl = navigatedLinks.ToList().Any(l => l == url);
			if (alreadyNavigatedUrl) {
				return;
			}
			navigatedLinks.Add(url);

			var item = extractUrlInformation(url, parentUrl, mainUrl);
			siteMap.Items.Add(item);

			if (item.Links != null && nextDepth <= maxDepth) {
				var linksToNavigate = item.Links.Distinct().Where(link => mustNavigateToUrl(link, siteMap.MainUrl)).ToList();
				Parallel.ForEach(linksToNavigate, (l) => {
					CrawlWebSite(l, maxDepth, nextDepth, url, mainUrl);
				});
			}
		}

		private CF.SiteMapItem extractUrlInformation(string currentUrl, string parentUrl, string mainUrl) {
			var fetcher = new UrlFetcherService();
			CF.ApiError error;

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			var pageText = fetcher.RetrieveUrlAsPlainText(currentUrl, out error);
			stopwatch.Stop();

			var item = new CF.SiteMapItem() {
				ParentUrl = parentUrl,
				Url = currentUrl,
				ResponseTime = stopwatch.Elapsed.TotalSeconds.ToString("0.000"),
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