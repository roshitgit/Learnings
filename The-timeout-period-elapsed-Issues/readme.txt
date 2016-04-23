https://lowleveldesign.wordpress.com/2012/09/07/diagnosing-ado-net-with-etw-traces/
https://github.com/HangfireIO/Hangfire/issues/273
http://www.smarterasp.net/support/kb/a1540/the-timeout-period-elapsed-prior-to-obtaining-a-connection-from-the-pool_.aspx
http://www.codeproject.com/Questions/423588/The-timeout-period-elapsed-prior-to-obtaining-a-co
http://www.codeguru.com/csharp/.net/net_asp/article.php/c19395/Tuning-Up-ADONET-Connection-Pooling-in-ASPNET-Applications.htm
http://www.c-sharpcorner.com/UploadFile/baimey/garbage-collection-in-net-4-5/
http://stackoverflow.com/questions/8602395/timeout-expired-the-timeout-period-elapsed-prior-to-completion-of-the-operation
https://support.microsoft.com/en-in/kb/2605597

******* perfect way to implement idisposable
https://msdn.microsoft.com/en-us/library/ff647790.aspx

http://www.codeproject.com/Articles/63147/Handling-database-connections-more-easily

** vgood- must learn
http://blogs.msdn.com/b/spike/archive/2008/07/31/timeout-expired-the-timeout-period-elapsed-prior-to-completion-of-the-operation-or-the-server-is-not-responding.aspx

*************** check this test and figure out ways to test connection pooling and timeouts ------ pretty good
https://jamessdixon.wordpress.com/2013/01/22/ado-net-and-connection-pooling/

Try out these changes on IIS to eliminate these issues:
http://securitywing.com/8-effective-ways-to-improve-iis-7-5-performance/
https://msdn.microsoft.com/en-US/library/Ee377050(v=BTS.70).aspx

http://blogs.msdn.com/b/tmarq/archive/2007/07/21/asp-net-thread-usage-on-iis-7-0-and-6-0.aspx
1. <system.web>
        <applicationPool maxConcurrentRequestsPerCPU="5000" maxConcurrentThreadsPerCPU="0" requestQueueLimit="5000"/>
 </system.web>
 using appcmd.exe
appcmd.exe set config -section:system.webServer/asp /limits.scriptTimeout:"00:02:00" /commit:apphost
appcmd.exe set config -section:system.webServer/asp /limits.queueConnectionTestTime:"00:00:05" /commit:apphost
appcmd.exe set config -section:system.webServer/asp /limits.requestQueueMax:"1000" /commit:apphost
 
 http://www.iis.net/configreference/system.webserver/asp/limits
 https://www.iis.net/configreference/system.applicationhost/sites/site/limits
2. <system.webServer>
      <asp>
         <cache diskTemplateCacheDirectory="%SystemDrive%\inetpub\temp\ASP Compiled Templates" />
         <limits scriptTimeout="00:02:00"
            queueConnectionTestTime="00:00:05"
            requestQueueMax="1000" />
      </asp>
   <system.webServer>
   using APPCMD
a. appcmd.exe set config -section:system.applicationHost/sites "/[name='Default Web Site'].limits.maxBandwidth:65536" /commit:apphost
b. appcmd.exe set config -section:system.applicationHost/sites "/[name='Default Web Site'].limits.maxConnections:1024" /commit:apphost
c. appcmd.exe set config -section:system.applicationHost/sites "/[name='Default Web Site'].limits.connectionTimeout:00:01:00" /commit:apphost
