https://lowleveldesign.wordpress.com/2012/09/07/diagnosing-ado-net-with-etw-traces/
https://github.com/HangfireIO/Hangfire/issues/273
http://www.smarterasp.net/support/kb/a1540/the-timeout-period-elapsed-prior-to-obtaining-a-connection-from-the-pool_.aspx
http://www.codeproject.com/Questions/423588/The-timeout-period-elapsed-prior-to-obtaining-a-co
http://www.codeguru.com/csharp/.net/net_asp/article.php/c19395/Tuning-Up-ADONET-Connection-Pooling-in-ASPNET-Applications.htm

Try out these changes on IIS to eliminate these issues:
http://securitywing.com/8-effective-ways-to-improve-iis-7-5-performance/
https://msdn.microsoft.com/en-US/library/Ee377050(v=BTS.70).aspx

http://blogs.msdn.com/b/tmarq/archive/2007/07/21/asp-net-thread-usage-on-iis-7-0-and-6-0.aspx
1. <system.web>
        <applicationPool maxConcurrentRequestsPerCPU="5000" maxConcurrentThreadsPerCPU="0" requestQueueLimit="5000"/>
 </system.web>
 
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
