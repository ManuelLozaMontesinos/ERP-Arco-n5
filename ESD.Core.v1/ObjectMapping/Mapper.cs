using ESD.Core.ObjectMapping;

namespace ESD.Core
{
   public static class Mapper
   {
      public static T Map<T>(object source)
      {
         return CustomMapper.Map<T>(source);
      }
   }
}
