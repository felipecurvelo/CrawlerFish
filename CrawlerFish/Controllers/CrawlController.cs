using CrawlerFish.Interfaces;
using CrawlerFish.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrawlerFish.Controllers
{
    public class CrawlController : ApiController
    {
		[Dependency]
		public ICrawlerService CrawlerService { get; set; }

		[HttpGet]
		public HttpResponseMessage Get() {
			return Request.CreateResponse(HttpStatusCode.OK, "Hi, I'm CrawlerFish!");
		}

		/// <summary>
		/// Crawls a website and generate a site map, including links and assets of each page.
		/// </summary>
		[HttpPost]
		public HttpResponseMessage Crawl(string url, int depth) {
			var siteMap = CrawlerService.CrawlWebSite(url, depth);
			return Request.CreateResponse(HttpStatusCode.OK, siteMap);
		}
	}
}
