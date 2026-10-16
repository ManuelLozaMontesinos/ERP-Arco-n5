using System.Data;

namespace ESD.Core.Data
{
   public class DbCommand
   {
      private string name = string.Empty;

      public string Name {
         get { return name; }
      }
      public int? Timeout { get; set; }
      public CommandType CommandType { get; set; } = CommandType.StoredProcedure;
      private void setName(string name)
      {
         this.name = name;
      }
      public static DbCommand Create(string name)
      {
         DbCommand dbCommand = new DbCommand();
         dbCommand.setName(name);

         return dbCommand;
      }

      public static DbCommand Create(string storedProcPreName, string storedProcActionConstant)
      {
         return Create(string.Concat(storedProcPreName, storedProcActionConstant));
      }
   }
}
