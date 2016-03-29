*** follow these steps to use autofac with MVC 5 and Web API in tandem. that is to use with controllers and apicontrollers.

A. Install these nuget packages
   Install-Package Autofac.Mvc5
   Install-Package Autofac.WebApi5 -Pre

B. Create Bootstrapper.cs file in UI project.
public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacWebAPI();
        }
        private static void SetAutofacWebAPI()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()); //Register MVC Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()); //Register WebApi Controllers
          
            //register application dependencies below. check example below. The interface comes second.
            builder.RegisterType<TestRepositories>().As<ITest>().InstancePerApiRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));  //Set the MVC DependencyResolver
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);  //Set the WebApi DependencyResolver
        }
    }
    
C. register autofac in web.config
 <dependentAssembly>
        <assemblyIdentity name="Autofac" publicKeyToken="17863af14b0044da" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-3.4.0.0" newVersion="3.4.0.0" />
      </dependentAssembly>

D. Call it from global.asax
protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // WebApi Configuration to hook up formatters and message handlers
            GlobalConfig.CustomizeConfig(GlobalConfiguration.Configuration);

            Bootstrapper.Run();//Initialise dependency resolvers

        }
