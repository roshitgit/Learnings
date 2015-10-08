/// <summary>
    /// This extension method converts dynamic list to "IDictionary<string, object>"
    /// and then converts it to ADO.NET Datatable
    /// </summary>
    public static class DynamicExtensions
    {
        public static List<IDictionary<string, object>> ConvertToIDictionary(IEnumerable<dynamic> dynamicList)
        {
            List<IDictionary<string, object>> idictionary = new List<IDictionary<string, object>>();

            foreach (var row in dynamicList)
            {
                var fields = row as IDictionary<string, object>;
                idictionary.Add(fields);
            }

            return idictionary;
        } 
        public static DataTable ConvertDynamicListToDataTable(this IEnumerable<dynamic> dynamicList)
        {
            DataTable dt = new DataTable();

            List<IDictionary<string, object>> idictionary = ConvertToIDictionary(dynamicList);

            foreach (string column in idictionary[0].Keys)
            {
                dt.Columns.Add(column);
            }

            foreach (IDictionary<string, object> dictionary in idictionary)
            {
                DataRow dataRow = dt.NewRow();

                foreach (string column in dictionary.Keys)
                {
                    dataRow[column] = dictionary[column];
                }

                dt.Rows.Add(dataRow);
            }

            return dt;
        }
    }
