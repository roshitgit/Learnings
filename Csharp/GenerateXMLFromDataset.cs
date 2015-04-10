/*
The code below shows how to read data from excel file  (.xls or .xlsx) into .net dataset.
uses system.data.oledb.dll to read from excel file
Then generate xml out of the dataset.
It allows validates excel for column length and column names
if any colums are blank in excel, then "empty tags" will be added in xml via table cloning mechanism

*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.OleDb; //.net driver to read data from excel file
using System.Data;

namespace GenerateXMLFromExcel
{
    public static class DataColumnCollectionExtensions
    {
        public static IEnumerable<DataColumn> AsEnumerable(this DataColumnCollection source)
        {
            return source.Cast<DataColumn>();
        }
    }

    /// <summary>
    /// Make all columns of type string, 
    /// replace all null values with string.empty, then call GetXml
    /// </summary>
    public class ExcelGenerator
    {
        private string excelPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\<filename>.xlsx";
        private string fileName = "<file name>.xlsx";
        private string ExcelProcessConnectionString = string.Empty;

        public void GenerateXMLFromExcel()
        {
            try
            {
                if (Path.GetExtension(fileName) == ".xlsx")
                {
                    ExcelProcessConnectionString =
                     "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + excelPath + ";" +
                     "Extended Properties=Excel 12.0";
                }
                else
                {
                    ExcelProcessConnectionString =
                     "Provider=Microsoft.Jet.OLEDB.4.0;" +
                     "Data Source=" + excelPath + ";" +
                     "Extended Properties=Excel 8.0";
                }

                using (OleDbConnection oledbConn = new OleDbConnection(ExcelProcessConnectionString))
                {
                    OleDbCommand oledbCmd = new OleDbCommand();
                    oledbCmd.Connection = oledbConn;

                    //DataColumnCollection dcc = oledbConn.GetSchema("Tables").Columns;

                    if (oledbConn.State == ConnectionState.Closed) oledbConn.Open();

                    DataTable sheetTable = oledbConn.GetSchema("Tables");
                    string strSHeetName = Convert.ToString(sheetTable.Rows[0]["TABLE_NAME"]);

                    if (ExcelIsValid(oledbConn, strSHeetName))
                    {
                        string selectQuery = "Select [COL1],[COL2] from [" + strSHeetName + "]";

                        OleDbDataAdapter da = new OleDbDataAdapter(selectQuery, oledbConn);
                        da.SelectCommand = oledbCmd;
                        da.SelectCommand.CommandText = selectQuery;
                        da.SelectCommand.CommandType = CommandType.Text;

                        DataTable dtActual = new DataTable();

                        da.Fill(dtActual);

                        DataTable dtCloned = dtActual.Clone();

                        foreach (DataColumn dc in dtCloned.Columns) {
                            dc.DataType = typeof(string); //change each datatype of column to string
                            dc.ColumnMapping = MappingType.Element;
                            //dc.DefaultValue = string.Empty;
                        }
                        

                        //import data from actual table to the cloned table
                        foreach (DataRow row in dtActual.Rows)
                        {
                            dtCloned.ImportRow(row);
                        }

                        foreach (DataRow row in dtCloned.Rows)
                        {
                            for (int i = 0; i < dtCloned.Columns.Count; i++)
                            {
                                dtCloned.Columns[i].ReadOnly = false;

                                if (string.IsNullOrEmpty(row[i].ToString()))
                                    row[i] = string.Empty;
                                    //row[i] = "null";
                            }
                        }

                        DataSet ds = new DataSet("AdhocData");
                        ds.Tables.Add(dtCloned);
                        ds.Tables[0].TableName = "Table";

                        string xml = ds.GetXml().Replace("_x0020_", "_");
                    }
                }
            }
            catch (Exception ex)
            {
                Environment.Exit(1); //close app
            }
        }

        /// <summary>
        /// Validate for the items mentioned below
        /// 1. Column count should match
        /// 2. Column Names should be correct
        /// </summary>
        /// <param name="oledbConn"></param>
        /// <returns></returns>
        private static bool ExcelIsValid(OleDbConnection oledbConn, string sheetName)
        {
            string[] ExcelColNames = { "COL1", "COl2"};

            int validColCount = ExcelColNames.Length;
            bool FileIsValid = true;
            List<string> fileColumns = new List<string>();


            string[] restrictions = new string[4] { null, null, sheetName, null };
            //Apply restrictions to get data only from the first sheet of excel file.
            //these restrictions work even if there are blank excel sheets in files.
            //if restrictions are not applied, then it gets junk columns from all available sheets in excel
            DataTable dtCols = oledbConn.GetSchema("Columns", restrictions );

            foreach (DataRow dr in dtCols.Rows)
            {
                fileColumns.Add(dr["COLUMN_NAME"].ToString());
            }

            if (fileColumns.Count != validColCount)
            {
                FileIsValid = false;
            }
            else //if column count match, process further
            {
                foreach (string name in ExcelColNames)
                {
                    //if (!fileColumns.Any(x => x.ToLower().Contains(name)))
                    if (!fileColumns.Any(x => x.Contains(name)))
                    {
                        FileIsValid = false;
                    }
                }
            }

            return FileIsValid;
        }
    }
}
