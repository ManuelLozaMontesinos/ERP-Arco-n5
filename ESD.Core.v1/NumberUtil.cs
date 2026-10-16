namespace ESD.Core
{
   public static class NumberUtil
   {
      public static int WhenValueIsZero(int value)
      {

         if (value == 0)
            return -1;

         return value;
      }

   }
}
