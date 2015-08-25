using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CAMPOracleDataAccess.Data.Utility;
using Dapper;
using System.Configuration;
using CAMPOracleDataAccess.Data.Contracts;
using CAMPExceptionEmailer;
using System.Diagnostics;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using CAMPOracleDataAccess.OracleUtility;

namespace CAMPOracleDataAccess.Data.Repository
{
    public class GenericRepository : IGenericRepository
    {
        

        /// <summary>
        /// Use this method to return single recordset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spname"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> QueryDynamic<T>(string spname, object parameters = null,
            int timeout = 30, CommandType? commandType = null)
        {
            try
            {
                OracleDynamicParameters dynParams = new OracleDynamicParameters();
                dynParams.Add("p_recordset", OracleDbType.RefCursor, ParameterDirection.Output);
                //dynParams.Add(":id", OracleDbType.Int32, ParameterDirection.Input, value);

                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    connection.Open();
                    return connection.Query<dynamic>(spname, param: dynParams,
                        commandTimeout: timeout, commandType: commandType);
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
        }
        #endregion

        #region "Non-Dynamic Methods"
        /// <summary>
        /// Executes a SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        public int Execute(string spname, object parameters = null, int timeout = 30,
            CommandType? commandType = null)
        {
            try
            {
                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    connection.Open();//open connection explicitly. do not call BaseUtility.OpenConnection()
                    return connection.Execute(spname, param: parameters,
                            commandTimeout: timeout, commandType: commandType);
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
        }

        /// <summary>
        /// Executes SQL statement against the connection and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the element to be the returned.</typeparam>
        /// <param name="spname">procedure name</param>
        /// <param name="parameters">input parameters of SP</param>
        /// <param name="timeout">query timeout</param>
        /// <param name="buffered"></param>
        /// <returns>Generic List collection</returns>
        public List<T> QueryList<T>(string spname, object parameters = null,
            int timeout = 30, CommandType? commandType = null, bool buffered = true)
        {
            List<T> type = null;
            try
            {
                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    connection.Open();//open connection explicitly. do not call BaseUtility.OpenConnection()
                    type = connection.Query<T>(spname, param: parameters, commandTimeout: timeout,
                        commandType: commandType, buffered: buffered).ToList();
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
            return type;
        }

        /// <summary>
        /// Executes SQL statement against the connection and builds multiple result sets.
        /// </summary>
        /// <typeparam name="T1">The type of the first result set elements to be the returned.</typeparam>
        /// <typeparam name="T2">The type of the second result set elements to be the returned.</typeparam>
        /// <param name="spname">stored procedure name</param>
        /// <param name="parameters">stored procedure input parameter list</param>
        /// <param name="timeout">query timeout</param>
        /// <param name="commandType">SP or Text</param>
        /// <returns>object of multiple result sets returned by the query</returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>> ExecuteMultiple<T1, T2>(
                        string spname, object parameters = null,
            int timeout = 30, CommandType? commandType = null)
        {
            try
            {
                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    connection.Open();//open connection explicitly. do not call BaseUtility.OpenConnection()
                    using (var multi = connection.QueryMultiple(spname,
                            param: parameters,
                            commandTimeout: timeout,
                            commandType: commandType))
                    {
                        IEnumerable<T1> item1 = multi.Read<T1>();
                        IEnumerable<T2> item2 = multi.Read<T2>();

                        return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
                    }
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="spname"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> ExecuteMultiple<T1, T2, T3>(string spname,
            object parameters = null, int timeout = 30, CommandType? commandType = null)
        {
            try
            {
                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    connection.Open();
                    using (var multi = connection.QueryMultiple(spname,
                            param: parameters,
                            commandTimeout: timeout,
                            commandType: commandType))
                    {
                        IEnumerable<T1> item1 = multi.Read<T1>();
                        IEnumerable<T2> item2 = multi.Read<T2>();
                        IEnumerable<T3> item3 = multi.Read<T3>();

                        return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(item1, item2, item3);
                    }
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="spname"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>
            ExecuteMultiple<T1, T2, T3, T4>(string spname, object parameters = null,
            int timeout = 30, CommandType? commandType = null)
        {
            try
            {
                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    connection.Open();
                    using (var multi = connection.QueryMultiple(spname,
                            param: parameters,
                            commandTimeout: timeout,
                            commandType: commandType))
                    {
                        IEnumerable<T1> item1 = multi.Read<T1>();
                        IEnumerable<T2> item2 = multi.Read<T2>();
                        IEnumerable<T3> item3 = multi.Read<T3>();
                        IEnumerable<T4> item4 = multi.Read<T4>();

                        return new Tuple<IEnumerable<T1>, IEnumerable<T2>,
                            IEnumerable<T3>, IEnumerable<T4>>(item1, item2, item3, item4);
                    }
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="spname"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>>
            ExecuteMultiple<T1, T2, T3, T4, T5>(string spname, object parameters = null,
            int timeout = 30, CommandType? commandType = null)
        {
            try
            {
                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    connection.Open();
                    using (var multi = connection.QueryMultiple(spname,
                            param: parameters,
                            commandTimeout: timeout,
                            commandType: commandType))
                    {
                        IEnumerable<T1> item1 = multi.Read<T1>();
                        IEnumerable<T2> item2 = multi.Read<T2>();
                        IEnumerable<T3> item3 = multi.Read<T3>();
                        IEnumerable<T4> item4 = multi.Read<T4>();
                        IEnumerable<T5> item5 = multi.Read<T5>();

                        return new Tuple<IEnumerable<T1>, IEnumerable<T2>,
                            IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>>(item1, item2, item3, item4, item5);
                    }
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="spname"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>,
            IEnumerable<T5>, IEnumerable<T6>> ExecuteMultiple<T1, T2, T3, T4, T5, T6>(string spname,
            object parameters = null, int timeout = 30, CommandType? commandType = null)
        {
            try
            {
                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    connection.Open();
                    using (var multi = connection.QueryMultiple(spname,
                            param: parameters,
                            commandTimeout: timeout,
                            commandType: commandType))
                    {
                        IEnumerable<T1> item1 = multi.Read<T1>();
                        IEnumerable<T2> item2 = multi.Read<T2>();
                        IEnumerable<T3> item3 = multi.Read<T3>();
                        IEnumerable<T4> item4 = multi.Read<T4>();
                        IEnumerable<T5> item5 = multi.Read<T5>();
                        IEnumerable<T6> item6 = multi.Read<T6>();

                        return new Tuple<IEnumerable<T1>, IEnumerable<T2>,
                            IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>,
                            IEnumerable<T6>>(item1, item2, item3, item4, item5, item6);
                    }
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
        }
        #endregion

        #region "Task-based dynamic"

        public async Task<dynamic> TaskQuerySingleDynamic<T>(string spname, object parameters = null,
            int timeout = 30, CommandType? commandType = null)
        {
            try
            {
                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    await connection.OpenAsync();//open connection explicitly. do not call BaseUtility.OpenConnection()
                    IEnumerable<dynamic> tasks = await connection.QueryAsync<dynamic>(spname, param: parameters,
                            commandTimeout: timeout, commandType: commandType);

                    //if (!tasks.Any())
                    //    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Task not found"));

                    return tasks.SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
        }
        public async Task<IEnumerable<dynamic>> TaskQueryDynamic<T>(string spname, object parameters = null,
            int timeout = 30, CommandType? commandType = null)
        {
            try
            {
                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    await connection.OpenAsync();//open connection explicitly. do not call BaseUtility.OpenConnection()
                    return await connection.QueryAsync<dynamic>(spname, param: parameters,
                        commandTimeout: timeout, commandType: commandType);
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spname"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<List<T>> TaskQueryList<T>(string spname, object parameters = null,
           int timeout = 30, CommandType? commandType = null)
        {
            try
            {
                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    await connection.OpenAsync();//open connection explicitly. do not call BaseUtility.OpenConnection()
                    IEnumerable<T> data = await connection.QueryAsync<T>(spname, param: parameters,
                        commandTimeout: timeout, commandType: commandType);

                    return data.ToList();
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
                throw;
            }
        }

        public async Task<List<IEnumerable<dynamic>>> TaskExecuteDynamic<dynamic>(string spname, object parameters = null,
            int timeout = 30, CommandType? commandType = null, int recordSetCount = 0)
        {
            try
            {
                List<IEnumerable<dynamic>> dynamicList = new List<IEnumerable<dynamic>>();

                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    await connection.OpenAsync();
                    using (var multi = await connection.QueryMultipleAsync(spname,
                            param: parameters,
                            commandTimeout: timeout,
                            commandType: commandType))
                    {
                        for (int i = 1; i <= recordSetCount; i++)
                        {
                            IEnumerable<dynamic> item = await multi.ReadAsync<dynamic>();
                            dynamicList.Add(item);
                        }
                        return dynamicList;
                    }
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException
                {
                    ErrorMessage = ex.Message,
                    MethodName = new StackTrace().GetFrame(1).GetMethod().Name,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source
                });
                throw;
            }
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
        }
        #endregion

        #region "Dynamic Methods - Multiple Recordsets"

        /// <summary>
        /// Use this method to return multiple recordsets.
        /// </summary>
        /// <typeparam name="dynamic"></typeparam>
        /// <param name="spname"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="commandType"></param>
        /// <param name="recordSetCount">Number of recordsets returned by stored procedure</param>
        /// <returns></returns>
        public List<IEnumerable<dynamic>> ExecuteDynamic<dynamic>(string spname, object parameters = null,
            int timeout = 30, CommandType? commandType = null, int recordSetCount = 0)
        {
            try
            {
                List<IEnumerable<dynamic>> dynamicList = new List<IEnumerable<dynamic>>();

                OracleDynamicParameters dynParams = new OracleDynamicParameters();
                dynParams.Add("p_recordset", OracleDbType.RefCursor, ParameterDirection.Output);
                dynParams.Add("p_recordset2", OracleDbType.RefCursor, ParameterDirection.Output);

                using (var connection = new OracleConnection(BaseUtility.ConnectionString))
                {
                    connection.Open();
                    using (var multi = connection.QueryMultiple(spname,
                            param: dynParams,
                            commandTimeout: timeout,
                            commandType: commandType))
                    {
                        //for (int i = 1; i <= recordSetCount; i++)
                        //{
                        //    IEnumerable<dynamic> item = multi.Read<dynamic>();
                        //    dynamicList.Add(item);
                        //}
                        //return dynamicList;

                        var customer = multi.Read<dynamic>().ToList();
                        var orders = multi.Read<dynamic>().ToList();

                        dynamicList.Add(customer);
                        dynamicList.Add(orders);

                        return dynamicList;
                    }
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException
                {
                    ErrorMessage = ex.Message,
                    MethodName = new StackTrace().GetFrame(1).GetMethod().Name,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source
                });
                throw;
            }

        }

        #endregion


        /// <summary>
        /// First close the connection and then dispose via the IDisposable interface implementation
        /// </summary>
        /// <param name="disposing"></param>
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (_disposed)
        //        return;

        //    if (disposing)
        //    {
        //        if (connection != null)
        //        {
        //            if (connection.State == ConnectionState.Open)
        //                connection.Close();
        //            connection.Dispose();
        //            connection = null;
        //        }
        //        _disposed = true;
        //    }
        //}

        /// <summary>
        /// Call GC.SuppressFinalize to let garbage collector know that this resource has cleaned up its own resources and doesn't need to be finalized.
        /// calling finalizer is an expensive operation so we avoid it.
        /// </summary>
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
