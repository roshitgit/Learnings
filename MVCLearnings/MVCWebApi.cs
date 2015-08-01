/*Use MVC Web API if pages load large datasets/json's. Large means data rows exceeding 5000 records in table.
 * Use Regular MVC controller only for smaller applications which is not data centric.
 * There is restriction to the size of JSON data that can be returned from MVC controller JSONResult Action method witin regular
    MVC controller.
 * Both MVC regular controller & Web API controller can be used within an app.*/


***************Dependency Injection *********************8
To use DI with VS2010 & MVC 4 Web API/Intranet Templates, download nuget package mentioned below
* Install-Package Unity.WebAPI -Version 0.10.0

***** Uploading and returning files in MVC
http://www.prideparrot.com/blog/archive/2012/8/uploading_and_returning_files

