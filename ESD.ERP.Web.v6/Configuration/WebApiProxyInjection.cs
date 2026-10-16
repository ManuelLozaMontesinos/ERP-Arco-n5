using ESD.Core.Service.WebApi;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Reflection;
using Unity;
using Unity.Extension;

namespace ESD.ERP.Web.Configuration
{
   public class WebApiProxyInjection : UnityContainerExtension
   {
      protected override void Initialize()
      {

         Container.RegisterType<IRestClient, RestClient>();
         Container.RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>();

         Assembly assembly = typeof(IUsuarioProxy).Assembly;

         var registrationList =
            from type in assembly.GetExportedTypes()
            where type.GetInterfaces().Any()
            select new { Service = type.GetInterfaces().Single(), Implementation = type };

         foreach (var reg in registrationList)
            Container.RegisterType(reg.Service, reg.Implementation);
      }
   }
}
