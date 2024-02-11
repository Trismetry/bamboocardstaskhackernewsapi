Bamboo-Card Test Task HackerNews API

# BambooCardTestTask 
GET top n stories, the default stories count to take is 22, to change it please use
--header 'NumberToTake: 22'

-curl GET 'http://localhost:4300/hackernews'

The main idea to avoid overload is to use cache.
For default I cache the results but you can disable the cache setting the DisableCache header attribute to false

Sample request: GET /hackernews --header 'DisableCache: true' To change number of returned stories please use --header 'NumberToTake: n'

Enhancements:
-Swagger

-AutoMapper

-DisableCache
   curl GET 'http://localhost:4300/hackernews' --header 'DisableCache: true'
   
-CacheClean
   curl GET 'http://localhost:4300/clean'
   
-IntegrationTests

-UnitTests

