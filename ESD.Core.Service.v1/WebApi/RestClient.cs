using Newtonsoft.Json;
using ESD.Core.Logging;
using System;
using System.Net;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ESD.Core.Service.WebApi
{
   public class RestClient : IRestClient
   {
      private readonly IConfiguration configuration;
      private readonly IHttpContextAccessor httpContextAccessor;

      private const string DEFAULT_API_NAME = "default";
      private const string SECTION_NAME = "WebApiConfiguration";
      private const string WEBAPI_ADDRESS_KEY = "Address";
      private const string WEBAPI_PROXY_KEY = "Proxy";

      private string baseAddress = string.Empty;
      private string baseProxy = string.Empty;

      public RestClient(
         IConfiguration configuration,
         IHttpContextAccessor httpContextAccessor
         )
      {
         this.configuration = configuration;
         this.httpContextAccessor = httpContextAccessor;
      }
      private void loadApSettings()
      {
         baseAddress = configuration.GetSection(SECTION_NAME)[WEBAPI_ADDRESS_KEY];
         baseProxy = configuration.GetSection(SECTION_NAME)[WEBAPI_PROXY_KEY];
      }

      public string GetBaseAddress
      {
         get
         {
            loadApSettings();
            return baseAddress;
         }
         set
         {
            baseAddress = value;
         }
      }

      public string GetBaseProxy
      {
         get
         {
            return baseProxy;
         }
         set
         {
            baseProxy = value;
         }
      }

      public T GetApi<T>(IWebApiMethod webApiMethod)
      {
         return generalGetApi<T>(DEFAULT_API_NAME, webApiMethod, null);
      }
      public T GetApi<T>(string apiNameKey, IWebApiMethod webApiMethod)
      {
         return generalGetApi<T>(apiNameKey, webApiMethod, null);
      }
      public T GetApi<T>(IWebApiMethod webApiMethod, object parameter)
      {
         return generalGetApi<T>(DEFAULT_API_NAME, webApiMethod, parameter);
      }
      public T GetApi<T>(string apiNameKey, IWebApiMethod webApiMethod, object parameter)
      {
         return generalGetApi<T>(apiNameKey, webApiMethod, parameter);
      }
      private T generalGetApi<T>(string apiNameKey, IWebApiMethod webApiMethod, object parameter)
      {

         T response = default(T);

         try
         {
            RestSharp.IRestClient client = createRestClient(apiNameKey,webApiMethod);

            RestSharp.IRestRequest request = createRequest(RestSharp.Method.GET, parameter);

            RestSharp.IRestResponse result = client.Execute(request);

            ServiceResponse serviceResponse = JsonConvert.DeserializeObject<ServiceResponse>(result.Content);

            if (serviceResponse.Success)
            {
               object data = serviceResponse.Data;

               if (data != null)
               {
                  object[] typeList = { typeof(string), typeof(int), typeof(bool), typeof(short), typeof(long), typeof(decimal), 
                     typeof(DateTime) };

                  if (typeList.Contains(typeof(T)))
                     response = (T)TypeUtil.ChangeType(data, typeof(T));
                  else
                     response = JsonConvert.DeserializeObject<T>(data.ToString());
               }
            }
            else
            {
               throw new Exception(serviceResponse.Message);
            }
         }
         catch (Exception ex)
         {
            Logger.Error(ex);
            throw;
         }
         Logger.InfoFinish();
         return response;
      }
      public ServiceResponse PostApi(IWebApiMethod webApiMethod)
      {
         return generalPostApi(DEFAULT_API_NAME, webApiMethod, null);
      }
      public ServiceResponse PostApi(string apiNameKey, IWebApiMethod webApiMethod)
      {
         return generalPostApi(apiNameKey, webApiMethod, null);
      }
      public ServiceResponse PostApi(IWebApiMethod webApiMethod, object parameter)
      {
         return generalPostApi(DEFAULT_API_NAME, webApiMethod, parameter);
      }
      public T PostApi<T>(IWebApiMethod webApiMethod, object parameter)
      {
         return generalPostApi<T>(DEFAULT_API_NAME, webApiMethod, parameter);
      }
      public ServiceResponse PostApi(string apiNameKey, IWebApiMethod webApiMethod, object parameter)
      {
         return generalPostApi(apiNameKey,webApiMethod, parameter);
      }
      public T PostApi<T>(string apiNameKey, IWebApiMethod webApiMethod, object parameter)
      {
         return generalPostApi<T>(apiNameKey, webApiMethod, parameter);
      }
      private ServiceResponse generalPostApi(string apiNameKey, IWebApiMethod webApiMethod, object parameter)
      {
         Logger.InfoStart();
         ServiceResponse response = null;
         try
         {
            RestSharp.IRestClient client = createRestClient(apiNameKey, webApiMethod);

            RestSharp.IRestRequest request = createRequest(RestSharp.Method.POST, parameter);

            RestSharp.IRestResponse result = client.Execute(request);

            if (result.StatusCode != HttpStatusCode.OK)
               throw new Exception(result.ErrorMessage);

            ServiceResponse serviceResponse = JsonConvert.DeserializeObject<ServiceResponse>(result.Content);
            if (serviceResponse.Success)
            {
               response = serviceResponse;
            }
            else
            {
               throw new Exception(serviceResponse.Message);
            }
         }
         catch (Exception ex)
         {
            Logger.Error(ex);
            throw;
         }
         Logger.InfoFinish();
         return response;
      }
      private T generalPostApi<T>(string apiNameKey, IWebApiMethod webApiMethod, object parameter)
      {
         Logger.InfoStart();

         T response = default(T);
         try
         {
            RestSharp.IRestClient client = createRestClient(apiNameKey, webApiMethod);

            RestSharp.IRestRequest request = createRequest(RestSharp.Method.POST, parameter);

            RestSharp.IRestResponse result = client.Execute(request);

            if (result.StatusCode != HttpStatusCode.OK)
               throw new Exception(result.ErrorMessage + "[StatusCode=" + result.StatusCode.ToString() 
                  + "\nClient.BaseUrl= " + client.BaseUrl.ToString()
                  + "]");

            switch (result.ContentType) {
               case MediaTypeNames.Application.Octet:
                  response = (T)Convert.ChangeType(result.RawBytes, typeof(T));
                  break;

               case MediaTypeNames.Application.Xml:
                  response = (T)Convert.ChangeType(result.RawBytes, typeof(T));
                  break;

               default:
                  ServiceResponse serviceResponse = JsonConvert.DeserializeObject<ServiceResponse>(result.Content);

                  if (serviceResponse.Success)
                  {
                     object data = serviceResponse.Data;

                     if (data != null)
                     {
                        object[] typeList = { typeof(string),typeof(int), typeof(bool), typeof(short), typeof(long), typeof(decimal), typeof(DateTime) };

                        if (typeList.Contains(typeof(T)))
                           response = (T)TypeUtil.ChangeType(data, typeof(T));
                        else
                           response = JsonConvert.DeserializeObject<T>(data.ToString());
                     }
                  }
                  else
                  {
                     throw new Exception(serviceResponse.Message);
                  }
                  break;
            }

         }
         catch (Exception ex)
         {
            Logger.Error(ex);
            throw;
         }
         Logger.InfoFinish();
         return response;
      }
      private RestSharp.IRestClient createRestClient(string apiNameKey, IWebApiMethod webApiMethod)
      {

         RestSharp.RestClient client = new RestSharp.RestClient(getApiSetting(apiNameKey,webApiMethod, true));
         var baseProxy = getApiSetting(apiNameKey,webApiMethod, false);
         if (!string.IsNullOrEmpty(baseProxy)) client.Proxy = new WebProxy(baseProxy);

         return client;
      }
      private RestSharp.IRestRequest createRequest(RestSharp.Method method, object parameter)
      {

         RestSharp.IRestRequest request = new RestSharp.RestRequest(method)
         {
            RequestFormat = RestSharp.DataFormat.Json
         };

         if (httpContextAccessor != null)
         {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            if (httpContext.User != null) {

               Claim claim = httpContext.User.Claims.Where(x => x.Type == "jwt").FirstOrDefault();
               if (claim != null) {
                  string token = claim.Value;
                  request.AddHeader("Authorization", string.Format("Bearer {0}", token));
               }
            }
         }
         request.AddHeader("Content-Type", "application/json; charset=utf-8");

         if (parameter != null) {

            if (method == RestSharp.Method.GET)
            {
               request.AddParameter("id", parameter);
            }

            if (method == RestSharp.Method.POST)
            {
               var settings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.IsoDateFormat };

               string stringJson = JsonConvert.SerializeObject(parameter, settings);
               request.AddParameter("application/json", stringJson, RestSharp.ParameterType.RequestBody);
               //request.AddJsonBody(parameter);
            }
         }

         return request;
      }
      private string getApiSetting(string apiNameKey, IWebApiMethod webApiMethod, bool typeSetting)
      {
         char[] delimiterRows = { ',' };
         char[] delimiterColumns = { '|' };

         string baseSetting = typeSetting ? GetBaseAddress : GetBaseProxy;
         string setting = string.Empty;

         if (string.IsNullOrEmpty(baseSetting))
            return string.Empty;

         string[] valueSettings = baseSetting.Split(delimiterRows);

         foreach (var value in valueSettings)
         {
            string[] columns = value.Split(delimiterColumns);
            if (columns.Length > 0)
            {
               if (columns[0] == apiNameKey)
               {
                  setting = columns[1];
                  break;
               }
            }
         }

         setting = typeSetting ? string.Concat(setting, webApiMethod.Name) : setting;

         return setting;
      }
   }
}