/*

Note: 
1. JIL serializer does not work with C# dynamic objects. It requires C# concrete classes for the Entities
   Ex: If MVC controller returns IEnumarable<dynamic>, dynamic or Task<dynamic>, it wont work especially if Dapper ORM is used
  to convert to dynamic.

2. JIL works with .NET 4.5+. Better to use with MVC as shown in link below. 
3. It works fine with MVC4 & .NET 4.5

Better to check for performance gain and do POC before using in production

*/

http://blog.developers.ba/replace-json-net-jil-json-serializer-asp-net-web-api/
https://github.com/jbattermann/Nancy.Serialization.Jil--- this should be used
