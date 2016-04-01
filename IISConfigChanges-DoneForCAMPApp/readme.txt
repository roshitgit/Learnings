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
