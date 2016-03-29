https://rmanimaran.wordpress.com/2010/06/24/creating-and-using-c-web-service-over-https-%E2%80%93-ssl-2/
https://msdn.microsoft.com/en-us/library/ff648667.aspx
http://stackoverflow.com/questions/770345/asmx-username-and-password-security

http://stackoverflow.com/questions/10861568/asmx-web-service-basic-authentication
http://www.codeguru.com/csharp/csharp/cs_webservices/security/article.php/c5479/Build-Secure-Web-Services-With-SOAP-Headers-and-Extensions.htm

***check these
http://www.c-sharpcorner.com/UploadFile/8ef97c/web-service-in-Asp-Net-security-by-soap-authentication-pa/----check this
http://stackoverflow.com/questions/770345/asmx-username-and-password-security
http://blog.gauffin.org/2012/12/05/solved-getting-401-unauthorized-while-calling-an-asmx-service-using-windows-authentication/

***Configure an XML Web Service for Windows Authentication
https://msdn.microsoft.com/en-us/library/bfazk0tb.aspx


*********

//turn off Certificate validation
        ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate certificate, X509Chain chain,
                     SslPolicyErrors sslPolicyErrors) => true;

        SecurityBindingElement securityElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
        securityElement.IncludeTimestamp = false;
        TextMessageEncodingBindingElement encodingElement = new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8);
        HttpsTransportBindingElement tranportElement = new HttpsTransportBindingElement();

        CustomBinding customBinding = new CustomBinding(securityElement, encodingElement, tranportElement);

        EndpointAddress address = new EndpointAddress("https://<url>?wsdl");
        
        <proxy>.class instance = new <proxy>.class(customBinding, address);
        instance.ClientCredentials.UserName.UserName = "admin";
        instance.ClientCredentials.UserName.Password = "admin";

        instance.Endpoint.Behaviors.Add(new CustomEndpointBehavior());

        var orderResult = instance.<method>("<params>");
        
