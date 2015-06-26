http://stackoverflow.com/questions/10949411/mvc4-webapi-not-compressing-get-responses
http://stackoverflow.com/questions/2138243/how-do-i-compress-a-json-result-from-asp-net-mvc-with-iis-7-5
http://benfoster.io/blog/aspnet-web-api-compression

** using nuget package
http://stackoverflow.com/questions/24815706/webapi-gzip-when-returning-httpresponsemessage

** In .NET 4.5 with MVC 4/5, use the nuget package below to compress JSON
https://github.com/azzlack/Microsoft.AspNet.WebApi.MessageHandlers.Compression

** In .NET 4.0 with MVC 4, use appcmd.exe to alter the applicationhost.config file to allow json compression
http://www.hanselman.com/blog/EnablingDynamicCompressionGzipDeflateForWCFDataFeedsODataAndOtherCustomServicesInIIS7.aspx
appcmd.exe set config -section:system.webServer/httpCompression /+"dynamicTypes.[mimeType='application/json',enabled='True']" /commit:apphost

Path on server => ApplicationHost.config. This file is located in %windir%\system32\inetsrv\config
