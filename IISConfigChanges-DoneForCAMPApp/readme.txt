1. Created new application pool.
2. configured app pool to run with Custom Identity.
3. configured "recycling" on app pool to recycle at regular intervals
4. increased queue length from 2000 to 5000.
5. modified application.config file on web server to enable json compression using appcmd.exe
6. created new website and at website level, used --- all below was done from web.config
  * output caching                   --- enabled user-mode and kernel mode caching with "cacheuntilchange" (file change noti..)   policy
  * used http response headers              --- for setting web content expiration when required
  * used http redirect                      --- to show maintenance page in production
  * used windows auth under authentication  --- disabled anonymous auth and enabled win auth with kernel mode checked.
  * enabled static and dynamic compression
7. Applied DNS mapping of server on the website host headers

8. For uploading/ saving file on server, provide full access to IUSR on the folder to upload file
