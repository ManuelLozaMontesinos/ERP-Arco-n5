namespace ESD.Core
{
   public class AppConstante
   {
      public static readonly string DTO_STATE = "DTOState";
      public static readonly int STATUS_CODE_END_SESSION = 999;
   }
   public class VariableSesionConstante
   {
      public const string USUARIO_ID = "_userId";
      public const string USUARIO_LOGON = "_userLogon";
      public const string PAIS_EMPRESA_ID = "_paisEmpresaId";
   }
   public class WebConfigConstante
   {
      public const string MAX_REG_BUSQUEDA = "app:maxRegistrosBusqueda";
   }

   public class SeparadorConstante
   {
      public static readonly string GUION = "-";
      public static readonly string SUB_GUION = "-";
      public static readonly string ESPACIO = " ";
      public static readonly string DOS_PUNTOS = ":";
      public static readonly string PUNTO = ".";
      public static readonly string COMA = ",";
      public static readonly string BARRA_OBLICUA = "/";
      public static readonly string PUNTO_COMA = ";";
   }
    public class StringFormatConstant
    {
        public const string DATE = "{0:dd/MM/yyyy}";
        public const string DECIMAL = "{0:0.00}";
        public const string INTEGER = "{0:0}";
        public const string TIME = "{0:hh:mm:ss}";
        public const string TIME_HOUR_24H = "{0:HH:mm}";
        public const string DATE_TIME = "{0:dd/MM/yyyy HH:mm:ss}";
        public const string DATE_ISO = "{0:yyyyMMdd}";
        public const string DATE_MONTH = "{0:MM}";
        public const string DATE_YEAR = "{0:yyyy}";
    }
    public class JavaScriptConstant
    {
        public const string DATE_FORMAT = "DD/MM/YYYY";
        public const string TIME_FORMAT = "HH:mm:ss";
        public const string DATE_TIME_FORMAT = "DD/MM/YYYY HH:mm:ss";
        public const string DATE_MONTH_FORMAT = "MM";
        public const string GRID_DATE_FORMAT = "d/m/Y";
        public const string GRID_DATE_TIME_FORMAT = "d/m/Y H:i:s";
    }
    public class DigitoConstante
   {
      public const string CERO = "0";
      public const string UNO = "1";
      public const string DOS = "2";
      public const string TRES = "3";
      public const string CUATRO = "4";
      public const string CINCO = "5";
      public const string SEIS = "6";
      public const string SIETE = "7";
      public const string OCHO = "8";
      public const string NUEVE = "9";
      public const string DIEZ = "10";
   }
   public class LetraConstante
   {
      public const string A = "A";
      public const string B = "B";
      public const string I = "I";
      public const string L = "L";
      public const string P = "P";
      public const string R = "R";
   }

   public class PartialViewNameConstant {
      public const string LIST = "Index";
      public const string CRUD = "CreateEdit";
   }

   public class ReportFormatConstant
   {
      public const string PDF = "PDF";
      public const string EXCEL = "EXCEL";
      public const string CSV = "CSV";
      public const string WORD = "WORD";
      public const string XML = "XML";
      public const string EXCELDATA = "EXCELDATA";
   }
   public class ReportExportExtensionConstant
   {
      public static readonly string PDF = ".pdf";
      public static readonly string EXCEL = ".xls";
      public static readonly string CSV = ".csv";
      public static readonly string WORD = ".doc";
      public static readonly string XML = ".xml";
      public static readonly string EXCELX = ".xlsx";
   }
   public class ReportExportNameConstant
   {
      public static readonly string DDMMYYY_FORMAT = "{0:ddMMyyyy}";
   }
   public class SSRSConstant
   {
      public const string URL_SEPARATOR = "&";
      public const string URL_EQUALS = "=";
      public const string URL_PARAMETER_NULL = ":IsNull=True";
   }
   public class SignoConstante {
      public const string PORCENTAJE = "%";
   }
}