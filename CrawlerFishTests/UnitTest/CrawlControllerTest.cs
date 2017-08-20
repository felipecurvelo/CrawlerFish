using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrawlerFish.Controllers;
using System.Net;
using Moq;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;

namespace CrawlerFish.Tests {
	[TestClass]
	public class CrawlControllerTest {
		[TestMethod]
		public void TestMainAdressCall_ReturnHiMessage() {
			var controller = new CrawlController() { Request = new HttpRequestMessage()	};
			controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

			var response = controller.Get();

			string actual = null;
			response.TryGetContentValue(out actual);

			Assert.AreEqual("Hi, I'm CrawlerFish!", actual);
		}
	}
}
