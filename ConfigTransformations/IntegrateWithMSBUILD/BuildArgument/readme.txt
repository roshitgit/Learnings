/p:DeployOnBuild=True will ask the TFS build agent to deploy the build to the server when the process succeeds.
/p:DeployTarget=MsDeployPublish will notify the build server to execute the Microsoft deployment process.
/p:MSDeployPublishMethod=RemoteAgent will ask the build server to run the remote agent for the deployment.
/p:CreatePackageOnPublish=True will create a package of the current build for deployment.
/p:DeployIisAppPath=”Default Web Site/MyBlog” will deploy the package to the “MyBlog” web application. Change this value based on your webserver configuration. Make sure that the application is already created in the IIS.
/p:MsDeployServiceUrl=kunal-chowdhury.com is the Server URL where you want to deploy your application. Make sure to change the value with your server’s domain name.
/p:username=kunal-chowdhury.com\Webmaster is the administrative account of your server. Change this value with proper username. You should specify it in this format: “DOMAIN\USERNAME”.
/p:password=MyPassword@1234 is where you have to specify the password of the administrative user account (i.e. the username that you specified)

Read more at http://www.kunal-chowdhury.com/2013/05/how-to-auto-deploy-after-build-in-tfs.html#sRugjHYVyejOPYEK.99
