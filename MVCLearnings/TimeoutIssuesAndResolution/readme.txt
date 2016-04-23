http://www.c-sharpcorner.com/UploadFile/baimey/garbage-collection-in-net-4-5/
http://stackoverflow.com/questions/8602395/timeout-expired-the-timeout-period-elapsed-prior-to-completion-of-the-operation
https://support.microsoft.com/en-in/kb/2605597


** check all these to resolve
a. IIS timeout
b. connection string "connect timeout". set to 200
c. min pool size. set to 20. set max to 100. 100 is default. 
d. set command timeout to 0 (infinite) --- not recommended

to try from UI
a. In the web.config try adding the following to the httpRuntime element: maxRequestLength="157286400" executionTimeout="10800"
b. add key="DBConnection" value="server=LocalHost;uid=sa;pwd=;database=DataBaseName;Connect Timeout=200; pooling='true'; Max Pool Size=200"/>

*****tell DBA's to try this
a. exec sp_updatestats
 If that doesn't work you could also try
b. dbcc freeproccache
