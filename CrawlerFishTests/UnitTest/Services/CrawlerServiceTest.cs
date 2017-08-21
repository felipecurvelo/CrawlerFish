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
		public void TestCrawlUolDepth0_ReturnNAssets() {
			var service = new CrawlerService();
			var siteMap = service.CrawlWebSite("http://uol.com.br", 0);
			Assert.IsTrue(siteMap.Items.Count > 0);
		}
	}
}
