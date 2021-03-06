﻿using CrawlerFish.Models;
using CrawlerFish.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerFish.Tests.UnitTest.Services {
	[TestClass]
	public class CrawlerServiceTest {
		[TestMethod]
		public void TestCrawlServiceUolDepth0_Return1Asset() {
			var service = new CrawlerService();
			var siteMap = service.CrawlWebSite("http://www.uol.com.br", 0);
			Assert.AreEqual(1, siteMap.Items.Count);
		}

		[TestMethod]
		public void TestCrawlServiceUolDepth2_Return3Assets() {
			var service = new CrawlerService();
			var siteMap = service.CrawlWebSite("http://www.uol.com.br", 2);
			Assert.AreEqual(3, siteMap.Items.Count);
		}
	}
}
