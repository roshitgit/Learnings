*** excellent to understand idisposable
https://lostechies.com/chrispatterson/2012/11/29/idisposable-done-right/

http://www.codeproject.com/Articles/413887/Understanding-and-Implementing-IDisposable-Interfa

***** TEST and Implement Finalise and dispose (must try and implement)
https://msdn.microsoft.com/library/b1yfkh5e(v=vs.100).aspx

*** another way of using
http://www.java2s.com/Tutorial/CSharp/0140__Class/ComplexIDisposablepattern.htm


****** Built in Lifetime Management within MS Unity

ContainerControlledLifetimeManager : singleton instance with dispose
HierarchicalLifetimeManager : singleton instance per container with dispose
TransientLifetimeManager : empty manager, always returns new object by resolve, no dispose!
PerRequestLifetimeManager (Unity.MVC) : singleton instance per http request with dispose
ExternallyControlledLifetimeManager : code must handle lifetime management
PerResolveLifetimeManager : like TransientLifetimeManager expect when in same object graph
PerThreadLifetimeManager : A LifetimeManager that holds the instances given to it, keeping one instance per thread.

***** complete idisposable pattern  (implemented in CAMP app)
http://www.codeproject.com/Articles/15360/Implementing-IDisposable-and-the-Dispose-Pattern-P
http://www.strathweb.com/2015/08/disposing-resources-at-the-end-of-web-api-request/ -----good

*** utility to check undisposed objects
http://www.codeproject.com/Articles/38318/Finding-Undisposed-Objects
http://undisposed.codeplex.com/releases/view/23017


