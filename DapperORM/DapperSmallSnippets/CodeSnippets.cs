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
