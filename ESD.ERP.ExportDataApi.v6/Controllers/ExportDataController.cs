using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using ESD.Core;
using ESD.Core.Logging;
using ESD.ERP.ExportDataApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace ESD.ERP.ExportDataApi.v6.Controllers
{
   [Route("api/ExportData")]
   [ApiController]
   public class ExportDataController : ControllerBase
   {
      [HttpPost("Export")]
      public IActionResult Export([FromBody] IDictionary<string, string> data)
      {

         string reportName = data["reportName"];

         data.Remove("reportName");

         byte[] result = null;

         try
         {
            using (SqlConnection connection = DataHelper.CreateConnection())
            {
               connection.Open();
               using (SqlCommand command = new("SELECT objeto_sql FROM reporte_expo WHERE reporte='" + reportName + "'", connection))
               {

                  object objetoSQL = command.ExecuteScalar();
                  command.Parameters.Clear();

                  foreach (string key in data.Keys)
                  {

                     if (data[key] == SSRSConstant.URL_PARAMETER_NULL)
                        command.Parameters.Add(DataHelper.CreateParameter("@" + key, DBNull.Value));
                     else
                        command.Parameters.Add(DataHelper.CreateParameter("@" + key, data[key]));
                  }

                  command.CommandType = CommandType.StoredProcedure;
                  command.CommandText = objetoSQL.ToString();

                  int row = 1;
                  int fieldCount = 0;
                  using (ExcelPackage package = new())
                  {
                     ExcelWorksheet excelWorksheet = package.Workbook.Worksheets.Add(objetoSQL.ToString());
                     using (SqlDataReader dataReader = command.ExecuteReader())
                     {
                        fieldCount = dataReader.FieldCount;
                        for (int i = 0; i < fieldCount; i++)
                        {
                           excelWorksheet.Cells[row, i + 1].Value = dataReader.GetName(i);
                           excelWorksheet.Cells[row, i + 1].Style.Font.Bold = true;
                        }
                        row++;
                        while (dataReader.Read())
                        {
                           for (int i = 0; i < fieldCount; i++)
                           {
                              object value = dataReader.GetValue(i);
                              excelWorksheet.Cells[row, i + 1].Value = value;

                              if(value is DateTime)
                                 excelWorksheet.Cells[row, i + 1].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                           }
                           row++;
                        }

                        result = package.GetAsByteArray();
                     }
                  }
               }
            }

            return new FileContentResult(result, MediaTypeNames.Application.Octet)
            {
               FileDownloadName = "excelfromByte1.xlsx"
            };

         }
         catch (Exception e)
         {
            Logger.Error(e.ToString());
            throw;
         }

      }
      [HttpPost("ExportXml")]
      public IActionResult ExportXml([FromBody] IDictionary<string, string> data)
      {

         string reportName = data["reportName"];

         data.Remove("reportName");

         byte[] result = null;
         DataTable dataTable = new("resultData");

         try
         {
            using (SqlConnection connection = DataHelper.CreateConnection())
            {
               connection.Open();
               using (SqlCommand command = new("SELECT objeto_sql FROM reporte_expo WHERE reporte='" + reportName + "'", connection))
               {

                  object objetoSQL = command.ExecuteScalar();
                  command.Parameters.Clear();

                  foreach (string key in data.Keys)
                  {

                     if (data[key] == SSRSConstant.URL_PARAMETER_NULL)
                        command.Parameters.Add(DataHelper.CreateParameter("@" + key, DBNull.Value));
                     else
                        command.Parameters.Add(DataHelper.CreateParameter("@" + key, data[key]));
                  }

                  command.CommandType = CommandType.StoredProcedure;
                  command.CommandText = objetoSQL.ToString();

                  using (SqlDataAdapter dataAdapter = new(command))
                  {
                     dataAdapter.Fill(dataTable);
                  }
               }
            }

            using (MemoryStream sw = new())
            {
               dataTable.WriteXml(sw,XmlWriteMode.WriteSchema);
               result = sw.ToArray();
            }

            return new FileContentResult(result, MediaTypeNames.Application.Xml)
            {
               FileDownloadName = "data.xml"
            };

         }
         catch (Exception e)
         {
            Logger.Error(e.ToString());
            throw;
         }

      }
   }
}
