using ESD.ERP.Business;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace ESD.ERP.WebApi.Configuration
{
   public class BusinessDependencyInjection
   {
      public static void Initialize(IServiceCollection services)
      {
         Assembly assembly = typeof(IUsuarioBusiness).Assembly;

         var registrationList =
            from type in assembly.GetExportedTypes()
            where type.GetInterfaces().Any()
            select new { Service = type.GetInterfaces().Single(), Implementation = type };

         foreach (var reg in registrationList)
            services.AddSingleton(reg.Service, reg.Implementation);
      }
   }
}