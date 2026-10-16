using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ESD.Core.ObjectMapping
{
   internal static class CustomMapper
   {
      private static string DTO_SUFFIX = "DTO";

      public static T Map<T>(object source)
      {
         if (source == null)
            return default(T);

         T target = Activator.CreateInstance<T>();

         IList sourceList = source as IList;
         if (sourceList == null)
            copySourceToTarget(source, target, !source.GetType().Name.EndsWith(DTO_SUFFIX));
         else {

            for (int i = 0; i < sourceList.Count; i++)
            {
               object newTarget = Activator.CreateInstance(target.GetType().GetGenericArguments()[0]);
               copySourceToTarget(sourceList[i], newTarget, !sourceList[i].GetType().Name.EndsWith(DTO_SUFFIX));
               target.GetType().GetMethod("Add").Invoke(target, new[] { newTarget });
            }
         }

         return target;
      }
      private static object copySourceToTarget(object source, object target, bool domainIsSource)
      {
         Type sourceType = source.GetType();
         Type targetType = target.GetType();

         List<PropertyInfo> sourcePropertyList = new List<PropertyInfo>(sourceType.GetProperties().Where(x => x.CanWrite == true));
         List<PropertyInfo> targetPropertyList = new List<PropertyInfo>(targetType.GetProperties().Where(x => x.CanWrite == true));

         foreach (PropertyInfo sourceProperty in sourcePropertyList)
         {

            string propertyName = string.Empty;
            bool isUserDefined = false;

            if (!sourceProperty.PropertyType.Namespace.StartsWith("System"))
            {
               isUserDefined = true;
               if(domainIsSource)
                  propertyName = string.Concat(sourceProperty.Name, DTO_SUFFIX);
               else
                  propertyName = sourceProperty.Name.Remove(sourceProperty.Name.Length - DTO_SUFFIX.Length);
            }
            else
               propertyName = sourceProperty.Name;

            PropertyInfo targetProperty = targetPropertyList.FirstOrDefault(x => x.Name == propertyName);
            if (targetProperty == null)
               continue;

            object propertyValue = sourceProperty.GetValue(source, null);

            if (isUserDefined && propertyValue != null)
            {
               object targetDTO = Activator.CreateInstance(targetProperty.PropertyType);
               propertyValue = copySourceToTarget(propertyValue, targetDTO, domainIsSource);
            }

            targetProperty.SetValue(target, propertyValue);
         }

         return target;
      }
   }
}
