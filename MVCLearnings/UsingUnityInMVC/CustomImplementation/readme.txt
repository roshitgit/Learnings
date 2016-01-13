In case we do not want to use Unity.MVC4 and Unity.WebAPI, then use the bootstrapper class
and do constructor injection to resolve dependencies using MVC 4 web api.

Not sure if unity.mvc4 disposes objects so can try the custom one which does dispose.

details here:
http://www.asp.net/web-api/overview/advanced/dependency-injection
