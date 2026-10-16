namespace ESD.Core.Data
{
   public class BaseEntity
   {
      public virtual int EntityId
      {
         get { return 0; }
      }
      public int PageSize { get; set; }
      public string OrderBy { get; set; }
   }
}
