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
   
   
   
 ********************************** Other learnings
 Functionality is to download excel report which is created on Database server/ App server or any other server other than Web server (remote server).
 Web App is hosted on Web server. User has to download excel report via link from UI in the web app hosted on web server.
 
 Steps to do in IIS.
 1. Create Virtual directory as an application within the Application's website or VD.
 2. While creating application, use the "Pass-through authentication" & click on "Connect As" button.
 3. On the window, click "specific user" radiobutton & Set the FID's User Id and Password.
    (Note: create a new FID via market place request for the application).
 4. After providing the credentials, use "Test Settings" button, to test. "Authentication" & "Authorization" should pass.
 5. Finally click OK and ensure the folders from servers are visible under the virtual directory created for downloading file from remote server.
 6. After this most important part is to provide access to the FID on the remote server folder.
    note: 
    a. Do not security tab on folder to provide permissions. It won't work
    b. Share the folder & provide access to the FID on shared folder. provide full access as "CoOwner".
 7. After folder share, test the download functionality from UI Web APP and click on download link.
 8. Excel file should be available for download or should open from the Client machine.
 
