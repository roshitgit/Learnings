/*

Note: JIL serializer does not work with C# dynamic objects. It requires C# concrete classes for the Entities
Ex: If MVC controller returns IEnumarable<dynamic>, dynamic or Task<dynamic>, it wont work especially if Dapper ORM is used
to convert to dynamic.

*/

http://blog.developers.ba/replace-json-net-jil-json-serializer-asp-net-web-api/
