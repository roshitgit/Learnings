** difference between async await and TPL
A) The Task Parallel Library was designed for parallel programming - when you have a lot of work to do and want to split up that work among multiple threads so you can use all the CPU cores. TPL is best suited for CPU-intensive work.

B) Async and await are for asynchronous programming - when you have an operation (or many operations) that will complete in the future, and you want to do other things in the meantime. Async is best suited for I/O-bound work.

There is some overlap. For example, you can treat a parallel computation as an asynchronous operation so it doesn't tie up your UI thread. Also, both the TPL and async/await make use of the Task type, though they use it in very different ways.

note: 
Asynchronous : Responsiveness.
TPL (parallel programming) : better Performance with reduction in execution time


*** good read
https://msdn.microsoft.com/en-us/library/hh211418(v=vs.110).aspx
https://github.com/jskeet/DemoCode/tree/master/AsyncIntro  
http://www.tagwith.com/question_4026503_some-questions-concerning-the-combination-of-ado-net-dapper-queryasync-and-glim


** async await best practices
https://msdn.microsoft.com/en-us/magazine/jj991977.aspx

** bad effects of async await
http://www.tugberkugurlu.com/archive/asynchronousnet-client-libraries-for-your-http-api-and-awareness-of-async-await-s-bad-effects

**Build Async Services with ASP.NET Web API and Entity Framework 6   (must read and implement)
http://blog.tonysneed.com/2013/03/22/async-services-asp-net-web-api-entity-framework-6/

** why to async controllers and async methods in MVC controllers   (must read and implement)
http://tech.pro/tutorial/1252/asynchronous-controllers-in-asp-net-mvc

http://www.c-sharpcorner.com/UploadFile/dbd951/why-do-we-need-async-action-methods-in-Asp-Net-mvc4-applicat/

http://www.asp.net/mvc/overview/performance/using-asynchronous-methods-in-aspnet-mvc-4 -- vgood must read

**Asynchronous Programming in Web API /ASP.NET MVC ( must read)
http://www.codeproject.com/Tips/805923/Asynchronous-programming-in-Web-API-ASP-NET-MVC

** github
https://github.com/SteveSanderson/ASP.NET-MVC-async-demos --- vgood
https://github.com/Tameshiwari/AsyncAwaitMVC_WCF
https://github.com/tenzinkabsang/MvcAsyncAwait
https://github.com/ashafir20/MVC_4_Fundamentals_AsyncAwait
https://github.com/trofimchuk-t/async-testing
