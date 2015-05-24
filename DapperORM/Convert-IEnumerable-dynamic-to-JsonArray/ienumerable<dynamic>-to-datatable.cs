DataTable dt;
string query = "SELECT * FROM WORKSHOP WHERE WORKSHOPID = 1";

// Execute Query
var result = Execute(query);

// Convert IEnumerable<dynamic> to DataTable (I Have Problem Here)
dt = CsvConverter.EnumToDataTable(result);

// Convert DataTable To CSV
var csv = CsvConverter.DataTableToCsv(dt, ",", true);

// Save File
string fileName = Path.GetTempPath() + Guid.NewGuid().ToString() + ".csv";
File.AppendAllText(fileName, csv);

// Method to Execute Query
public IEnumerable<dynamic> Execute(string commandText)
{
   using (var result = databaseManager.ReadData(commandText, false))
      foreach (IDataRecord record in result)
      {
         yield return new DataRecordDynamicWrapper(record);
      }
}

// Wrapper of Dynamic Record
public class DataRecordDynamicWrapper : DynamicObject
{
    private IDataRecord _dataRecord;
    public DataRecordDynamicWrapper(IDataRecord dataRecord) { _dataRecord = dataRecord; }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        result = _dataRecord[binder.Name];
        return result != null;
    }
}

// Method to Convert Enum to DT
public static DataTable EnumToDataTable<T>(IEnumerable<T> l_oItems)
    {
        DataTable oReturn = new DataTable(typeof (T).Name);
        object[] a_oValues;
        int i;

        //#### Collect the a_oProperties for the passed T
        PropertyInfo[] a_oProperties = typeof (T).GetType().GetProperties();


        //#### Traverse each oProperty, .Add'ing each .Name/.BaseType into our oReturn value
        //####     NOTE: The call to .BaseType is required as DataTables/DataSets do not support nullable types, so it's non-nullable counterpart Type is required in the .Column definition
        foreach (PropertyInfo oProperty in a_oProperties)
        {
            oReturn.Columns.Add(oProperty.Name, BaseType(oProperty.PropertyType));
        }

        //#### Traverse the l_oItems
        foreach (T oItem in l_oItems)
        {
            //#### Collect the a_oValues for this loop
            a_oValues = new object[a_oProperties.Length];

            //#### Traverse the a_oProperties, populating each a_oValues as we go
            for (i = 0; i < a_oProperties.Length; i++)
            {
                a_oValues[i] = a_oProperties[i].GetValue(oItem, null);
            }

            //#### .Add the .Row that represents the current a_oValues into our oReturn value
            oReturn.Rows.Add(a_oValues);
        }

        //#### Return the above determined oReturn value to the caller
        return oReturn;
    }

    public static Type BaseType(Type oType)
    {
        //#### If the passed oType is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
        if (oType != null && oType.IsValueType &&
            oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof (Nullable<>)
            )
        {
            return Nullable.GetUnderlyingType(oType);
        }
            //#### Else the passed oType was null or was not logicially nullable, so simply return the passed oType
        else
        {
            return oType;
        }
    }
c#
