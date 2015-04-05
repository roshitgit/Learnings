using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;


namespace Dapper
{
public static   class SqlMapperUtil
    {
         // Remember to add <remove name="LocalSqlServer" > in ConnectionStrings
	 section if using this, as otherwise it would be the first one.
        private static string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;

        /// <summary>
        /// Gets the open connection.
        /// </summary>
        /// <param name="name">The name of the connection string (optional).</param>
        /// <returns></returns>
      public static SqlConnection GetOpenConnection( string name = null)
      {
          string connString = "";
        connString= name==null?connString = ConfigurationManager.ConnectionStrings[0].ConnectionString:connString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
        var connection = new SqlConnection(connString);
        connection.Open();
        return connection;
      }


         public static int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connectionName=null) where T : class, new()
        {
             using (SqlConnection cnn = GetOpenConnection(connectionName ))
            {
                int records = 0;

                foreach (T entity in entities)
                {
                    records += cnn.Execute(sql, entity);
                 }
                 return records;
             }
        }

        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
             for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                 table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in list)
            {
                 for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                 table.Rows.Add(values);
             }
             return table;
        }
     
     public static DynamicParameters GetParametersFromObject( object obj, string[] propertyNamesToIgnore)
     {
         if(propertyNamesToIgnore ==null)propertyNamesToIgnore = new string[]{String.Empty};
         DynamicParameters p = new DynamicParameters();
         PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

         foreach (PropertyInfo prop in properties)
         {
             if(   !propertyNamesToIgnore.Contains(prop.Name ))
             p.Add("@" + prop.Name, prop.GetValue(obj, null));
         }
         return p;
     }

         public static void SetIdentity<T>(IDbConnection connection, Action<T> setId)
        {
            dynamic identity = connection.Query("SELECT @@IDENTITY AS Id").Single();
            T newId = (T)identity.Id;
            setId(newId);
        }

     
     public static object GetPropertyValue(object target, string propertyName )
     {
         PropertyInfo[] properties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

         object theValue = null;
            foreach (PropertyInfo prop in properties)
             {
                 if (string.Compare(prop.Name, propertyName, true) == 0)
                {
                    theValue= prop.GetValue(target, null);
                }
            }
         return theValue;
     }

    public static void SetPropertyValue(object p, string propName, object value)
     {
         Type t = p.GetType();
         PropertyInfo info = t.GetProperty(propName);
         if (info == null)
             return ;
         if (!info.CanWrite)
             return;
         info.SetValue(p, value, null);
     }

         /// <summary>
        /// Stored proc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procname">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
      public  static List<T> StoredProcWithParams<T>(string procname, dynamic parms, string connectionName = null)
        {
             using (SqlConnection connection = GetOpenConnection(connectionName))
             {
                  return connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure).ToList();
            }

        }


       /// <summary>
      /// Stored proc with params returning dynamic.
      /// </summary>
      /// <param name="procname">The procname.</param>
      /// <param name="parms">The parms.</param>
      /// <param name="connectionName">Name of the connection.</param>
      /// <returns></returns>
      public static List<dynamic> StoredProcWithParamsDynamic(string procname, dynamic parms, string connectionName=null)
      {
           using (SqlConnection connection = GetOpenConnection(connectionName))
           {
               return connection.Query(procname, (object)parms, commandType: CommandType.StoredProcedure).ToList();
          }
      }
     
      /// <summary>
      /// Stored proc insert with ID.
      /// </summary>
      /// <typeparam name="T">The type of object</typeparam>
      /// <typeparam name="U">The Type of the ID</typeparam>
      /// <param name="procName">Name of the proc.</param>
      /// <param name="parms">instance of DynamicParameters class. This
	 should include a defined output parameter</param>
      /// <returns>U - the @@Identity value from output parameter</returns>
      public static U StoredProcInsertWithID<T,U>(string procName, DynamicParameters  parms, string connectionName=null)
      {
           using (SqlConnection connection = SqlMapperUtil.GetOpenConnection(connectionName))
          {
              var x = connection.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure);
               return parms.Get<U>("@ID");
          }
       }


       /// <summary>
      /// SQL with params.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="sql">The SQL.</param>
      /// <param name="parms">The parms.</param>
      /// <returns></returns>
       public  static List<T> SqlWithParams<T>(string sql, dynamic parms,string connectionnName=null)
        {
             using (SqlConnection connection = GetOpenConnection( connectionnName))
             {
                  return connection.Query<T>(sql, (object)parms).ToList();
            }
        }

       /// <summary>
       /// Insert update or delete SQL.
       /// </summary>
       /// <param name="sql">The SQL.</param>
       /// <param name="parms">The parms.</param>
       /// <returns></returns>
       public static int InsertUpdateOrDeleteSql(string sql, dynamic parms, string connectionName=null)
        {
           using (SqlConnection connection = GetOpenConnection(connectionName))
             {
                  return connection.Execute(sql, (object)parms);
            }
        }

       /// <summary>
       /// Insert update or delete stored proc.
       /// </summary>
       /// <param name="procName">Name of the proc.</param>
       /// <param name="parms">The parms.</param>
       /// <returns></returns>
       public static int InsertUpdateOrDeleteStoredProc(string procName, dynamic parms, string connectionName =null)
        {
             using (SqlConnection connection = GetOpenConnection( connectionName))
             {
                  return connection.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure );
            }
        }

       /// <summary>
       /// SQLs the with params single.
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="sql">The SQL.</param>
       /// <param name="parms">The parms.</param>
       /// <param name="connectionName">Name of the connection.</param>
       /// <returns></returns>
     public static T SqlWithParamsSingle<T>( string sql, dynamic parms, string connectionName=null)
       {
           using (SqlConnection connection = GetOpenConnection(connectionName))
           {
                 return connection.Query<T>(sql, (object) parms).FirstOrDefault();
           }
       }

     /// <summary>
     ///  proc with params single returning Dynamic object.
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="sql">The SQL.</param>
     /// <param name="parms">The parms.</param>
     /// <param name="connectionName">Name of the connection.</param>
     /// <returns></returns>
     public static System.Dynamic.DynamicObject DynamicProcWithParamsSingle<T>(string sql, dynamic parms, string connectionName=null)
     {
         using (SqlConnection connection = GetOpenConnection(connectionName))
         {
             return connection.Query(sql, (object)parms,commandType: CommandType.StoredProcedure ).FirstOrDefault();
         }
     }
     
     /// <summary>
     /// proc with params returning Dynamic.
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="sql">The SQL.</param>
     /// <param name="parms">The parms.</param>
     /// <param name="connectionName">Name of the connection.</param>
     /// <returns></returns>
     public static IEnumerable<dynamic> DynamicProcWithParams<T>(string sql, dynamic parms, string connectionName=null)
     {
         using (SqlConnection connection = GetOpenConnection(connectionName))
         {
             return connection.Query(sql, (object)parms, commandType: CommandType.StoredProcedure);
         }
     }


     /// <summary>
     /// Stored proc with params returning single.
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="procname">The procname.</param>
     /// <param name="parms">The parms.</param>
     /// <param name="connectionName">Name of the connection.</param>
     /// <returns></returns>
     public static T StoredProcWithParamsSingle<T>(string procname, dynamic parms, string connectionName=null)
     {
         using (SqlConnection connection = GetOpenConnection(connectionName))
         {
            return connection.Query<T>(procname, (object) parms, commandType: CommandType.StoredProcedure).SingleOrDefault();
         }
     }
    }
}

