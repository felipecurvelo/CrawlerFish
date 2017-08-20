using CrawlerFish.Models;
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
		[HttpGet]
		public HttpResponseMessage Get() {
			return Request.CreateResponse(HttpStatusCode.OK, "Hi, I'm CrawlerFish!");
		}

		[HttpGet]
		public HttpResponseMessage Crawl(string url) {

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
