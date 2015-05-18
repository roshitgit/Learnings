//As per dapper documentation use the query method and get your dymanics:

dynamic account = conn.Query<dynamic>(@"
                    SELECT Name, Address, Country
                    FROM Account
            WHERE Id = @Id", new { Id = Id }).FirstOrDefault();
Console.WriteLine(account.Name);
Console.WriteLine(account.Address);
Console.WriteLine(account.Country);

/*As you can see you get a dynamic object and you can access its properties as long as they are well defined in the 
query statement. If you ommit .FirstOrDefault() you get an IEnumerable<dynamic> which you can do whatever you want with it.*/
