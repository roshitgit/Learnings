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
