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
    }
}
