using CrawlerFish.Helpers;
using System;
using System.Net;

namespace CrawlerFish.Models {
	public class ApiError {
		public HttpStatusCode Status { get; set; }
		public string Message { get; set; }
		public string InnerException { get; set; }

		public ApiError() {
		}

		public ApiError(ErrorCode errorCode) {
			Status = ErrorHelper.GetStatusCode(errorCode);
			Message = ErrorHelper.GetMessage(errorCode);
		}

		public ApiError(ErrorCode errorCode, Exception innerException) : this(errorCode) {
			InnerException = innerException.Message;
		}
	}
}