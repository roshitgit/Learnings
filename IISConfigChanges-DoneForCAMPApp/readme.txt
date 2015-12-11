1. Created new application pool.
2. configured app pool to run with Custom Identity.
3. increased queue length from 2000 to 5000.
4. modified application.config file on web server to enable json compression using appcmd.exe
5. at website level, used --- all below was done from web.config
  * output caching                          --- 
  * used http response headers              --- for setting web content expiration when required
  * used http redirect                      --- to show maintenance page in production
  * used windows auth under authentication  --- disabled anonymous auth and enabled win auth with kernel mode checked.
  * enabled static and dynamic compression
  
