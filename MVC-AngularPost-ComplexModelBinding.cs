At Controller =>
[System.Web.Mvc.HttpPost]
  public void Get([FromBody] List<Person> list)
  {
    ....list will have multiple person objects when posted from angular service below
  }
  
Person Entity =>
 [Serializable]
public class Person
  {
      public string FullName { get; set; }
      public string LastName { get; set; }
      public string Address { get; set; }
  }
  
At angular service =>

 var options = {
                url: <url to mvc controller action>,
                method: "POST",
                data: JSON.stringify(<personJsonData>),
                headers: { 'Content-Type': 'application/json' }
            };

            return $http(options).success(function (data, status, headers, config) {

            });


Note:
The List<T> in controller can have multiple child objects in it.
Some serialization links:
1. http://stackoverflow.com/questions/20038441/deserialize-a-listabstractclass-with-newtonsoft-json
2. http://stackoverflow.com/questions/17126323/parsing-a-complex-json-result-with-c-sharp
3. http://www.codeproject.com/Tips/79435/Deserialize-JSON-with-C
4. http://philcurnow.wordpress.com/2013/12/29/serializing-and-deserializing-json-in-c/
5. http://www.cshandler.com/2013/09/deserialize-list-of-json-objects-as.html
