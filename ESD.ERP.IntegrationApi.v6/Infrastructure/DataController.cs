using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Xml.Linq;

namespace ESD.ERP.IntegrationApi.Infrastructure
{
   public class DataController : ApiController
   {
      private string AppWebDirectory
      {
         get
         {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string directory = Path.GetDirectoryName(path);
            int i = directory.IndexOf("bin");
            if (i >= 0)
               directory = directory.Substring(0, i);

            return directory;
         }
      }
      public SqlConnection CreateERPConnection()
      {
         return CreateERPConnection("default");
      }
      public SqlConnection CreateERPConnection(string name)
      {
         string connectionString = string.Empty;

         string directoryName = AppWebDirectory;
         string xmlFile = Path.Combine(directoryName, "connectionStrings.xml");

         if (System.IO.File.Exists(xmlFile))
         {
            XDocument xDocument = XDocument.Load(xmlFile);

            XElement xElement = (from item in xDocument.Descendants("connectionString")
                                 where item.Attribute("name").Value == name
                                 select item).FirstOrDefault();

            if (xElement != null)
            {
               connectionString = xElement.LastAttribute.Value;
            }
            xDocument = null;
         }

         SqlConnection sqlConnection = new SqlConnection(connectionString);
         sqlConnection.Open();

         return sqlConnection;
      }
      public static SqlParameter CreateParameter(string paramName, SqlDbType dataType, int dataSize, object dataValue)
      {
         SqlParameter sqlParameter = new SqlParameter(paramName, dataType, dataSize);
         sqlParameter.Direction = ParameterDirection.Input;
         sqlParameter.Value = dataValue;
         return sqlParameter;
      }
      public static SqlParameter CreateParameter(string paramName, SqlDbType dataType, object dataValue)
      {
         SqlParameter sqlParameter = new SqlParameter(paramName, dataType);
         sqlParameter.Direction = ParameterDirection.Input;
         sqlParameter.Value = dataValue;
         return sqlParameter;
      }
      public static SqlParameter CreateParameter(string paramName, object dataValue)
      {
         SqlParameter sqlParameter = new SqlParameter(paramName, dataValue);
         sqlParameter.Direction = ParameterDirection.Input;
         return sqlParameter;
      }
   }
}