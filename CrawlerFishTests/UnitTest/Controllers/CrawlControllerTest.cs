﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrawlerFish.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using CrawlerFish.Models;
using Newtonsoft.Json;
using CrawlerFish.Services;
using System.Net;

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
		public void TestCrawlControllerUolDepth0Timeout2000ms_Return1Asset() {
			var controller = new CrawlController() {
				Request = new HttpRequestMessage(),
				CrawlerService = new CrawlerService() 
			};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
			var response = controller.Crawl("http://www.uol.com.br", 0, 2000);
			var actual = (SiteMap)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(SiteMap));

			Assert.AreEqual(1, actual.Items.Count);
		}

		[TestMethod]
		public void TestCrawlControllerUolDepth0Timeout2000ms_FirstAssetReturnMoreThan100Links() {
			var controller = new CrawlController() {
				Request = new HttpRequestMessage(),
				CrawlerService = new CrawlerService()
			};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
			var response = controller.Crawl("http://www.uol.com.br", 0, 2000);
			var actual = (SiteMap)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(SiteMap));

			Assert.IsTrue(actual.Items[0].Links.Count > 100);
		}

		[TestMethod]
		public void TestCrawlControllerUolDepth0Timeout2000ms_FirstAssetReturnMoreThan50Assets() {
			var controller = new CrawlController() {
				Request = new HttpRequestMessage(),
				CrawlerService = new CrawlerService()
			};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
			var response = controller.Crawl("www.uol.com.br", 0, 2000);
			var actual = (SiteMap)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(SiteMap));

			Assert.IsTrue(actual.Items[0].Assets.Count > 50);
		}

		[TestMethod]
		public void TestCrawlControllerErrorWebsite_BadRequest() {
			var controller = new CrawlController() {
				Request = new HttpRequestMessage(),
				CrawlerService = new CrawlerService()
			};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
			var response = controller.Crawl("http://www.detran.sp.gov.br", 0, 2000);
			var actual = (SiteMap)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(SiteMap));

			Assert.AreEqual(HttpStatusCode.BadRequest, actual.Items[0].Error.Status);
		}

		[TestMethod]
		public void TestCrawlControllerEmptyUrl_NotAcceptable() {
			var controller = new CrawlController() {
				Request = new HttpRequestMessage(),
				CrawlerService = new CrawlerService()
			};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
			var response = controller.Crawl("", 0, 2000);
			var actual = (SiteMap)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(SiteMap));

			Assert.AreEqual(HttpStatusCode.NotAcceptable, response.StatusCode);
		}

		[TestMethod]
		public void TestCrawlControllerTimeout10ms_RequestTimeout() {
			var controller = new CrawlController() {
				Request = new HttpRequestMessage(),
				CrawlerService = new CrawlerService()
			};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
			var response = controller.Crawl("www.uol.com.br", 0, 5);
			var actual = (SiteMap)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(SiteMap));

			Assert.AreEqual(HttpStatusCode.RequestTimeout, response.StatusCode);
		}
	}
}
