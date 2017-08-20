using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrawlerFish.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using CrawlerFish.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using CrawlerFish.Services;

namespace CrawlerFish.Tests {
	[TestClass]
	public class CrawlControllerTest {

		[TestMethod]
		public void TestMainAdressCall_ReturnHiMessage() {
			var controller = new CrawlController() { Request = new HttpRequestMessage() };
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

			var response = controller.Get();
			var actual = response.Content.ReadAsStringAsync().Result;

			Assert.AreEqual("\"Hi, I'm CrawlerFish!\"", actual);
		}

		[TestMethod]
		public void TestBasicPageCall_ReturnBasicPageMap() {

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
			var expected = JsonConvert.SerializeObject(map);

			var controller = new CrawlController() {
				Request = new HttpRequestMessage(),
				FetcherService = new UrlFetcherService() 
			};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
			var response = controller.Crawl("file:///C:/Projetos/CrawlerFish/CrawlerFishTests/Files/CrawlBasicTestPage.html");
			var actual = response.Content.ReadAsStringAsync().Result;

			Assert.AreEqual(expected, actual);
		}
	}
}
