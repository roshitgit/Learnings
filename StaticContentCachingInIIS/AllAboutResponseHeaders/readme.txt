https://developers.google.com/web/fundamentals/performance/optimizing-content-efficiency/http-caching#caching-checklist
https://developers.google.com/web/fundamentals/performance/optimizing-content-efficiency/http-caching#defining-optimal-cache-control-policy
https://developers.google.com/web/fundamentals/performance/optimizing-content-efficiency/http-caching#invalidating-and-updating-cached-responses
https://developers.google.com/web/fundamentals/performance/optimizing-content-efficiency/http-caching#validating-cached-responses-with-etags
https://developers.google.com/web/fundamentals/performance/optimizing-content-efficiency/http-caching#cache-control


*** cache setup and configuration on web.config  (((very very good)))
https://github.com/h5bp/server-configs-iis
https://www.owasp.org/index.php/List_of_useful_HTTP_headers


*** fiddler response to confirm file is not cached
HTTP/1.1 200 OK
Access-Control-Allow-Origin: *
Date: Wed, 16 Dec 2015 04:46:01 GMT
Pragma: no-cache
Expires: Mon, 01 Jan 1990 00:00:00 GMT
Last-Modified: Sun, 17 May 1998 03:00:00 GMT
X-Content-Type-Options: nosniff       (optional)
Content-Type: image/gif
Content-Length: 35
Cache-Control: no-cache, no-store, must-revalidate
Age: 1239092
Proxy-Connection: Keep-Alive    (optional)
Connection: Keep-Alive                (optional)

