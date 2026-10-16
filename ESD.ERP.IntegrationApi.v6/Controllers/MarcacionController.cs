using ESD.ERP.IntegrationApi.Infrastructure;
using NLog;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.Http;

namespace ESD.ERP.IntegrationApi.Controllers
{
   public class MarcacionController: DataController
   {
      private static Logger logger = LogManager.GetCurrentClassLogger();

      [Route("api/Marcacion/MigrarMarcaciones")]
      [HttpPost]
      public ServiceResponse MigrarMarcaciones([FromBody] string empresaId)
      {

         ServiceResponse serviceResponse = new ServiceResponse();
         StringBuilder stringBuilder = new StringBuilder();

         try
         {
            string tempTable = "tmp_marcacion_" + empresaId;
            DataTable dataReloj = new DataTable();
            using (SqlConnection sqlConnection = CreateERPConnection())
            {
               string commandText = "SELECT reloj_id,empresa_id,nombre,ruta_archivo FROM reloj WHERE empresa_id="+ empresaId + " AND estado='A' ";
               using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
               {
                  using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                  {
                     dataReloj.Load(dataReader);
                  }
               }

               commandText = "IF OBJECT_ID('" + tempTable + "', 'U') IS NOT NULL DROP TABLE " + tempTable + ";"
                  + "\nCREATE TABLE " + tempTable
                  + "\n("
                  + "\nempresa_id INT NOT NULL,"
                  + "\ncodigo CHAR(5) NOT NULL,"
                  + "\nfecha DATETIME NOT NULL"
                  + "\n)";
               using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
               {
                  sqlCommand.ExecuteNonQuery();
               }
            }

            foreach (DataRow row in dataReloj.Rows)
            {
               DataTable dataAccess = new DataTable();
               string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + row["ruta_archivo"].ToString() + ";User Id=admin;Password=;";

               logger.Info("connectionString=" + connectionString);

               if (!File.Exists(row["ruta_archivo"].ToString())){
                  stringBuilder.AppendLine("- Marcador: " + row["nombre"].ToString() + " - No hay coneccion" );
                  logger.Info("Marcador: " + row["nombre"].ToString() + " [ Archivo no existe o la ruta no es accesible ]");
                  continue;
               }

               using (OleDbConnection oledbConnection = new OleDbConnection(connectionString))
               {
                  oledbConnection.Open();

                  string commandText = "SELECT " + row["empresa_id"] + " AS EmpresaId, RIGHT(ui.CardNo,5) AS CarnNo, ci.CHECKTIME"
                     + "\nFROM CHECKINOUT AS ci INNER JOIN USERINFO AS ui ON ci.USERID = ui.USERID WHERE ui.CardNo <> '' AND "
                     + "\n(ci.CHECKTIME >= DateAdd(\"d\",-30,Date()))";
                  using (OleDbCommand oledbCommand = new OleDbCommand(commandText, oledbConnection))
                  {

                     using (OleDbDataReader dataReader = oledbCommand.ExecuteReader())
                     {
                        dataAccess.Load(dataReader);
                     }
                  }
                  logger.Info("Marcador: " + row["nombre"].ToString() + " [ Count =" + dataAccess.Rows.Count.ToString() + "]");
               }

               using (SqlConnection sqlConnection = CreateERPConnection())
               {
                  SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection)
                  {
                     DestinationTableName = tempTable
                  };
                  bulkCopy.ColumnMappings.Add("empresaId", "empresa_id");
                  bulkCopy.ColumnMappings.Add("CarnNo", "codigo");
                  bulkCopy.ColumnMappings.Add("CHECKTIME", "fecha");

                  bulkCopy.WriteToServer(dataAccess);

                  string commandText = "INSERT INTO reloj_marca"
                     + "\nSELECT"
                     + "\nNEXT VALUE FOR reloj_marca_seq," + row["reloj_id"] + ",s.personal_id,CONVERT(DATE,s.fecha),s.fecha,s.fecha,'','A'"
                     + "\nFROM"
                     + "\n("
                     + "\nSELECT p.personal_id,t.fecha FROM " + tempTable + " t"
                     + "\nINNER JOIN personal p ON p.personal = t.codigo COLLATE database_default"
                     + "\nINNER JOIN empresa_sede es ON es.empresa_sede_id = p.empresa_sede_id"
                     + "\nWHERE es.empresa_id = " + empresaId
                     + "\n) s"
                     + "\nLEFT JOIN reloj_marca AS t"
                     + "\nON (t.personal_id = s.personal_id AND t.fecha_orig = s.fecha)"
                     + "\nWHERE t.personal_id IS NULL;"
                     + "\nDELETE FROM " + tempTable;
                  using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
                  {
                     sqlCommand.ExecuteNonQuery();
                  }
               }
               stringBuilder.AppendLine("- Marcador: " + row["nombre"].ToString() + " - Proceso Concluido");
            }

            using (SqlConnection sqlConnection = CreateERPConnection())
            {
               string commandText = "IF OBJECT_ID('" + tempTable + "', 'U') IS NOT NULL DROP TABLE " + tempTable;
               using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
               {
                  sqlCommand.ExecuteNonQuery();
               }
            }

            serviceResponse.Message = stringBuilder.ToString();

         }
         catch (Exception e)
         {
            logger.Error(e.ToString());
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
         }

         return serviceResponse;
      }
   }
}