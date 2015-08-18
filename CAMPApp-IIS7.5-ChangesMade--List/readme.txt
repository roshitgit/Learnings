** These are the list of changes made on server after migrating to .net 4.5 on win 2008 r2 server (IIS 7.5)

1. Modified "Request Filtering" on website to "4294967295" bytes
   => Click on website
   => Double click "Request Filtering" under "Features View"
   => Click on "Edit Feature Settings" on actions panel on the right
   => Modify "Maximum allowed content length (bytes)" to "4294967295" (4GB)
   

2. Modified Application pool "Queue Length" from default 1000 to 5000 after created a new .NET 4.5 Application
   => Click on Application Pool
   => Click on advanced settings on the actions panel
   => In the window, change "Queue Length" from default 1000 to 5000
   
3. Modified processorThreadMax.
appcmd.exe set config -section:system.webServer/asp /limits.processorThreadMax:"50" /commit:apphost
   Modified requestQueueMax
appcmd.exe set config -section:system.webServer/asp /limits.requestQueueMax:"5000" /commit:apphost

4. Modified and applied json compression using appcmd.exe which modified applicationhost.config file on server.
appcmd path=> %windir%\system32\inetsrv\
appcmd.exe set config -section:system.webServer/httpCompression /-"dynamicTypes.[mimeType='application/json']"
appcmd.exe set config -section:system.webServer/httpCompression /+"dynamicTypes.[mimeType='application/json',enabled='True']"



