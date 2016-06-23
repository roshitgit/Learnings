http://stackoverflow.com/questions/15855777/how-to-use-appcmd-to-test-and-see-if-a-website-exists-in-iis7-using-the-sites-n
https://forums.iis.net/t/1176690.aspx

** full appcmd ref
https://blogs.msdn.microsoft.com/mikezh/2012/04/23/iis-appcmd-quick-reference/

***** list of post-build event macros
http://miteshsureja.blogspot.in/2012/04/how-to-use-pre-build-and-post-build.html
http://jeremybytes.blogspot.in/2014/02/using-build-events-in-visual-studio-to.html

**** using robocopy
http://blogs.interknowlogy.com/2011/12/01/copying-files-after-a-successful-build-with-robocopy-on-vs2010/

** permissions
http://stackoverflow.com/questions/5615296/cannot-read-configuration-file-due-to-insufficient-permissions

** using batch file 
http://blogs.iis.net/ksingla/things-you-can-do-by-piping-appcmd-commands
http://www.alexandervanwynsberghe.be/automatic-iis7-deployment-using-msdeploy-and-appcmd/
http://stackoverflow.com/questions/12890239/creating-an-application-pool-in-iis-through-bat-file
http://robinosborne.co.uk/2011/10/10/batch-file-to-create-an-iis7-website/

http://www.benramey.com/2010/06/29/visual-studio-post-build-event-to-copy-dlls/

https://mohamedradwan.wordpress.com/2014/06/22/how-to-call-a-batch-file-from-tfs2013-build/

http://www.hanselman.com/blog/ManagingMultipleConfigurationFileEnvironmentsWithPreBuildEvents.aspx





You can use this in a batch file to report anomalies, as follows:

if errorlevel 16 echo ***FATAL ERROR*** & goto end
if errorlevel 15 echo OKCOPY + FAIL + MISMATCHES + XTRA & goto end
if errorlevel 14 echo FAIL + MISMATCHES + XTRA & goto end
if errorlevel 13 echo OKCOPY + FAIL + MISMATCHES & goto end
if errorlevel 12 echo FAIL + MISMATCHES& goto end
if errorlevel 11 echo OKCOPY + FAIL + XTRA & goto end
if errorlevel 10 echo FAIL + XTRA & goto end
if errorlevel 9 echo OKCOPY + FAIL & goto end
if errorlevel 8 echo FAIL & goto end
if errorlevel 7 echo OKCOPY + MISMATCHES + XTRA & goto end
if errorlevel 6 echo MISMATCHES + XTRA & goto end
if errorlevel 5 echo OKCOPY + MISMATCHES & goto end
if errorlevel 4 echo MISMATCHES & goto end
if errorlevel 3 echo OKCOPY + XTRA & goto end
if errorlevel 2 echo XTRA & goto end
if errorlevel 1 echo OKCOPY & goto end
if errorlevel 0 echo No Change & goto end
:end
