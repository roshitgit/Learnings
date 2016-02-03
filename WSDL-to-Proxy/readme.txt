****** USING WSDL Utility

1. Open command prompt with admin priveleges
2. Go to "c:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools"
3. Type command as below
	wsdl /out:<cs file path> <WSDL URL>
   Refer => Using WSDL => 
			https://msdn.microsoft.com/en-us/library/7h3ystb6.aspx
	
4.   Refer => Using CSC to compile proxy into .DLL => 
			http://gsraj.tripod.com/dotnet/webservices/webservice_csharp_client.html



This way of using WSDL utlity and compiling the generated proxy class into a .dll library comes into play when
you wish to use the api's exposed by a 3rd party service. ex: ServiceNow or TFS (team foundation Server).

Then in most of the cases, ServiceNow or TFS development support team will provide a WSDL or SOAP url of their API.
If WSDL is provided, then we can generate a proxy class using .NET 4/4.5 WSDL tool which is part of the SDK.
Once proxy class is available, we cna easilyt compile the same into a .dll using .NET CSC utility.

compiling into a .dll library is important so that it can be used to call the api's for testing in other projects.
projects can then be a console app, website or web project where the dll can be added as reference.



Examples:
WSDL:
Go to path => c:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools from command prompt
wsdl /out:<local solution folder>\<child folder in solution>\ServiceNowProdChangeApi.cs https://servicemanagement.citigroup.net/ChangeQuery.do?WSDL


CSC compiler:
Go to path => c:\Windows\Microsoft.NET\Framework64\v4.0.30319\ from command prompt
Format: csc /t:library /out:<local path to create dll> /r:System.Web.Services.dll /r:System.Xml.dll <local path of proxy class>

Example:
csc /t:library /out:<local solution folder>\<local assemblies folder>\ ServiceNowProdChangeApi.dll /r:System.Web.Services.dll /r:System.Xml.dll <local solution folder>\<child folder in solution>\ <.cs class>






