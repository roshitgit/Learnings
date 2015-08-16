**  understand deadlocks in async --- must read
http://blog.stephencleary.com/2012/07/dont-block-on-async-code.html

** question
http://stackoverflow.com/questions/27367454/is-it-worth-it-to-make-web-api-async-method
Question=> 
Angular $http methods return a promise. Therefore by nature it's already an async call to the web api.. 
Now the question is in what circumstance you wish to make your web api method to be async/await???
Ans=>
If your method has asynchronous work to do, then it should be async.
As @l3arnon commented, async on the server and async on the client are completely different. 
Async on the client is all about remaining responsive to the end-user (not blocking the UI thread). 
Async on the server is all about scalability (not blocking thread pool threads).

** web api throttling
http://blog.maartenballiauw.be/post/2013/05/28/Throttling-ASPNET-Web-API-calls.aspx -- must implement

** web api exceptions
http://blogs.msdn.com/b/pfxteam/archive/2011/09/28/task-exception-handling-in-net-4-5.aspx ---good read
http://adamprescott.net/tag/asp-net-web-api/
https://gist.github.com/darrelmiller/8548166 --- good read and try it
http://odetocode.com/blogs/scott/archive/2013/04/08/webapi-tip-8-working-with-tasks.aspx -- good one
http://stackoverflow.com/questions/10732644/best-practice-to-return-errors-in-asp-net-web-api
http://stackoverflow.com/questions/13269267/how-to-translate-exceptions-from-tasks-returned-by-asynchronous-asp-net-web-api
http://dotnetcodr.com/2013/01/07/exception-handling-in-async-methods-in-net4-5-mvc4-with-c/
http://weblogs.asp.net/fredriknormen/asp-net-web-api-exception-handling --- try this
http://blog.tonysneed.com/2013/04/01/more-fun-async-asp-net-web-api/
http://www.brytheitguy.com/?p=10 --- vgood. try this

** wep api request transform and exception handling
http://www.tugberkugurlu.com/archive/dealing-with-asynchrony-inside-the-asp-net-web-api-http-message-handlers---vgood

** best practice
http://www.c-sharpcorner.com/UploadFile/dacca2/asynchronous-programming-in-C-Sharp-5-0-part-6-3-best-practices/
http://www.anujvarma.com/async-await-in-web-api-controller/
http://stackoverflow.com/questions/28588652/web-api-2-return-content-with-ihttpactionresult-for-non-ok-response

***** What should my actions be returning?
A. Typed Objects/Collections: Customer or IEnumerable<Order>
B. HttpResponseMessage
C. IHttpActionResult
http://iswwwup.com/t/a7403961037f/asp-net-webapi-action-return-types.html

** How and Why's of async and await
https://msdn.microsoft.com/en-us/magazine/Dn802603.aspx
https://msdn.microsoft.com/en-us/library/hh191443.aspx
https://msdn.microsoft.com/en-us/library/vstudio/hh191443(v=vs.110).aspx --- good read

** mvc async using httpclient, webclient
https://github.com/RickAndMSFT/Async-ASP.NET

*** How to Build ASP.NET Web Applications Using Async
https://www.youtube.com/watch?v=RqPwOEjudD4
http://blogs.msdn.com/b/dotnet/archive/2012/04/03/async-in-4-5-worth-the-await.aspx

** excellent articles to use LINQ operations on Task<dynamic>  ------- must read
https://wizardsofsmart.wordpress.com/2013/03/21/web-api-and-dynamic-data-access/
https://gist.github.com/panesofglass/5212462

**Web API async method with AngularJS
http://stackoverflow.com/questions/28238418/web-api-async-method-with-angularjs

**angular js async calls to MVC controller not async
http://stackoverflow.com/questions/22847334/angular-js-async-calls-to-mvc-controller-not-async

*Async Await and ASP.NET MVC
http://www.devcurry.com/2013/05/async-await-and-aspnet-mvc.html

*Why do We Need Async Action Methods in ASP.Net MVC4 Application
http://www.c-sharpcorner.com/UploadFile/dbd951/why-do-we-need-async-action-methods-in-Asp-Net-mvc4-applicat/