/*
You can see there is a method to return an open connection, with an optional parameter for the connection string name. The default value of this is null, and if it is, it returns the first connection string element in the configuration file. To use this you must add the <remove name="LocalSqlServer" /> directive at the beginning of your connectionStrings section. Otherwise, it will return the connection string by name.

    The InsertMultiple method accepts an IEnumerable of your type, iterates over it, and performs an insert on every one, all on the same connection.

    The ToDatatable method is a convenience method to convert an IList of your objects to a DataTable. I rarely use it, but I keep it in the class for convenience.

    The GetParametersFromObject method accepts your type and returns a set of Dapper DynamicParameters, optionally excluding a list of property names to ignore. This is useful since many of the underlying Dapper methods want DynamicParameters. Of course you can create your own DynamicParameters, optionally specifying properties such as a parameter being an output parameter.

    The SetIdentity method is useful when you are doing an insert and want to get back the @@IDENTITY value of the new row. You would call this method immediately after the line of code that performs your insert, and before the connection is closed.

    The GetPropertyValue and SetPropertyValue methods are, again, convenience methods. I don't often use these, but as above, I keep the methods in the class so I don't need to go hunting around for code when I do.

    The StoredProcWithParams<T> method executes a named stored proc, accepting an instance of dynamic containing the parameters, and an optional connection name.

    The   U StoredProcInsertWithID<T,U> method executes a stored proc and expects a defined output parameter to return the value to the caller.

    The SqlWithParams<T> method executes a specified SQL string with dynamic parameters and returns a List<T> of the type specified.

    The InsertUpdateOrDeleteSql method will perform an insert, Update, or Delete with the specified SQL with dynamic parameters and returns the integer result of the operation.

    The InsertUpdateOrDeleteStoredProc  method does the same thing but is used to execute a named stored procedure.

    The T SqlWithParamsSingle<T> executes the specified SQL with the supplied dynamic for parameters, and returns a single instance of the type T.

    The DynamicProcWithParamsSingle<T> executes a stored proc and returns type DynamicObject.

    The IEnumerable<dynamic> DynamicProcWithParams<T> does the same and returns an IEnumerable of type dynamic.

    The T StoredProcWithParamsSingle<T> executes a stored procedure with the specified dynamic for params and returns a single of type T that was specified.

    There are other variations of these that can be created; the idea is simply to "wrap" the lines of code needed to do some operation and make it very easy to use.

    In the downloadable demo solution, I have four sample stored procs that you can put into the Northwind Database in SQL Server; these are used in the demo code.

    There are three classes in the demo: Employee, EmployeeTerritory, and Territory. One interesting use of these is to perform mapping of subobjects. My Employee class has a property of type:

         public  List<Territory> Territories { get; set; }

I do this because one employee can have more than one territory. Here is how I do the mapping:

  public static List<Employee>  GetEmployeesWithRegionAndTerritory()
        {
            List<Employee> employees;
             using (SqlConnection conn = SqlMapperUtil.GetOpenConnection("local"))
            {
                List<EmployeeTerritory> employeeTerritories;
                List<Territory> territories;

                 using (var multi = conn.QueryMultiple("GetEmployeesWithTerritory", null, commandType: CommandType.StoredProcedure))
                {
                    employees = multi.Read<Employee>().ToList();
                    employeeTerritories = multi.Read<EmployeeTerritory>().ToList(); // this is the junction table
                    territories = multi.Read<Territory>().ToList();
                    foreach (var emp in employees)
                    {
                        emp.Territories = new List<Territory>();
                            foreach (var empter in employeeTerritories)
                            {
                                foreach (var ter in territories)
                                  {
                                       if (empter.EmployeeId == emp.EmployeeID && ter.TerritoryID == empter.TerritoryId)
                                           emp.Territories.Add(ter);
                                }
                            }
                        }
                    } // end using multi
                } // end using conn

               return employees;

            } // end  method


This uses the Dapper QueryMultiple method, which returns multiple select results. You can also do this with LINQ, but I'm leaving that as an exercise for the reader.  Dapper can also support Table Valued Parameters which is a very efficient was to perform an insert of multiple objects in one go.

There's a lot more to Dapper-Dot-Net, but I hope this provides enough material to get you interested. Remember: raw speed is your friend! The SqlMapper.cs Dapper file included is the latest one as of the date of this article on 12/22/2011.

*/
