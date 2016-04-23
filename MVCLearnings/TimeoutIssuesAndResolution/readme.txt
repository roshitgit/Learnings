http://www.c-sharpcorner.com/UploadFile/baimey/garbage-collection-in-net-4-5/
http://stackoverflow.com/questions/8602395/timeout-expired-the-timeout-period-elapsed-prior-to-completion-of-the-operation
https://support.microsoft.com/en-in/kb/2605597


** check all these to resolve
IIS timeout
connection string "connect timeout"
min pool size. set to 20. set max to
set command timeout to 0 (infinite)
In the web.config try adding the following to the httpRuntime element: maxRequestLength="157286400" executionTimeout="10800"
