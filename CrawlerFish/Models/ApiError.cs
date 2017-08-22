using CrawlerFish.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace CrawlerFish.Models {
	public class ApiError {
		public HttpStatusCode Status { get; set; }
		public string Message { get; set; }

		public ApiError(ErrorCode errorCode) {
			Status = ErrorHelper.GetStatusCode(errorCode);
			Message = ErrorHelper.GetMessage(errorCode);
		}
	}
}