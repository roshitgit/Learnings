using System;
using System.Web;
using System.Web.Http;
using Microsoft.Practices.Unity;
all app dll's below
...
..
..
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace <>
{
    /// <summary>
    /// 
    /// </summary>
    public class UnityResolver : IDependencyResolver
    {
        protected IUnityContainer container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }

    
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise(HttpConfiguration config)
        {
            var container = BuildUnityContainer();
            config.DependencyResolver = new UnityResolver(container);
            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        }

        /// <summary>
        /// Register all dependencies here as singleton instances.
        /// </summary>
        /// <param name="container">MS Unity Container</param>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.BindInRequestScope<ITest, Test>();
            
        }
    }
}


public static class IOCExtensions
    {
        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            //note: This version of Unity.MVC4 with "HierarchicalLifetimeManager" calls the dispose() method of the object
            // without any issues after resolving.
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }

        public static void BindInSingletonScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new ContainerControlledLifetimeManager());
        }
    }
