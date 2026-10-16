using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ESD.Core.Logging;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace ESD.Core.Data
{
   public static class DbFactory
   {
      public static int ExecuteCommand(string commandText, Dictionary<string,object> parameters) {

         int rows = 0;

         DynamicParameters dynamicParameters = convertDictionaryParametersToDynamic(parameters);

         logCommandInterceptor(commandText, dynamicParameters);

         Logger.Info("CommandText " + commandText + " :: Start Execution");

         using (SqlConnection sqlConnection = createConnection())
         {
            rows = sqlConnection.Execute(commandText, param: dynamicParameters, commandType: CommandType.StoredProcedure);
         }

         Logger.Info("CommandText " + commandText + " :: Finish Execution");
         return rows;
      }
      public static int ExecuteCommand(string commandText, Dictionary<string, object> parameters, int timeOut)
      {

         int rows = 0;

         DynamicParameters dynamicParameters = convertDictionaryParametersToDynamic(parameters);

         logCommandInterceptor(commandText, dynamicParameters);

         Logger.Info("CommandText " + commandText + " :: Start Execution");

         using (SqlConnection sqlConnection = createConnection())
         {
            rows = sqlConnection.Execute(commandText, param: dynamicParameters, commandType: CommandType.StoredProcedure, commandTimeout: timeOut);
         }

         Logger.Info("CommandText " + commandText + " :: Finish Execution");
         return rows;
      }

      private static string AppWebDirectory
      {
         get
         {
            string codeBase = Assembly.GetExecutingAssembly().Location;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string directory = Path.GetDirectoryName(path);
            int i = directory.IndexOf("bin");
            if (i >= 0)
               directory = directory.Substring(0, i);

            return directory;
         }
      }
      public static string GetConnectionString()
      {
         string connectionString = string.Empty;

         string directoryName = AppWebDirectory;
         string xmlFile = Path.Combine(directoryName, "connectionStrings.xml");

         if (File.Exists(xmlFile))
         {
            XDocument xDocument = XDocument.Load(xmlFile);

            XElement xElement = (from item in xDocument.Descendants("connectionString")
                                 where item.Attribute("name").Value == "default"
                                 select item).FirstOrDefault();

            if (xElement != null)
            {
               connectionString = xElement.LastAttribute.Value;
            }
            xDocument = null;
         }

         return connectionString;
      }
      private static SqlConnection createConnection()
      {
         string connectionString = GetConnectionString();

         SqlConnection sqlConnection = new SqlConnection(connectionString);
         sqlConnection.Open();

         return sqlConnection;
      }
      private static SqlParameter createParameter(string paramName, object dataValue)
      {
         SqlParameter sqlParameter = new SqlParameter(paramName, dataValue)
         {
            Direction = ParameterDirection.Input
         };

         return sqlParameter;
      }
      public static T Query<T>(DbCommand dbCommand, int id)
      {

         Dictionary<string, object> parameters = new Dictionary<string, object>
         {
            { "id", id }
         };

         List<T> list = Query<T>(dbCommand, parameters);

         if (list == null || list.Count == 0)
            return default;

         return list[0];
      }
      public static T Query<T>(DbCommand dbCommand, int id, Type[] types, Func<object[], T> map, string splitOn)
      {

         Dictionary<string, object> parameters = new Dictionary<string, object>
         {
            { "id", id }
         };

         List<T> list = Query(dbCommand, parameters,types,map,splitOn);

         if (list == null || list.Count == 0)
            return default;

         return list[0];
      }
      public static List<T> Query<T>(DbCommand dbCommand, Dictionary<string, object> parameters)
      {

         DynamicParameters dynamicParameters = convertDictionaryParametersToDynamic(parameters);

         SqlMapper.SetTypeMap(typeof(T), new CustomPropertyTypeMap(
             typeof(T), (type, columnName) => type.GetProperties().FirstOrDefault(prop =>
             prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));

         logCommandInterceptor(dbCommand.Name, dynamicParameters);

         Logger.Info("CommandText " + dbCommand.Name + " :: Start Execution");

         List<T> list = null;
         using (SqlConnection sqlConnection = createConnection())
         {
            list = sqlConnection.Query<T>(dbCommand.Name, param: dynamicParameters,commandTimeout: dbCommand.Timeout,
               commandType: dbCommand.CommandType).ToList();
         }

         Logger.Info("CommandText " + dbCommand.Name + " :: Finish Execution");
         return list;
      }
      public static List<T> Query<T>(DbCommand dbCommand, Dictionary<string, object> parameters, Type[] types, Func<object[],T> map, 
         string splitOn)
      {

         DynamicParameters dynamicParameters = convertDictionaryParametersToDynamic(parameters);

         foreach (Type item in types)
         {
            SqlMapper.SetTypeMap(item, new CustomPropertyTypeMap(
                item, (type, columnName) => type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
         }

         logCommandInterceptor(dbCommand.Name, dynamicParameters);

         Logger.Info("CommandText " + dbCommand.Name + " :: Start Execution");
         List<T> list = null;
         using (SqlConnection sqlConnection = createConnection())
         {
            list = sqlConnection.Query(dbCommand.Name, types,map, param: dynamicParameters, splitOn: splitOn, 
               commandTimeout: dbCommand.Timeout, commandType: dbCommand.CommandType).ToList();
         }
         Logger.Info("CommandText " + dbCommand.Name + " :: Finish Execution");
         return list;
      }
      public static T ExecuteScalar<T>(DbCommand dbCommand, Dictionary<string, object> parameters)
      {

         DynamicParameters dynamicParameters = convertDictionaryParametersToDynamic(parameters);

         logCommandInterceptor(dbCommand.Name, dynamicParameters);

         Logger.Info("CommandText " + dbCommand.Name + " :: Start Execution");

         object singleValue = null;
         using (SqlConnection sqlConnection = createConnection())
         {
            singleValue = sqlConnection.ExecuteScalar<T>(dbCommand.Name, param: dynamicParameters, commandType: CommandType.StoredProcedure);
         }

         Logger.Info("CommandText " + dbCommand.Name + " :: Finish Execution");

         if (singleValue == null)
            return default;

         return (T)singleValue;
      }
      public static int ExecCommand(DbCommand dbCommand, Dictionary<string, object> parameters)
      {

         DynamicParameters dynamicParameters = convertDictionaryParametersToDynamic(parameters);

         logCommandInterceptor(dbCommand.Name, dynamicParameters);

         Logger.Info("CommandText " + dbCommand.Name + " :: Start Execution");

         int newID = 0;
         using (SqlConnection sqlConnection = createConnection())
         {
            sqlConnection.Query<int>(dbCommand.Name, param: dynamicParameters, commandType: dbCommand.CommandType).ToList();
         }

         Logger.Info("CommandText " + dbCommand.Name + " :: Finish Execution");

         if (dynamicParameters.ParameterNames.Contains("newID"))
            newID = dynamicParameters.Get<int>("newID");

         return newID;
      }
      public static DataTable ExecCommandWithResult(string commandText, Dictionary<string, object> parameters)
      {
         DataTable dataTable = new DataTable();

         using (SqlConnection sqlConnection = createConnection())
         {
            using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
            {

               sqlCommand.Parameters.Clear();
               sqlCommand.Parameters.AddRange(convertDictionaryParametersToSqlParameter(parameters));
               sqlCommand.CommandType = CommandType.StoredProcedure;
               sqlCommand.CommandText = commandText;

               Logger.Info("CommandText " + commandText + " :: Start Execution");

               using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
               {
                  dataTable.Load(dataReader);
               }

            }
         }

         Logger.Info("CommandText " + commandText + " :: Finish Execution");

         return dataTable;
      }
      internal static DynamicParameters convertDictionaryParametersToDynamic(Dictionary<string, object> parameters) {

         DynamicParameters dynamicParameters = new DynamicParameters();

         foreach (KeyValuePair<string, object> keyValuePair in parameters)
         {
            object value = keyValuePair.Value;
            string parameterName = StringUtil.FirstLetterToLowerCase(keyValuePair.Key);

            if (value == null)
               continue;

            Type type = value.GetType();
            if (type.IsGenericType)
            {
               string sqlTypeName = string.Empty;
               DataTable dataTable = null;
               int totalArg = type.GetGenericArguments().Count();

               if (totalArg != 0)
               {

                  Type argumentType = type.GetGenericArguments()[0];

                  if (argumentType == typeof(int))
                  {
                     List<int> list = (List<int>)value;
                     sqlTypeName = "ListInt";
                     dataTable = new DataTable();
                     dataTable.Columns.Add("value", typeof(int));
                     foreach (int i in list)
                        dataTable.Rows.Add(i);
                  }

                  if (argumentType == typeof(string))
                  {
                     List<string> list = (List<string>)value;
                     sqlTypeName = "ListString";
                     dataTable = new DataTable();
                     dataTable.Columns.Add("value", typeof(string));
                     foreach (string s in list)
                        dataTable.Rows.Add(s);
                  }

                  value = dataTable.AsTableValuedParameter(sqlTypeName);
               }
            }

            if (type == typeof(ParameterStructure)) {
               ParameterStructure parameterStructure = (ParameterStructure)keyValuePair.Value;

               DataTable dataTable = (DataTable)parameterStructure.Value;

               value = dataTable.AsTableValuedParameter(parameterStructure.Key);
            }

            if(parameterName == "newID")
               dynamicParameters.Add(string.Concat("@", parameterName), DbType.Int32, direction: ParameterDirection.Output);
            else
               dynamicParameters.Add(string.Concat("@", parameterName), value);
         }

         return dynamicParameters;
      }
      internal static SqlParameter[] convertDictionaryParametersToSqlParameter(Dictionary<string, object> parameters)
      {

         List<SqlParameter> sqlParameterList = new List<SqlParameter>();

         foreach (KeyValuePair<string, object> keyValuePair in parameters)
         {
            object value = keyValuePair.Value;
            string parameterName = StringUtil.FirstLetterToLowerCase(keyValuePair.Key);
            Type type = value.GetType();
            SqlParameter sqlParameter = null;

            if (!type.IsGenericType)
            {
               if (value == null)
                  value = DBNull.Value;

               sqlParameter = createParameter(string.Concat("@", parameterName), value);
            }
            else
            {

               string sqlTypeName = string.Empty;
               DataTable dataTable = null;
               int totalArg = type.GetGenericArguments().Count();

               if (totalArg != 0)
               {

                  Type argumentType = type.GetGenericArguments()[0];

                  if (argumentType == typeof(int))
                  {
                     List<int> list = (List<int>)value;
                     sqlTypeName = "dbo.ListInt";
                     dataTable = new DataTable();
                     dataTable.Columns.Add("value", typeof(int));
                     foreach (int i in list)
                        dataTable.Rows.Add(i);
                  }

                  if (argumentType == typeof(string))
                  {
                     List<string> list = (List<string>)value;
                     sqlTypeName = "dbo.ListString";
                     dataTable = new DataTable();
                     dataTable.Columns.Add("value", typeof(string));
                     foreach (string s in list)
                        dataTable.Rows.Add(s);
                  }

                  sqlParameter = createParameter(string.Concat("@", parameterName), dataTable);
                  sqlParameter.SqlDbType = SqlDbType.Structured;
                  sqlParameter.TypeName = sqlTypeName;
               }
            }

            sqlParameterList.Add(sqlParameter);
         }

         return sqlParameterList.ToArray<SqlParameter>();
      }
      internal static void logCommandInterceptor(string commandText, DynamicParameters dynamicParameters) {

         if (!Logger.IsInfoEnabled)
            return;

         StringBuilder sb = new StringBuilder();

         sb.Append("SQL : [" + commandText + " ");

         int size = 0;
         foreach (string parameterName in dynamicParameters.ParameterNames)
         {
            size++;
            sb.Append(string.Concat("@",parameterName,"="));

            object value = dynamicParameters.Get<object>(parameterName);
            if (value == null)
               sb.Append("NULL");
            else
            {
               if (value is DateTime)
                  sb.Append("'"+((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")+ "'");

               if (value is string)
                  sb.Append("'" + value.ToString() + "'");
               else
                  sb.Append(value.ToString());
            }

            if (size < dynamicParameters.ParameterNames.Count())
               sb.Append(", ");
         }

         sb.Append("]");

         Logger.Info(sb.ToString());
      }
   }
   internal class ParameterStructure
   {
      public string Key { get; set; }
      public object Value { get; set; }
   }
}
