using ESD.ERP.Web.Areas.FE.Service;
using ESD.ERP.Web.Configuration;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Text.Json.Serialization;
using Unity;

namespace ESD.ERP.Web
{
   public class Startup
   {
      public void ConfigureServices(IServiceCollection services)
      {
         //services.AddIdentity<AppIdentityUser, AppIdentityRole<string>>()
         //   .AddDefaultTokenProviders();

         services.AddMvc()
            .AddControllersAsServices()
            .AddJsonOptions(opts =>
            {
               opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

         services.AddControllersWithViews(options =>
            options.ModelBinderProviders.RemoveType<DateTimeModelBinderProvider>()).AddRazorRuntimeCompilation();
         services.AddRazorPages();

         services.AddAuthentication(options =>
         {
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
         })
         .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
         {
            config.Cookie.HttpOnly = true;
            config.Cookie.Name = "erpv6sec";
            config.LoginPath = "/Login";
            //config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            //config.SlidingExpiration = false;
         });

         services.AddSignalR();
         services.AddHostedService<CalendarEventReminderBackgroundService>();

      }
      public void ConfigureContainer(IUnityContainer container)
      {
         //container.RegisterType<UserManager<AppIdentityUser>, AppUserManager>();
         //container.RegisterType<IUserStore<AppIdentityUser>, AppUserStore<AppIdentityUser>>();
         //container.RegisterType<IRoleStore<AppIdentityRole<string>>, AppRoleStore<AppIdentityRole<string>>>();

         container.AddNewExtension<WebApiProxyInjection>();
         container.RegisterType<AppHelper, AppHelper>();
         container.RegisterType<IFacturacionService, FacturacionService>();
         container.RegisterType<INotifyService, NotifyService>();

      }
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         var supportedCultures = new[]
         {
            new CultureInfo("es-PE")
         };

         app.UseRequestLocalization(new RequestLocalizationOptions
         {
            DefaultRequestCulture = new RequestCulture("es-PE"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
         });

         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         RewriteOptions rewriteOptions = new RewriteOptions()
            .AddRedirect(@"^Home?cid=(.*)", "Home/$1");

         app.UseRewriter(rewriteOptions);

         app.UseStaticFiles(new StaticFileOptions()
         {
            OnPrepareResponse = context =>
            {
               context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
               context.Context.Response.Headers.Add("Expires", "-1");
            }
         });
         app.UseRouting();

         app.UseAuthentication();
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapRazorPages();

            endpoints.MapControllerRoute("pf", "pf",
               defaults: new { controller = "PerfilAcceso", action = "Index" });

            endpoints.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
               name: "default",
               pattern: "{controller=Login}/{action=Index}/{id?}");

            endpoints.MapHub<NotifyHub>("/NotifyHub");
         });

      }
   }
}
