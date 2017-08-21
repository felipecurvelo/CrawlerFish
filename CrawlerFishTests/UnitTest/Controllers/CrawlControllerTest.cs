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
		public void TestCrawlControllerUolDepth0_ReturnTwoAssets() {
			var controller = new CrawlController() {
				Request = new HttpRequestMessage(),
				CrawlerService = new CrawlerService() 
			};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
			var response = controller.Crawl("www.uol.com.br");
			var actual = (SiteMap)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(SiteMap));

			Assert.AreEqual(2, actual.Items.Count);
		}

		[TestMethod]
		public void TestCrawlControllerUolDepth0_FirstAssetReturn398Links() {
			var controller = new CrawlController() {
				Request = new HttpRequestMessage(),
				CrawlerService = new CrawlerService()
			};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
			var response = controller.Crawl("www.uol.com.br");
			var actual = (SiteMap)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(SiteMap));

			Assert.AreEqual(398, actual.Items[0].Links.Count);
		}

		[TestMethod]
		public void TestCrawlControllerUolDepth0_FirstAssetReturnNAssets() {
			var controller = new CrawlController() {
				Request = new HttpRequestMessage(),
				CrawlerService = new CrawlerService()
			};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
			var response = controller.Crawl("www.uol.com.br");
			var actual = (SiteMap)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(SiteMap));

			Assert.AreEqual(141, actual.Items[0].Assets.Count);
		}
	}
}
