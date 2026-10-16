using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ESD.Core.Data
{
   public static class RepositoryHelper
   {
      public static Dictionary<string, object> CreateParameterListForDynamicQuery(int pageSize, string orderBy) {

         Dictionary<string, object> parameters = new Dictionary<string, object>
         {
            { "PageSize", pageSize },
            { "OrderBy", orderBy }
         };

         return parameters;
      }
      //public static void AddParameter<T>(this Dictionary<string, object> parameters, Dictionary<string, object> source, string name) {

      //   if (!source.ContainsKey(name))
      //      return;

      //   var t = typeof(T);
      //   object value = Convert.ChangeType(source[name], Nullable.GetUnderlyingType(t) ?? t);

      //   parameters.Add(StringUtil.FirstLetterToLowerCase(name), value);
      //}
      public static void AddParameter(this Dictionary<string, object> parameters, string parameterClauseConstant, string name, object value)
      {
         parameters.Add(string.Concat(parameterClauseConstant,StringUtil.FirstLetterToUpperCase(name)), value);
      }

      public static void AddParameter(this Dictionary<string, object> parameters, string name, object value)
      {
         parameters.Add(StringUtil.FirstLetterToLowerCase(name), value);
      }
      public static void AddNewIdParameter(this Dictionary<string, object> parameters)
      {
         parameters.Add(string.Concat("newID"), 0);
      }
      public static void AddOutputParameter(this Dictionary<string, object> parameters, string name)
      {
         parameters.Add(string.Concat(StringUtil.FirstLetterToLowerCase(name),"_output"), null);
      }
      public static void AddTableParameter<T>(this Dictionary<string, object> parameters, string name, string sqlTypeName, List<T> list)
      {

         DataTable dataTable = new DataTable();
         PropertyInfo[] properties = typeof(T).GetRuntimeProperties().ToArray();

         foreach (PropertyInfo propertyInfo in properties)
         {
            if (propertyInfo.SetMethod == null)
               continue;

            dataTable.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
         }

         foreach (T item in list)
         {
            List<object> valueList = new List<object>();
            properties = typeof(T).GetRuntimeProperties().ToArray();

            foreach (PropertyInfo propertyInfo in properties)
            {
               if (propertyInfo.SetMethod == null)
                  continue;

               object value = propertyInfo.GetValue(item);
               valueList.Add(value);
            }

            dataTable.Rows.Add(valueList.ToArray());
         }

         ParameterStructure parameterStructure = new ParameterStructure() {
            Key = sqlTypeName,
            Value = dataTable
         }; 

         parameters.Add(StringUtil.FirstLetterToLowerCase(name), parameterStructure);
      }
      public static void AddParameter(this Dictionary<string, object> parameters, Expression<Func<object>> expression)
      {
         PropertyInfo propertyInfo;

         if (expression.Body is UnaryExpression)
         {
            propertyInfo = ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member as PropertyInfo;
         }
         else
         {
            propertyInfo = ((MemberExpression)expression.Body).Member as PropertyInfo;
         }
      }

   }
}
