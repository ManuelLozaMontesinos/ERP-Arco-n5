using ESD.ERP.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace ESD.ERP.WebApi.Configuration
{
   public class RepositoryDependencyInjection
   {
      public static void Initialize(IServiceCollection services)
      {
         Assembly assembly = typeof(IUsuarioRepository).Assembly;

         var registrationList =
            from type in assembly.GetExportedTypes()
            where type.IsInterface == false
            select new { Service = type.GetInterfaces().FirstOrDefault(), Implementation = type };

         foreach (var reg in registrationList)
            services.AddSingleton(reg.Service, reg.Implementation);
      }
   }
}