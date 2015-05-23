using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Dapper;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Nito.AsyncEx.AsyncContext.Run(() => MainAsync(args));
        }

        static async Task MainAsync(string[] args)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            using (var server = new HttpServer(config))
            using (var client = new HttpClient(server))
            {
                client.BaseAddress = new Uri("http://localhost/");
                var cts = new CancellationTokenSource();

                var json = @"{""title"":""Task"",""description"":""The task"",""createdDate"":""" + DateTime.UtcNow.ToString() + "\"}";
                var postRequest = new HttpRequestMessage(HttpMethod.Post, "/api/tasks")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                var postResponse = await client.SendAsync(postRequest, cts.Token);
                Trace.Assert(postResponse.StatusCode == HttpStatusCode.Created);

                var location = postResponse.Headers.Location.AbsoluteUri;
                var getResponse = await client.GetAsync(location);
                Trace.Assert(getResponse.StatusCode == HttpStatusCode.OK);

                var getBody = await getResponse.Content.ReadAsAsync<JObject>();
                dynamic data = getBody;
                Trace.Assert((string)data.title == "Task");
            }

            Console.WriteLine("Press any key to quit.");
            Console.ReadLine();
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

    public class TasksController : ApiController
    {
        static string _connString = ConfigurationManager.ConnectionStrings["Database1"].ConnectionString;
        public async Task<IEnumerable<dynamic>> GetAll()
        {
            using (var connection = new SqlCeConnection(_connString))
            {
                await connection.OpenAsync();

                IEnumerable<dynamic> tasks = await connection.QueryAsync<dynamic>("select Id as id, Title as title, Description as description, CreatedDate as createdDate from Tasks;");
                return tasks;
            }
        }

        public async Task<dynamic> Get(int id)
        {
            using (var connection = new SqlCeConnection(_connString))
            {
                await connection.OpenAsync();

                IEnumerable<dynamic> tasks = await connection.QueryAsync<dynamic>("select Id as id, Title as title, Description as description, CreatedDate as createdDate from Tasks where Id = @id;", new { id = id });
                if (!tasks.Any())
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Task not found"));
                
                return tasks.First();
            }
        }

        public async Task<HttpResponseMessage> Post(JObject value)
        {
            dynamic data = value;
            IEnumerable<int> result;
            using (var connection = new SqlCeConnection(_connString))
            {
                await connection.OpenAsync();

                connection.Execute(
                    "insert into Tasks (Title, Description, CreatedDate) values (@title, @description, @createdDate);",
                    new
                    {
                        title = (string)data.title,
                        description = (string)data.description,
                        createdDate = DateTime.Parse((string)data.createdDate)
                    }
                );

                result = await connection.QueryAsync<int>("select max(Id) as id from Tasks;");
            }

            int id = result.First();
            data.id = id;
            var response = Request.CreateResponse(HttpStatusCode.Created, (JObject)data);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { controller = "Tasks", id = id }));
            return response;
        }
    }
}
