http://www.asp.net/mvc/overview/performance/using-asynchronous-methods-in-aspnet-mvc-4
http://stackoverflow.com/questions/23927343/changing-my-server-side-webapi-to-get-the-benefits-of-async-await

http://www.journeyofcode.com/will-block-debunking-asyncawait-pitfalls/ -- good read to know about async programming

************************The performance problems that async/await solve

The async/await feature solves three performance or scalability problems:

They can make your application handle more users. 
Most multi-user applications, such as web sites, use a thread for each user session. There are a lot of threads in the thread pool, but if lot of users try to access the site then they can run out, which in turn leads to blocking. Also there is a memory cost for each thread started, see Using Asynchronous Methods in ASP.NET MVC 4 
If you have requests that access an external resource such as a database or a web API then async  frees up the thread while it is waiting. This means you will use fewer threads, and so avoid reaching the maximum number of threads so quickly and use less memory as well.
You can process multiple I/O bound methods in parallel
If, for example, you needed to access a number of remote services either in legacy systems or over the web and then combine their results to show the user. If this were done synchronously, the process would take the sum of the access times for each service. With the new async/await feature you can easily run them all in parallel and then it takes only as long at the longest access-time. See an example at Performing Multiple Operations in Parallel .
You can make your interface more responsive to the user.
If you have a request on a site that is slow you can make the site more responsive by starting the action asynchronously and returning to the user before the action has finished. There are some issues around handling errors if the asynchronous task fails, but for places where your application interfaces to an external system that is slow this can be worth the effort.

*****************Processing Asynchronous Requests

In web applications that sees a large number of concurrent requests at start-up or has a bursty load 
(where concurrency increases suddenly), making these web service calls asynchronous will increase the responsiveness 
of your application. An asynchronous request takes the same amount of time to process as a synchronous request. For example, if a request makes a web service call that requires two seconds to complete, the request takes two seconds whether it is performed synchronously or asynchronously. However, during an asynchronous call, a thread is not blocked  from responding to other requests while it waits for the first request to complete. Therefore, asynchronous requests prevent request queuing and thread pool growth when there are many concurrent requests that invoke long-running operations.

*******************Choosing Synchronous or Asynchronous Action Methods

This section lists guidelines for when to use synchronous or asynchronous action methods. These are just guidelines;  examine each application individually to determine whether asynchronous methods help with performance.

In general, use synchronous methods for the following conditions:

The operations are simple or short-running.
Simplicity is more important than efficiency.
The operations are primarily CPU operations instead of operations that involve extensive disk or network overhead. Using asynchronous action methods on CPU-bound operations provides no benefits and results in more overhead.
In general, use asynchronous methods for the following conditions:
You're calling services that can be consumed through asynchronous methods, and you're using .NET 4.5 or higher.
The operations are network-bound or I/O-bound instead of CPU-bound.
Parallelism is more important than simplicity of code.
You want to provide a mechanism that lets users cancel a long-running request.
When the benefit of switching threads out weights the cost of the context switch. In general, you should make a method asynchronous if the synchronous method waits on the ASP.NET request thread while doing no work.  By making the call asynchronous,  the ASP.NET request thread is not stalled doing no work while it waits for the web service request to complete.
Testing shows that the blocking operations are a bottleneck in site performance and that IIS can service more requests by using asynchronous methods for these blocking calls.
