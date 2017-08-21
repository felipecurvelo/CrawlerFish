using CrawlerFish.Exceptions;
using CrawlerFish.Helpers;
using CrawlerFish.Interfaces;
using Microsoft.Practices.Unity;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrawlerFish.Controllers
{
    public class CrawlController : ApiController
    {
		[Dependency]
		public ICrawlerService CrawlerService { get; set; }

		/// <summary>
		/// Simple get to test server
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public HttpResponseMessage Get() {
			return Request.CreateResponse(HttpStatusCode.OK, "Hi, I'm CrawlerFish!");
		}

		/// <summary>
		/// Crawls a website and generate a site map, including links and assets of each page.
		/// </summary>
		[HttpPost]
		public HttpResponseMessage Crawl(string url, int depth) {
			try {
				var siteMap = CrawlerService.CrawlWebSite(url, depth);
				return Request.CreateResponse(HttpStatusCode.OK, siteMap);
			} 
			catch (ApiException apiException) {
				var error = ErrorHelper.CreateError(apiException.ErrorCode);
				return Request.CreateResponse(error.Status, error);
			}
			catch {
				var error = ErrorHelper.CreateError(ErrorCode.UnhandledError);
				return Request.CreateResponse(error.Status, error);
			}
		}
	}
}
