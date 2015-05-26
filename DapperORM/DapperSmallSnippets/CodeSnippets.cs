/******************************Using Dapper Dynamic in MVC controller *******************************/
using Dapper;
 
namespace DapperCleanDemo.Controllers
{
    public class PeopleController : ApiController
    {
        public IEnumerable<dynamic> GetResultById(string id)
        {
            using (var conn = new SqlConnection(@"Server=localhost\sqlexpress;Database=XmlTest;Trusted_Connection=True"))
            {
                var rows = conn.Query("select id,name FROM people WHERE Id=@Id", new { Id = id });
 
                return rows;
            }
        }
    }
}


You could use a dynamic query and map it afterwards. Something like this

var result = conn.Query<dynamic>(query).Select(x => new Tuple<Type1, Type2, Type3, Type4, Type5>( 
// type initialization here 
    new Type1(x.Property1,x.Property2),
    new Type2(x.Property3,x.Property4),
    new Type3(x.Property5,x.Property6) etc....));
    
Edit: With a rather huge result set, another option might be to use multiple querys and then use a Grid Reader. 
That might work for you.

There's the example taken from the dapper age:

var sql = 
@"
select * from Customers where CustomerId = @id
select * from Orders where CustomerId = @id
select * from Returns where CustomerId = @id";

using (var multi = connection.QueryMultiple(sql, new {id=selectedId}))
{
   var customer = multi.Read<Customer>().Single();
   var orders = multi.Read<Order>().ToList();
   var returns = multi.Read<Return>().ToList();
   ...
} 


***Dapper ORM with dynamic model - How to return no field instead of 'Field' = NULL?
public static class DapperRowExtensions
{
    public static IEnumerable<dynamic> RemoveNullParams(this IEnumerable<dynamic> rows)
    {
        foreach (var row in rows)
        {
            var item = (IDictionary<string, object>)row;
            foreach (var key in item.Keys.ToList())
            {
                if (item[key] == null)
                    item.Remove(key);
            }
        }
        return rows;
    }
}
You would then use it on the result from the query:

var result = connection.Query("SELECT...");
return result.RemoveNullParams();
