using ESD.Core.Service.Security;
using ESD.ERP.WebApi.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ESD.ERP.WebApi
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         //services.AddCors(options =>
         //{
         //   options.AddPolicy("esdERPPolicy",
         //   builder =>
         //   {
         //      // Not a permanent solution, but just trying to isolate the problem
         //      builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
         //   });
         //});
         services.AddCors();
         services.AddControllers();

         IConfigurationSection configurationSection = Configuration.GetSection("TokenJwt");
         TokenJwtSettings tokenJwtSettings = configurationSection.Get<TokenJwtSettings>();

         services.AddAuthentication(x =>
         {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         })
        .AddJwtBearer(cfg =>
           {
              cfg.TokenValidationParameters = new TokenValidationParameters()
              {
                 ValidateIssuer = true,
                 ValidIssuer = tokenJwtSettings.IssuerToken,
                 ValidateAudience = true,
                 ValidAudience = tokenJwtSettings.AudienceToken,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenJwtSettings.SecretKey)),
              };
           });

         services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
         services.AddSingleton<ITokenGenerator, TokenGenerator>();

         BusinessDependencyInjection.Initialize(services);
         RepositoryDependencyInjection.Initialize(services);

      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseRouting();
         //app.UseCors("esdERPPolicy");
         app.UseCors(x => x
                         .AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());

         app.UseAuthentication();
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });
      }
   }
}