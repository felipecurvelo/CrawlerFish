# CrawlerFish

Service to crawl a website and generate a site map, including links and assets of each page and its dependencies.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

Clone the repository, and run the nuget restore to install packages dependencies:

```
C:\%YourProjectWorkspace%\WebCrawler>nuget restore CrawlerFish.sln
```

### Building

Build the the complete solution

```
Build > Build Solution (Ctrl + Shift + B)
```

### Testing

Run the automated tests to check everything is working correctly:

```
Test > Windows > Test Explorer > Run All (The tests just show up after build)
```

## Running

Run the API server project with Visual Studio 2015 or superior:

```
Debug > Start Debugging (F5) or Debug > Start Debugging (Ctrl + F5)
```

The API will run on:

```
http://localhost:2222
```

Call API using [Postman](https://www.getpostman.com/):

```
GET http://localhost:2222/crawl?url=http://www.g1.com.br&depth=1&timeout=9000
```

```
GET http://localhost:2222/crawl?url=http://www.uol.com.br&depth=0&timeout=5000
```

### Parameters:

* **url** - The main url that will be crawled.

* **depth** - The depth of the crawling search (Default value = 0).

* **timeout** - The max duration of API's search in milisseconds (Default value = 10000).

### Return information

* **MainUrl** - The url of the request.

* **TotalTime** - Total time elapsed on all url calls during the request.

* **Items** - A collection of called url objects.

* **ParentUrl** - The parent url of the item's url.

* **Url** - The item's url.

* **ResponseTime** - Time elapsed on the  url call.

* **Links** - Url page's links collection.

* **Assets** - Url page's assets collection (css, js or images).

* **Error** - Any error that ocurred during the single call. (Null if it's ok)

## Authors

* **Felipe Souto Curvelo**

## License

This project is license free
