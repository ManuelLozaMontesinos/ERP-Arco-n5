namespace ESD.Core.Service.DTO
{
   public enum DTOState
   {
      Added = 0,
      Modified = 1,
      Deleted = 2,
      Unchanged = 3
   }
   public class DTOBase
   {
      public virtual int Id
      {
         get { return 0; }
      }
      public int PageSize { get; set; }
      public string OrderBy { get; set; }
      public DTOState DTOState { get; set; } = DTOState.Unchanged;
   }
}
