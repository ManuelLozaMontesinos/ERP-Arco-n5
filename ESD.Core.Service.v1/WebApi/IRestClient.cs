namespace ESD.Core.Service.WebApi
{
   public interface IRestClient
   {
      T GetApi<T>(IWebApiMethod webApiMethod);
      T GetApi<T>(string apiNameKey,IWebApiMethod webApiMethod);
      T GetApi<T>(IWebApiMethod webApiMethod, object requestBodyObject);
      T GetApi<T>(string apiNameKey,IWebApiMethod webApiMethod, object requestBodyObject);
      ServiceResponse PostApi(IWebApiMethod webApiMethod);
      ServiceResponse PostApi(string apiNameKey, IWebApiMethod webApiMethod);
      ServiceResponse PostApi(IWebApiMethod webApiMethod, object requestBodyObject);
      T PostApi<T>(IWebApiMethod webApiMethod, object requestBodyObject);
      ServiceResponse PostApi(string apiNameKey, IWebApiMethod webApiMethod, object requestBodyObject);
      T PostApi<T>(string apiNameKey, IWebApiMethod webApiMethod, object requestBodyObject);
   }
}