using CrawlerFish.Exceptions;
using CrawlerFish.Helpers;
using CrawlerFish.Interfaces;
using CrawlerFish.Models;
using Microsoft.Practices.Unity;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CrawlerFish.Controllers {
	public class CrawlController : ApiController {
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
		/// <param name="url">Url to crawl</param>
		/// <param name="depth">Depth of crawling (Starting in 0)</param>
		/// <param name="timeout">Timeout in miliseconds</param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage Crawl(string url, int depth, int timeout) {
			var task = Task.Run(() => {
				try {
					var siteMap = CrawlerService.CrawlWebSite(url, depth);
					return Request.CreateResponse(HttpStatusCode.OK, siteMap);
				} catch (ApiException apiException) {
					var error = ErrorHelper.CreateError(apiException.ErrorCode);
					return Request.CreateResponse(error.Status, error);
				} catch {
					var error = ErrorHelper.CreateError(ErrorCode.UnhandledError);
					return Request.CreateResponse(error.Status, error);
				}
			});

			TimeSpan ts = TimeSpan.FromMilliseconds(timeout);
			if (!task.Wait(ts)) {
				var error = new ApiError(ErrorCode.Timeout);
				return Request.CreateResponse(HttpStatusCode.RequestTimeout, error);
			} else {
				return task.Result;
			}
		}
	}
}
