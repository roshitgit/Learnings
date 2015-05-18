http://stackoverflow.com/questions/9518119/mapping-entity-in-dapper

**** stored procedure:

CREATE PROCEDURE [dbo].GetUserById (@UserId int)
AS  
begin               
        SELECT UserId,LastName,FirstName,EmailAddress
        FROM users
        WHERE UserID = @UserId

end
go

*** entity:

public class User
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
}


Dapper deliberately doesn't have a mapping layer; it is the absolute minimum that can work, and frankly 
covers the majority of real scenarios in the process. However, if I understand correctly that you don't want to 
alias in the TSQL, and don't want any pass-thru properties - then use the non-generic Query API:

User user = connection.Query("...", ...).Select(obj => new User {
           Id = (int) obj.UserId,
           FirstName = (string) obj.FirstName,
           LastName = (string) obj.LastName,
           Email = (string) obj.EmailAddress
        }).FirstOrDefault();
or perhaps more simply in the case of a single record:

var obj = connection.Query("...", ...).FirstOrDefault();
User user = new User {
      Id = (int) obj.UserId,
      FirstName = (string) obj.FirstName,
      LastName = (string) obj.LastName,
      Email = (string) obj.EmailAddress
};
The trick here is that the non-generic Query(...) API uses dynamic, offering up members per column name.
