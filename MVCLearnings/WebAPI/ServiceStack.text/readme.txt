*** steps to use servicestack.text in MVC 4+ Web API with Angular/Jquery applications
step1 => download servicestack.text in MVC app from nuget using "Install-Package ServiceStack.Text"
step2 => Add formatter & setup formatter as shown in link below
       http://www.strathweb.com/2013/01/replace-json-net-with-servicestack-text-in-asp-net-web-api
step3 => If you are using "JObject" while posting object to MVC APIController using Angular/ JQuery,
       then replace "JObject" with "ServiceStack.Text.JsonObject" in the code.

** dynamic
https://gist.github.com/danielcrenna/3719610
https://github.com/ServiceStack/ServiceStack.Text#supports-dynamic-json
http://stackoverflow.com/questions/26080003/deserialize-json-into-a-dynamic-object-with-servicestack-text
http://www.ienablemuch.com/2012/05/anonymous-type-dynamic-servicestack.html

https://servicestack.net/download
https://servicestack.net/
http://www.strathweb.com/2013/01/replace-json-net-with-servicestack-text-in-asp-net-web-api/ --- must see and try out

https://github.com/ServiceStack/ServiceStack/wiki/Mvc-integration
http://blogs.askcts.com/2014/04/23/getting-started-with-servicestack-part-one/
http://www.slideshare.net/Fabio.Cozzolino/introduction-to-service-stack
http://stackoverflow.com/questions/19050672/servicestack-text-output-utc-offset

https://components.xamarin.com/view/servicestacktext
http://codereply.com/answer/5o5wok/serializing-expandoobject-servicestacktext.html


** good example of using web api routes with verbs
http://www.dotnetcurry.com/aspnet/1056/introducing-service-stack-tutorial

** alternate json serializer
https://weblog.west-wind.com/posts/2012/Mar/09/Using-an-alternate-JSON-Serializer-in-ASPNET-Web-API

**  dynamic-expandoobject-to-serialize-to-json-as-expected
http://www.patridgedev.com/2011/08/24/getting-dynamic-expandoobject-to-serialize-to-json-as-expected/

**web-api-to-servicestack-jobject-to-jsonobject
http://stackoverflow.com/questions/20636410/converting-web-api-to-servicestack-jobject-to-jsonobject

http://stackoverflow.com/questions/17359558/servicestack-jsonserializer-not-serializing-object-members-when-inheritance
http://stackoverflow.com/questions/14387202/how-to-use-a-custom-json-serializer-in-servicestack

** ServiceStackJsonFormatter.cs - github/ gist
https://gist.github.com/mrchief/4043834

** use ServiceStack.JsonSerializer in MVC
http://docs.servicestack.net/text-serializers/json-csv-jsv-serializers
http://docs.servicestack.net/text-serializers/json-serializer
** note: Based on the Northwind Benchmarks it's 3.6x faster than .NET's BCL JsonDataContractSerializer and 3x faster then the 
** previous fastest JSON serializer benchmarked - JSON.NET.
