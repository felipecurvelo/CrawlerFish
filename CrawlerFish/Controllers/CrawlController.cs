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
		public HttpResponseMessage Crawl(string url) {

			//var siteMap = CrawlerService.CrawlWebSite(url);

			var map = new SiteMap() {
				Items = new List<Models.SiteMapItem>() {
					new Models.SiteMapItem() {
						Url = "CrawlBasicTestPage.html",
						Assets = new List<string>() {
							"simplejs.js",
							"simplecss.css"
						},
						Links = new List<string>() {
							"simplelink.html"
						}
					}
				}
			};

			return Request.CreateResponse(HttpStatusCode.OK, map);
		}
	}
}
