using CrawlerFish.Models;
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
		public void TestCrawlServiceUolDepth0_Return2Assets() {
			var service = new CrawlerService();
			var siteMap = service.CrawlWebSite("http://uol.com.br", 0);
			Assert.AreEqual(2, siteMap.Items.Count);
		}
	}
}
