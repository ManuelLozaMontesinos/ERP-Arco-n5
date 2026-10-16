using ESD.ERP.IntegrationApi.Infrastructure;
using ESD.ERP.IntegrationApi.Models;
using NLog;
using System;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ESD.ERP.IntegrationApi.Controllers
{
   public class RucController: DataController
   {
      private static Logger logger = LogManager.GetCurrentClassLogger();

      [HttpGet]
      public ServiceResponse Get(string id)
      {
         ServiceResponse serviceResponse = new ServiceResponse();

         logger.Info("Get :: Start Execution");

         try
         {
            logger.Info("Get :: Execution finished!");

            RucDTO rucDTO = obtenerProveedor(id);

            serviceResponse.Data = new JavaScriptSerializer().Serialize(rucDTO);
         }
         catch (Exception e)
         {
            logger.Error(e.ToString());
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
         }

         return serviceResponse;
      }
      private RucDTO obtenerProveedor(string ruc)
      {
         logger.Info("obtenerProveedor :: Start Execution!");

         string sql = "SELECT ruc,razon_social,estado,condicion_domicilio,ubigeo,tipo_de_via,nombre_de_via,"
            + "codigo_de_zona,tipo_de_zona,numero,interior,lote,departamento,manzana,kilometro "
            + "FROM tabla_ruc WHERE ruc = @ruc";

         RucDTO rucDTO = null;

         using (SqlConnection connection = CreateERPConnection("sunat"))
         {
            using (SqlCommand command = new SqlCommand(sql, connection))
            {

               command.Parameters.Add(CreateParameter("ruc", ruc));
               using (SqlDataReader dataReader = command.ExecuteReader())
               {
                  while (dataReader.Read())
                  {
                     if (rucDTO == null)
                        rucDTO = new RucDTO();

                     rucDTO.RUC = dataReader.GetString(dataReader.GetOrdinal("ruc"));
                     rucDTO.RazonSocial = clearStringData(dataReader.GetString(dataReader.GetOrdinal("razon_social")));
                     rucDTO.EstadoNombre = dataReader.GetString(dataReader.GetOrdinal("estado"));
                     rucDTO.EsActivo = rucDTO.EstadoNombre == "ACTIVO" ? true : false;

                     rucDTO.CondicionDomicilio = dataReader.GetString(dataReader.GetOrdinal("condicion_domicilio"));
                     rucDTO.EsHabido = rucDTO.CondicionDomicilio == "HABIDO" ? true : false;

                     string ubigeo = clearStringData(dataReader.GetString(dataReader.GetOrdinal("ubigeo")));
                     if (!string.IsNullOrEmpty(ubigeo))
                     {
                        rucDTO.UbigeoDepartamento = ubigeo.Substring(0, 2);
                        rucDTO.UbigeoProvincia = ubigeo.Substring(2, 2);
                        rucDTO.UbigeoDistrito = ubigeo.Substring(4, 2);
                     }


                     rucDTO.TipoVia = clearStringData(dataReader.GetString(dataReader.GetOrdinal("tipo_de_via")));
                     rucDTO.NombreVia = clearStringData(dataReader.GetString(dataReader.GetOrdinal("nombre_de_via")));
                     rucDTO.TipoZona = clearStringData(dataReader.GetString(dataReader.GetOrdinal("codigo_de_zona")));
                     rucDTO.NombreZona = clearStringData(dataReader.GetString(dataReader.GetOrdinal("tipo_de_zona")));
                     rucDTO.Numero = clearStringData(dataReader.GetString(dataReader.GetOrdinal("numero")));
                     rucDTO.Interior = clearStringData(dataReader.GetString(dataReader.GetOrdinal("interior")));
                     rucDTO.Lote = clearStringData(dataReader.GetString(dataReader.GetOrdinal("lote")));
                     rucDTO.Departamento = clearStringData(dataReader.GetString(dataReader.GetOrdinal("departamento")));
                     rucDTO.Manzana = clearStringData(dataReader.GetString(dataReader.GetOrdinal("manzana")));
                     rucDTO.Kilometro = clearStringData(dataReader.GetString(dataReader.GetOrdinal("kilometro")));
                  }
               }
            }
         }

         logger.Info("obtenerProveedor :: Execution finished!");

         return rucDTO;
      }
      private string clearStringData(string val)
      {

         if (string.IsNullOrEmpty(val))
            return string.Empty;

         if (val == "-|")
            return string.Empty;

         if (val.Substring(0, 1) == "-")
            return string.Empty;

         return val.Trim();
      }
   }
}