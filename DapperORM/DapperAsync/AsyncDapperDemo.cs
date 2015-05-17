using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
 
public class Program
{
	public static void Main()
	{
		var connectionString = "your connection string";
		PersonRepository personRepo = new PersonRepository(connectionString);
		Person person = null;
		Guid Id = new Guid("{82B31BB2-85BF-480F-8927-BB2AB71CE2B3}");
		
		// Typically, you'd be doing this inside of an async Web API controller, not the main method of a console app.
		// So, we're just using Task.Factory to simulate an async Web API call.
		var task = new Task(async () =>
		{
			person = await personRepo.GetPersonById(Id);
		});
		
		// This just prevents the console app from exiting before the async work completes.
		Task.WaitAll(task);
	}
}
 
// Just a simple POCO model
public class Person
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Phone { get; set; }
	public string Email { get; set; }
}
 
// Yes, I know this doesn't fit definition of a generic repository, 
// but the assumption is that I have no idea how you want to get 
// your data. That's up to you. This Base repo exists for the 
// sole purpoose of providing SQL connection management.
public abstract class BaseRepository
{
	private readonly string _ConnectionString;
	protected BaseRepository(string connectionString)
	{
		_ConnectionString = connectionString;
	}
 
	// use for buffered queries that return a type
	protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
	{
		try
		{
			using (var connection = new SqlConnection(_ConnectionString))
			{
				await connection.OpenAsync();
				return await getData(connection);
			}
		}
		catch (TimeoutException ex)
		{
			throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
		}
		catch (SqlException ex)
		{
			throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
		}
	}
	
	// use for buffered queries that do not return a type
	protected async Task WithConnection(Func<IDbConnection, Task> getData)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    await connection.OpenAsync();
                    await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }
 
	// use for non-buffered queries that return a type
	protected async Task<TResult> WithConnection<TRead, TResult>(Func<IDbConnection, Task<TRead>> getData, Func<TRead, Task<TResult>> process)
	{
		try
		{
			using (var connection = new SqlConnection(_ConnectionString))
			{
				await connection.OpenAsync();
				var data = await getData(connection);
				return await process(data);
			}
		}
		catch (TimeoutException ex)
		{
			throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
		}
		catch (SqlException ex)
		{
			throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
		}
	}
	
	
}
 
public class PersonRepository : BaseRepository
{
	public PersonRepository(string connectionString): base (connectionString)
	{
	}
 
	// Assumes you have a Person table in your DB that 
	// aligns with the Person POCO model.
	//
	// Assumes you have an exsiting SQL sproc in your DB 
	// with @Id UNIQUEIDENTIFIER as a parameter. The sproc 
	// returns rows from the Person table.
	public async Task<Person> GetPersonById(Guid Id)
	{
		return await WithConnection(async c =>
		{
			var p = new DynamicParameters();
			p.Add("Id", Id, DbType.Guid);
			var people = await c.QueryAsync<Person>(sql: "sp_Person_GetById", param: p, commandType: CommandType.StoredProcedure);
			return people.FirstOrDefault();
		});
	}
}
