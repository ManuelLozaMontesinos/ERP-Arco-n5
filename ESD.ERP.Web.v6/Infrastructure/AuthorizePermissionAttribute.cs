using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Infrastructure
{
   public class PermissionConstant
   {
      public const string Create = "C";
      public const string Read = "R";
      public const string Update = "U";
      public const string Delete = "D";
   }


   //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
   public class AuthorizePermissionAttribute : Attribute //, IAuthorizationFilter
   {
      private readonly string resourceName;
      private readonly List<string> requiredPermissionList;
      public AuthorizePermissionAttribute()
      {
      }
      public AuthorizePermissionAttribute(string resourceName, string requiredPermission)
      {
         this.resourceName = resourceName;
         requiredPermissionList = new List<string>() { requiredPermission };
      }
      public AuthorizePermissionAttribute(string resourceName, List<string> requiredPermissionList)
      {
         this.resourceName = resourceName;
         this.requiredPermissionList = requiredPermissionList;
      }
      public void OnAuthorization(AuthorizationFilterContext context)
      {
         context.Result = new UnauthorizedResult();
      }
   }
}