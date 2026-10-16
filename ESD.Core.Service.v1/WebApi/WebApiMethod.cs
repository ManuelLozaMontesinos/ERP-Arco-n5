namespace ESD.Core.Service.WebApi
{
   public class MethodNameConstant
   {
      public const string SINGLE_SEARCH = "SingleSearch";
      public const string SEARCH = "Search";
      public const string GET_BY_ID = "GetById";
      public const string INSERT = "Insert";
      public const string UPDATE = "Update";
      public const string DELETE = "Delete";
      public const string GET_BY_CODE = "ObtenerPorCodigo";
   }

   public class WebApiMethod : IWebApiMethod
   {
      private string name = string.Empty;
      private const string DEFAULT_URL_PATH_SEGMENT = "api";
      private const string URL_SEPARATOR = "/";

      public string Name
      {
         get
         {
            return name;
         }
      }

      private void setName(string name)
      {
         this.name = name;
      }

      public static IWebApiMethod Create(string name)
      {
         WebApiMethod webApiMethod = new WebApiMethod();
         webApiMethod.setName(name);
         return webApiMethod;
      }
      public static IWebApiMethod Create(string controllerName, MethodNameConstant methodName)
      {
         return Create(DEFAULT_URL_PATH_SEGMENT, controllerName, methodName.ToString());
      }
      public static IWebApiMethod Create(string controllerName, string methodName)
      {
         return Create(DEFAULT_URL_PATH_SEGMENT, controllerName, methodName);
      }
      public static IWebApiMethod Create(string pathSegment, string controllerName, string methodName)
      {
         WebApiMethod webApiMethod = new WebApiMethod();
         webApiMethod.setName(string.Concat(pathSegment, URL_SEPARATOR, controllerName, URL_SEPARATOR, methodName));

         return webApiMethod;
      }

   }
}
