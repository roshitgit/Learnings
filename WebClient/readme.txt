* downloading excel files from sharepoint using .net webclient
* this code can be added as part of an exe (console app) and put under windows task scheduler in win 2003/ 2008 servers.
* the account under which the exe runs should have full access in sharepoint and better have admin access on the server where its 
  scheduled to run at a certain time interval.
  
Gotchas: The code works fine on Windows 2003 server but does not execute on Windows 2008 server R2.  

Dim client As New WebClient()

        Try
            'Dim cc As New CredentialCache()
            'cc.Add(New Uri(docUrl), "NTLM", New NetworkCredential("<uid>", "<pass>", "<domain>"))
            'Dim nc As New NetworkCredential("<uid>", "<pass>", "<domain>")
            'client.Credentials = cc

            client.UseDefaultCredentials = True

            client.DownloadFile(docUrl, fileName)

            

        Catch ex As System.Net.WebException
            SendMail(ex.Message, "CheckIfSharePointFileExists: System.Net.WebException")
            Return False
        Catch ex As Exception
            SendMail(ex.Message, "CheckIfSharePointFileExists: System.Exception")
            Return False
        Finally
            If Not client Is Nothing Then
                client.Dispose()
            End If
        End Try
        Return True
        
** few more links
http://stackoverflow.com/questions/12212116/how-to-get-httpclient-to-pass-credentials-along-with-the-request
