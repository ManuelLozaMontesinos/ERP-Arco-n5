namespace ESD.Core.Data
{
   public class ParameterClauseConstant
   {
      public const string CONTAINS = "contains";
      public const string IS_NULL = "isNull";
      public const string NOT_NULL = "notNull";
   }
   public class StoredProcActionConstant
   {
      public const string SEARCH = "_buscarPor";
      public const string GET_BY_ID = "_porId";
      public const string GET_BY_CODE = "_porCodigo";
      public const string INSERT = "_insertar";
      public const string UPDATE = "_actualizar";
      public const string ESTADO = "_actualizarEstado";
      public const string DELETE = "_eliminar";
      public const string SOLICITAR = "_solicitar";
      public const string APROBAR = "_aprobar";
      public const string DESAPROBAR = "_desaprobar";
   }
}