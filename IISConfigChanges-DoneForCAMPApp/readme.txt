1. Created new application pool.
2. configured app pool to run with Custom Identity.
3. configured "recycling" on app pool to recycle at regular intervals
4. increased queue length from 2000 to 5000.
5. modified application.config file on web server to enable json compression using appcmd.exe

6. created new website and at website level, used --- all below was done from web.config
  * used http redirect                      --- to show maintenance page in production
  * used windows auth under authentication  --- disabled anonymous auth and enabled win auth with kernel mode checked.
  * enabled static and dynamic compression
  
7. Applied DNS mapping of server on the website host headers
8. For uploading/ saving file on server, provide full access to "IIS_IUSRS" group on the folder to upload file
9. Disabled browser caching on DEV & QA using cache buster. Enabled caching on UAT and PROD using "FileVersion" key in web.config.
10. Tried the below IOC containers in App.
   a. MS Unity
   b. Ninject
   c. Autofac -- best

** These are the list of changes made on server after migrating to .net 4.5 on win 2008 r2 server (IIS 7.5)

11. Modified "Request Filtering" on website to "4294967295" bytes
   => Click on website
   => Double click "Request Filtering" under "Features View"
   => Click on "Edit Feature Settings" on actions panel on the right
   => Modify "Maximum allowed content length (bytes)" to "4294967295" (4GB)
   
12. Modified processorThreadMax.
appcmd.exe set config -section:system.webServer/asp /limits.processorThreadMax:"50" /commit:apphost
   Modified requestQueueMax
appcmd.exe set config -section:system.webServer/asp /limits.requestQueueMax:"5000" /commit:apphost

13. Modified and applied json compression using appcmd.exe which modified applicationhost.config file on server.
appcmd path=> %windir%\system32\inetsrv\
appcmd.exe set config -section:system.webServer/httpCompression /-"dynamicTypes.[mimeType='application/json']"
appcmd.exe set config -section:system.webServer/httpCompression /+"dynamicTypes.[mimeType='application/json',enabled='True']"

   *******IIS optimizations to try
   https://msdn.microsoft.com/en-us/library/ee377050(v=bts.10).aspx
   http://webdebug.net/2015/03/iis-performance-optimization-guideline/
   http://docslide.us/documents/iis7-integrated-pipeline-mode-vs-classic-mode-v2-41.html
   http://quabr.com/28882774/iis-performance-issues-moving-from-7-to-8-5
