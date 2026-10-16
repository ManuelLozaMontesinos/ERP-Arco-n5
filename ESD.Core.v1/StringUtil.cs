namespace ESD.Core
{
   public static class StringUtil
   {
      public static string FirstLetterToLowerCase(string s)
      {
         if (string.IsNullOrEmpty(s))
            return string.Empty;

         char[] c = s.ToCharArray();
         c[0] = char.ToLower(c[0]);
         return new string(c);
      }
      public static string FirstLetterToUpperCase(string s)
      {
         if (string.IsNullOrEmpty(s))
            return string.Empty;

         char[] c = s.ToCharArray();
         c[0] = char.ToUpper(c[0]);
         return new string(c);
      }

   }
}
