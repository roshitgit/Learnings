To read .xls or .xlsx excel format files using .NET MVC, carry out following steps

1. Install "MS Access database engine 2010 redistributable" 64 bit from
            <a href="http://www.microsoft.com/en-us/download/details.aspx?id=13255">Download link</a>.
    Install 64 bit to install on windows 2008, 2008 r2 machines which are 64 bit ones.
    install 32 bit to install on windows 2003 server and below which are 32 bit machines.
    this redistributable works if code justs reads and excel file and processes records in it.
    this may not work if .net code tries to create excel file as for creation and to open file, 
    ms office has to be installed.
    
    if application has excel file upload functionality, then its better to upload file using server.mappath
    so that file sits on the path where code is hosted in IIS. Create a folder under /Root of site and do server.mappath on it.

2. Set full access to "Authenticated Users" role on  the folder in IIS.
    Go in IIS, navigate to the virtual directory of site.
    right click and do edit permissions and give full access to "Authenticated Users" role.
    
    If MS Office is installed, no need to do step 1 from above. 
    Thereby code works properly on local machine as local machines normally would have all softwares installed on it.
    
    But on server, most likely MS-office won't be installed. Thats why to identify providers such as 
    Microsoft.ACE.OLEDB.12.0 OR Microsoft.Jet.OLEDB.4.0 in the code, we have to install redistributable as in step 1 above.
    
3.  To debug with access database engine, its important to install the correct version on local/ development machine.
    If MS office products are of 32-bit on machine, then install AccessDataBaseEngine targetting 32-bit and vice versa.
    If incorrect version is installed, then excel files on local will get corrupted and u can't open file.
    to resolve, uninstall access engine and restart machine.
    
    
    
sample code to read excel
if (Path.GetExtension(_requestedFileName) == ".xlsx")
                {
                    ExcelProcessConnectionString =
                     "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + _requestedFilePath + ";" +
                     "Extended Properties=Excel 12.0";
                }
                else
                {
                    ExcelProcessConnectionString =
                     "Provider=Microsoft.Jet.OLEDB.4.0;" +
                     "Data Source=" + _requestedFilePath + ";" +
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
                        string selectQuery = "Select [Category ID],[App ID],[DescriptionText1],[DescriptionText2],[DescriptionText3],[DescriptionText4],[DescriptionText5],[DueDate],[KPF_01], [KPF_02], [KPF_03],[KPF_04],[ActionURL],[ActionDescription],[ActionHelp]  from [" + strSHeetName + "]";

                        OleDbDataAdapter da = new OleDbDataAdapter(selectQuery, oledbConn);
                        da.SelectCommand = oledbCmd;
                        da.SelectCommand.CommandText = selectQuery;
                        da.SelectCommand.CommandType = CommandType.Text;

                        DataTable dtActual = new DataTable();

                        da.Fill(dtActual);
                          .................................
                          .....................
                          
