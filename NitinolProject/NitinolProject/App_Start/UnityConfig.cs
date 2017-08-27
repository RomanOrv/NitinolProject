using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using System.Configuration;
using NitinolProject.Repository.Interfaces;
using NitinolProject.Repository;

namespace NitinolProject.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            string connectionString = ConfigurationManager.ConnectionStrings["assessment_quality_materialsEntities"].ConnectionString;
            container.RegisterType<IAlloyRepository, AlloyRepository>(new InjectionConstructor(connectionString));
            container.RegisterType<IMetalRepository, MetalRepository>(new InjectionConstructor(connectionString));
            container.RegisterType<IPmmaRepository, PmmaRepository>(new InjectionConstructor(connectionString));
            container.RegisterType<ISpheroplastRepository, SpheroplastRepository>(new InjectionConstructor(connectionString));
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}