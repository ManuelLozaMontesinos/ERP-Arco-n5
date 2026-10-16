using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ESD.Core.Data;
using ESD.Core.Logging;

namespace ESD.ERP.ExportDataApi.Infrastructure
{
   public class DataHelper
   {
      public static void ExecCommandWithParameters(string storedProcedureName, List<SqlParameter> list)
      {
         Logger.InfoStart();

         SqlConnection sqlConnection = CreateConnection();

         try
         {

            sqlConnection.Open();

            if (sqlConnection.State == ConnectionState.Open)
            {
               SqlCommand sqlCommand = new(storedProcedureName)
               {
                  Connection = sqlConnection,
                  CommandType = CommandType.StoredProcedure
               };
               sqlCommand.Parameters.AddRange(list.ToArray());
               sqlCommand.ExecuteNonQuery();

               sqlConnection.Close();
            }

            Logger.InfoFinish();
         }
         catch (Exception e)
         {
            Logger.Error(e.ToString());
            throw;
         }
      }
      public static DataTable ExecCommandWithResult(string storedProcedureName, List<SqlParameter> list)
      {
         Logger.InfoStart();

         SqlConnection sqlConnection = CreateConnection();

         try
         {

            DataTable dataTable = new();
            sqlConnection.Open();

            if (sqlConnection.State == ConnectionState.Open)
            {
               SqlCommand sqlCommand = new(storedProcedureName);
               sqlCommand.Connection = sqlConnection;
               sqlCommand.CommandType = CommandType.StoredProcedure;
               sqlCommand.Parameters.AddRange(list.ToArray());
               sqlCommand.ExecuteNonQuery();

               SqlDataAdapter sqlDataAdapter = new(sqlCommand);
               sqlDataAdapter.Fill(dataTable);

               sqlConnection.Close();
            }

            Logger.InfoFinish();

            return dataTable;
         }
         catch (Exception e)
         {
            Logger.Error(e.ToString());
            throw;
         }
      }
      public static SqlConnection CreateConnection()
      {
         string connectionString = DbFactory.GetConnectionString();

         SqlConnection sqlConnection = new(connectionString);

         return sqlConnection;
      }
      public static SqlParameter CreateParameter(string paramName, SqlDbType dataType, int dataSize, object dataValue)
      {
         SqlParameter sqlParameter = new(paramName, dataType, dataSize)
         {
            Direction = ParameterDirection.Input,
            Value = dataValue
         };
         return sqlParameter;
      }
      public static SqlParameter CreateParameter(string paramName, SqlDbType dataType, object dataValue)
      {
         SqlParameter sqlParameter = new(paramName, dataType)
         {
            Direction = ParameterDirection.Input,
            Value = dataValue
         };
         return sqlParameter;
      }
      public static SqlParameter CreateParameter(string paramName, object dataValue)
      {
         SqlParameter sqlParameter = new(paramName, dataValue)
         {
            Direction = ParameterDirection.Input
         };
         return sqlParameter;
      }
   }
}