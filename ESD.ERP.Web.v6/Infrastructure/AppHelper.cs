using ESD.Core;
using ESD.Core.Logging;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.WebApiProxy;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using Path = System.IO.Path;
using PageSize = iText.Kernel.Geom.PageSize;

namespace ESD.ERP.Web.Infrastructure
{
   public class AppHelper
   {
      private readonly IParametroSistemaProxy parametroSistemaProxy;
      private readonly IExportarDataProxy exportarDataProxy;
      private readonly IConfiguration configuration;

      public AppHelper(
         IParametroSistemaProxy parametroSistemaProxy,
         IExportarDataProxy exportarDataProxy,
         IConfiguration configuration = null
         )
      {
         this.parametroSistemaProxy = parametroSistemaProxy;
         this.exportarDataProxy = exportarDataProxy;
         this.configuration = configuration;
      }

      public List<ParametroSistemaDTO> ObtenerGrupoParametroSistema(string grupo)
      {
         ParametroSistemaDTO parametroSistemaDTO = new ParametroSistemaDTO()
         {
            OrderBy = "ps.orden_grupo",
            GrupoParametroSistemaDTO = new GrupoParametroSistemaDTO() { Codigo = grupo },
            Estado = EstadoConstante.ACTIVO
         };

         List<ParametroSistemaDTO> parametroSistemaDTOList = parametroSistemaProxy.Search(parametroSistemaDTO)
            .Cast<ParametroSistemaDTO>().ToList();

         return parametroSistemaDTOList;
      }
      public object ObtenerParametroSistemaValor(ParametroSistemaDTO parametroSistemaDTO)
      {

         object returnValue = null;

         if (parametroSistemaDTO == null)
            return null;

         switch (parametroSistemaDTO.TipoDatoDTO.Codigo)
         {
            case TipoDatoConstante.CADENA:
            case TipoDatoConstante.CADENA_OCULTA:
               returnValue = parametroSistemaDTO.ValorCadena;
               break;
            case TipoDatoConstante.ENTERO:
               returnValue = Convert.ToInt32(parametroSistemaDTO.ValorNumerico);
               break;
            //case TipoDatoConstante.RANGO_NUMERICO:
            //   int rangoDesde = Convert.ToInt32(parametroSistemaDTO.RangoNumericoDesde);
            //   int rangoHasta = Convert.ToInt32(parametroSistemaDTO.RangoNumericoHasta);
            //   returnValue = new int[] { rangoDesde, rangoHasta };
            //   break;
            case TipoDatoConstante.DECIMAL:
               returnValue = parametroSistemaDTO.ValorNumerico;
               break;
            case "05": // BOOLEANO
               returnValue = parametroSistemaDTO.ValorNumerico == 1 || parametroSistemaDTO.ValorCadena == "1" || string.Equals(parametroSistemaDTO.ValorCadena, "true", StringComparison.OrdinalIgnoreCase);
               break;
            //case TipoDatoConstante.FECHA:
            //   returnValue = parametroSistemaDTO.ValorFecha;
            //   break;
            //case TipoDatoConstante.ESTADO:
            //   returnValue = Convert.ToInt32(parametroSistemaDTO.ValorNumerico) == 1;
            //   break;

         }

         return returnValue;
      }
      public void SendEmail(List<string> addressTo, List<string> addressCC, string subject, string body, bool isBodyHtml, byte[] data, string attachmentName)
      {

         if (addressTo.Count == 0)
            return;

         List<ParametroSistemaDTO> parametroSistemaDTOList = ObtenerGrupoParametroSistema(GrupoParametroSistemaConstante.EMAIL_CONFIGURATION);
         if (parametroSistemaDTOList == null || parametroSistemaDTOList.Count == 0)
            return;

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.HABILITAR_ENVIO_EMAIL);
         if (parametroSistemaDTO == null) return;
         bool envioCorreoHabilitado = Convert.ToBoolean(ObtenerParametroSistemaValor(parametroSistemaDTO));

         if (!envioCorreoHabilitado)
         {
            return;
         }

         parametroSistemaDTO = parametroSistemaDTOList.FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.SERVIDOR_EMAIL);
         if (parametroSistemaDTO == null) return;
         string host = parametroSistemaDTO.ValorCadena;

         parametroSistemaDTO = parametroSistemaDTOList.FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.PUERTO_SMTP);
         int port = parametroSistemaDTO != null ? Convert.ToInt32(parametroSistemaDTO.ValorNumerico) : 25;

         parametroSistemaDTO = parametroSistemaDTOList.FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.USUARIO_EMAIL);
         string emailUserName = parametroSistemaDTO?.ValorCadena ?? string.Empty;

         parametroSistemaDTO = parametroSistemaDTOList.FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.CONTRASENA_EMAIL);
         string emailPassword = parametroSistemaDTO?.ValorCadena ?? string.Empty;

         parametroSistemaDTO = parametroSistemaDTOList.FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.USAR_SSL);
         bool usarSSL = parametroSistemaDTO != null && Convert.ToBoolean(ObtenerParametroSistemaValor(parametroSistemaDTO));

         try
         {
            MailMessage message = new MailMessage();

            List<string> distinctAddressList = addressTo.Distinct().ToList();
            foreach (string address in distinctAddressList)
               message.To.Add(new MailAddress(address));

            if (addressCC != null)
            {
               distinctAddressList = addressCC.Distinct().ToList();
               foreach (string address in distinctAddressList)
                  message.CC.Add(new MailAddress(address));
            }

            message.From = new MailAddress(emailUserName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isBodyHtml;

            if (data != null)
            {
               Attachment attachment = new Attachment(new MemoryStream(data), attachmentName);
               message.Attachments.Add(attachment);
            }

            using (SmtpClient smtpClient = new SmtpClient())
            {
               NetworkCredential credential = new NetworkCredential
               {
                  UserName = emailUserName,
                  Password = emailPassword
               };
               smtpClient.Credentials = credential;
               smtpClient.Host = host;
               smtpClient.Port = port;
               smtpClient.EnableSsl = usarSSL;
               smtpClient.Send(message);
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public FileContentResult ReportToPdf(string reportName, Dictionary<string, string> parameters, string downloadFileName)
      {

         //string reportURL = "http://localhost/ReportServer?/[MODULE]%2fProyectoPresupAnual&rs:Command=Render&rs:Format=PDF";
         byte[] fileBytes = ExecuteReport(reportName, parameters, ReportFormatConstant.PDF);

         FileContentResult fileContentResult = new FileContentResult(fileBytes, MediaTypeNames.Application.Octet);
         fileContentResult.FileDownloadName = string.Concat(Path.GetFileNameWithoutExtension(downloadFileName), ReportExportExtensionConstant.PDF);

         return fileContentResult;
      }
      public FileContentResult ReportToPdf(byte[] fileBytes, string downloadFileName)
      {
         FileContentResult fileContentResult = new FileContentResult(fileBytes, MediaTypeNames.Application.Octet)
         {
            FileDownloadName = string.Concat(Path.GetFileNameWithoutExtension(downloadFileName), ReportExportExtensionConstant.PDF)
         };

         return fileContentResult;
      }
      public FileContentResult ReportToExcel(string reportName, Dictionary<string, string> parameters, string downloadFileName)
      {
         byte[] fileBytes = ExecuteReport(reportName, parameters, ReportFormatConstant.EXCEL);

         FileContentResult fileContentResult = new FileContentResult(fileBytes, MediaTypeNames.Application.Octet)
         {
            FileDownloadName = string.Concat(Path.GetFileNameWithoutExtension(downloadFileName), ReportExportExtensionConstant.EXCEL)
         };

         return fileContentResult;
      }
      public FileContentResult ReportToExcel(byte[] fileBytes, string downloadFileName)
      {
         FileContentResult fileContentResult = new FileContentResult(fileBytes, MediaTypeNames.Application.Octet);
         fileContentResult.FileDownloadName = string.Concat(Path.GetFileNameWithoutExtension(downloadFileName), ReportExportExtensionConstant.EXCEL);

         return fileContentResult;
      }
      public byte[] ExecuteReport(string reportName, Dictionary<string, string> parameters, string format)
      {

         WebClient webClient = new WebClient();

         string baseURL = string.Empty;
         bool useWebAppCredential = false;
         string userName = string.Empty;
         string password = string.Empty;

         List<ParametroSistemaDTO> parametroSistemaDTOList = ObtenerGrupoParametroSistema(GrupoParametroSistemaConstante.SSRS_CONFIGURATION);
         if (parametroSistemaDTOList == null || parametroSistemaDTOList.Count == 0)
            throw new Exception("No se encontró la configuración de Reporting Services (SSRS).");

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.SSRS_BASE_PATH);
         baseURL = parametroSistemaDTO?.ValorCadena ?? string.Empty;

         parametroSistemaDTO = parametroSistemaDTOList.FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.SSRS_USE_CREDENTIAL);
         useWebAppCredential = parametroSistemaDTO != null && Convert.ToBoolean(ObtenerParametroSistemaValor(parametroSistemaDTO));

         parametroSistemaDTO = parametroSistemaDTOList.FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.SSRS_USER);
         userName = parametroSistemaDTO != null ? Convert.ToString(ObtenerParametroSistemaValor(parametroSistemaDTO)) : string.Empty;

         parametroSistemaDTO = parametroSistemaDTOList.FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.SSRS_PASSWORD);
         password = parametroSistemaDTO != null ? Convert.ToString(ObtenerParametroSistemaValor(parametroSistemaDTO)) : string.Empty;

         try
         {
            if (useWebAppCredential || string.IsNullOrWhiteSpace(userName))
            {
               webClient.UseDefaultCredentials = true;
            }
            else
            {
               string[] networkUser = userName.Split(new char[] { '\\' });
               if (networkUser.Length == 1)
                  webClient.Credentials = new NetworkCredential(networkUser[0], password);
               else
                  webClient.Credentials = new NetworkCredential(networkUser[1], password, networkUser[0]);
            }

            StringBuilder sb = new StringBuilder();

            if (parameters != null)
               foreach (KeyValuePair<string, string> entry in parameters)
               {
                  if (entry.Value == null)
                     sb.Append(string.Concat(SSRSConstant.URL_SEPARATOR, entry.Key, SSRSConstant.URL_PARAMETER_NULL));
                  else
                     sb.Append(string.Concat(SSRSConstant.URL_SEPARATOR, entry.Key, SSRSConstant.URL_EQUALS, entry.Value));
               }

            string reportURL = baseURL + reportName + "&rs:Command=Render&rs:Format=" + format + sb.ToString();

            byte[] fileBytes = null;
            try
            {
               fileBytes = webClient.DownloadData(reportURL);
            }
            catch (WebException ex)
            {
               Logger.Info($"ExecuteReport: Error al descargar desde SSRS ({reportURL}): {ex.Message}");
               if (reportURL.IndexOf("SRVQA001", StringComparison.OrdinalIgnoreCase) >= 0)
               {
                  try
                  {
                     using WebClient fallbackClient = new WebClient();
                     fallbackClient.UseDefaultCredentials = true;
                     string fallbackURL = reportURL.Replace("SRVQA001", "localhost", StringComparison.OrdinalIgnoreCase);
                     fileBytes = fallbackClient.DownloadData(fallbackURL);
                  }
                  catch (WebException exLocal)
                  {
                     Logger.Info($"ExecuteReport: Error en fallback local SSRS ({reportURL}): {exLocal.Message}");
                  }
               }
            }

            // Si SSRS no respondió o el reporte no existe en SSRS (error 500 rsItemNotFound), y se solicitó PDF, usamos el fallback con datos
            if (fileBytes == null && string.Equals(format, ReportFormatConstant.PDF, StringComparison.OrdinalIgnoreCase))
            {
               Logger.Info($"ExecuteReport: SSRS no disponible o reporte no encontrado. Utilizando generador PDF fallback para '{reportName}'...");
               fileBytes = GenerarPdfFallback(reportName, parameters);
            }

            if (fileBytes == null)
            {
               throw new Exception($"No se pudo generar el reporte '{reportName}'. SSRS no respondió y no se encontraron datos para el fallback.");
            }

            return fileBytes;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public FileContentResult ReportToExcelData(string reportName, Dictionary<string, string> parameters, string downloadFileName)
      {

         byte[] fileBytes = ExecuteExportData(reportName, parameters);

         FileContentResult fileContentResult = new FileContentResult(fileBytes, MediaTypeNames.Application.Octet)
         {
            FileDownloadName = string.Concat(Path.GetFileNameWithoutExtension(downloadFileName), ReportExportExtensionConstant.EXCELX)
         };

         return fileContentResult;
      }
      public byte[] ExecuteExportData(string reportName, Dictionary<string, string> parameters)
      {
         Logger.Info("ExecuteExportData :: Start Execution");

         try
         {
            var p = new Dictionary<string, string>(parameters ?? new Dictionary<string, string>());
            p["reportName"] = reportName;

            byte[] fileBytes = exportarDataProxy.Exportar(p);

            if (fileBytes == null)
               Logger.Info("ExecuteReportDataOnly :: fileBytes size : null");
            else
               Logger.Info("ExecuteReportDataOnly :: fileBytes size :" + fileBytes.Length.ToString());

            Logger.Info("ExecuteReportDataOnly :: Execution finished!");

            return fileBytes;
         }
         catch (Exception e)
         {
            Logger.Error(e.ToString());
            throw;
         }
      }
      public byte[] ExecuteExportXml(string reportName, Dictionary<string, string> parameters)
      {
         Logger.Info("ExecuteExportData :: Start Execution");

         try
         {
            var p = new Dictionary<string, string>(parameters ?? new Dictionary<string, string>());
            p["reportName"] = reportName;

            byte[] fileBytes = exportarDataProxy.ExportarXml(p);

            if (fileBytes == null)
               Logger.Info("ExecuteReportDataOnly :: fileBytes size : null");
            else
               Logger.Info("ExecuteReportDataOnly :: fileBytes size :" + fileBytes.Length.ToString());

            Logger.Info("ExecuteReportDataOnly :: Execution finished!");

            return fileBytes;
         }
         catch (Exception e)
         {
            Logger.Error(e.ToString());
            throw;
         }
      }

      public byte[] GenerarPdfFallback(string reportName, Dictionary<string, string> parameters)
      {
         Logger.Info($"GenerarPdfFallback :: Generando PDF alternativo para {reportName}");
         try
         {
            byte[] excelBytes = null;
            try
            {
               excelBytes = ExecuteExportData(reportName, parameters);
            }
            catch (Exception exProxy)
            {
               Logger.Info($"GenerarPdfFallback :: exportarDataProxy falló ({exProxy.Message}). Intentando consulta directa SQL...");
            }

            if (excelBytes == null || excelBytes.Length == 0)
            {
               excelBytes = GenerarExcelDesdeSql(reportName, parameters);
            }

            if (excelBytes != null && excelBytes.Length > 0)
            {
               return ConvertExcelToPdf(excelBytes, reportName);
            }
            else
            {
               return GenerarPdfVacio(reportName);
            }
         }
         catch (Exception ex)
         {
            Logger.Error($"GenerarPdfFallback :: Error: {ex}");
         }

         return null;
      }

      private byte[] GenerarExcelDesdeSql(string reportName, Dictionary<string, string> parameters)
      {
         string connStr = DatabaseConnectionHelper.GetConnectionString(configuration);

         ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
         using SqlConnection conn = new SqlConnection(connStr);
         conn.Open();

         string sqlObjeto = null;
         using (SqlCommand cmdExpo = new SqlCommand("SELECT objeto_sql FROM reporte_expo WHERE reporte = @rep", conn))
         {
            cmdExpo.Parameters.AddWithValue("@rep", reportName);
            object obj = cmdExpo.ExecuteScalar();
            sqlObjeto = obj?.ToString();
         }

         if (string.IsNullOrWhiteSpace(sqlObjeto))
         {
            if (reportName.IndexOf("LiquidacionCaja", StringComparison.OrdinalIgnoreCase) >= 0)
               sqlObjeto = "sp_rptvt_liquidacionCaja";
            else if (reportName.IndexOf("PedidoPorProducto", StringComparison.OrdinalIgnoreCase) >= 0)
               sqlObjeto = "sp_rptvt_registroPedidoProducto";
            else if (reportName.IndexOf("SaldoConsolidado", StringComparison.OrdinalIgnoreCase) >= 0 || reportName.IndexOf("SaldoValorizado", StringComparison.OrdinalIgnoreCase) >= 0)
               sqlObjeto = "sp_rptiv_saldoValorizado";
            else
               return null;
         }

         using SqlCommand spCmd = new SqlCommand(sqlObjeto, conn);
         spCmd.CommandType = CommandType.StoredProcedure;
         spCmd.CommandTimeout = 180;
         if (parameters != null)
         {
            foreach (var kvp in parameters)
            {
               if (string.Equals(kvp.Key, "reportName", StringComparison.OrdinalIgnoreCase)) continue;
               string paramName = kvp.Key.StartsWith("@") ? kvp.Key : "@" + kvp.Key;
               if (kvp.Value == null || kvp.Value == SSRSConstant.URL_PARAMETER_NULL)
                  spCmd.Parameters.Add(new SqlParameter(paramName, DBNull.Value));
               else
                  spCmd.Parameters.Add(new SqlParameter(paramName, kvp.Value));
            }
         }

         using ExcelPackage package = new ExcelPackage();
         ExcelWorksheet ws = package.Workbook.Worksheets.Add(reportName);
         using (SqlDataReader reader = spCmd.ExecuteReader())
         {
            int fieldCount = reader.FieldCount;
            int row = 1;
            for (int i = 0; i < fieldCount; i++)
            {
               ws.Cells[row, i + 1].Value = reader.GetName(i);
               ws.Cells[row, i + 1].Style.Font.Bold = true;
            }
            row++;
            while (reader.Read())
            {
               for (int i = 0; i < fieldCount; i++)
               {
                  ws.Cells[row, i + 1].Value = reader.GetValue(i);
               }
               row++;
            }
         }
         return package.GetAsByteArray();
      }

      public byte[] ConvertExcelToPdf(byte[] excelBytes, string reportTitle)
      {
         if (excelBytes == null || excelBytes.Length == 0)
            return GenerarPdfVacio(reportTitle);

         ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
         using MemoryStream excelStream = new MemoryStream(excelBytes);
         using ExcelPackage package = new ExcelPackage(excelStream);
         ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();
         if (ws == null || ws.Dimension == null || ws.Dimension.End.Row <= 0)
            return GenerarPdfVacio(reportTitle);

         int totalRows = ws.Dimension.End.Row;
         int totalCols = ws.Dimension.End.Column;
         if (totalCols <= 0)
            return GenerarPdfVacio(reportTitle);

         using MemoryStream pdfStream = new MemoryStream();
         PageSize pageSize = totalCols > 6 ? PageSize.A4.Rotate() : PageSize.A4;
         using (PdfWriter writer = new PdfWriter(pdfStream))
         using (PdfDocument pdfDoc = new PdfDocument(writer))
         using (Document document = new Document(pdfDoc, pageSize))
         {
            document.SetMargins(15, 15, 20, 15);

            string cleanTitle = string.IsNullOrWhiteSpace(reportTitle) ? "REPORTE" : reportTitle.Replace('_', ' ').ToUpper();
            Paragraph titlePara = new Paragraph(cleanTitle)
               .SetFontSize(14)
               .SetBold()
               .SetFontColor(new DeviceRgb(31, 78, 120))
               .SetTextAlignment(TextAlignment.CENTER)
               .SetMarginBottom(3);
            document.Add(titlePara);

            Paragraph subtitlePara = new Paragraph($"Fecha de emisión: {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
               .SetFontSize(8)
               .SetFontColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetMarginBottom(10);
            document.Add(subtitlePara);

            Table table = new Table(UnitValue.CreatePercentArray(totalCols)).UseAllAvailableWidth();

            for (int col = 1; col <= totalCols; col++)
            {
               string headerText = ws.Cells[1, col].Text ?? string.Empty;
               Cell headerCell = new Cell()
                  .Add(new Paragraph(headerText).SetFontSize(8).SetBold())
                  .SetBackgroundColor(new DeviceRgb(41, 128, 185))
                  .SetFontColor(ColorConstants.WHITE)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetPadding(4)
                  .SetBorder(new SolidBorder(new DeviceRgb(31, 78, 120), 0.5f));
               table.AddHeaderCell(headerCell);
            }

            if (totalRows <= 1)
            {
               Cell emptyCell = new Cell(1, totalCols)
                  .Add(new Paragraph("No se encontraron registros para los filtros seleccionados.").SetFontSize(9).SetItalic())
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetPadding(12)
                  .SetBorder(new SolidBorder(new DeviceRgb(220, 224, 230), 0.5f));
               table.AddCell(emptyCell);
            }
            else
            {
               DeviceRgb altBg = new DeviceRgb(245, 248, 252);
               for (int row = 2; row <= totalRows; row++)
               {
                  bool isAlt = row % 2 == 1;
                  for (int col = 1; col <= totalCols; col++)
                  {
                     var cell = ws.Cells[row, col];
                     string text = cell.Text ?? string.Empty;
                     var val = cell.Value;

                     TextAlignment align = TextAlignment.LEFT;
                     if (val is double || val is decimal || val is float || val is int || val is long)
                     {
                        align = TextAlignment.RIGHT;
                     }
                     else if (val is DateTime || Regex.IsMatch(text, @"^\d{2}/\d{2}/\d{4}"))
                     {
                        align = TextAlignment.CENTER;
                     }

                     Cell dataCell = new Cell()
                        .Add(new Paragraph(text).SetFontSize(7))
                        .SetTextAlignment(align)
                        .SetPadding(3)
                        .SetBorder(new SolidBorder(new DeviceRgb(220, 224, 230), 0.5f));

                     if (isAlt)
                     {
                        dataCell.SetBackgroundColor(altBg);
                     }

                     table.AddCell(dataCell);
                  }
               }
            }

            document.Add(table);
         }

         return pdfStream.ToArray();
      }

      private byte[] GenerarPdfVacio(string reportTitle)
      {
         using MemoryStream pdfStream = new MemoryStream();
         using (PdfWriter writer = new PdfWriter(pdfStream))
         using (PdfDocument pdfDoc = new PdfDocument(writer))
         using (Document document = new Document(pdfDoc, PageSize.A4))
         {
            document.SetMargins(20, 20, 20, 20);
            string cleanTitle = string.IsNullOrWhiteSpace(reportTitle) ? "REPORTE" : reportTitle.Replace('_', ' ').ToUpper();
            document.Add(new Paragraph(cleanTitle)
               .SetFontSize(14)
               .SetBold()
               .SetFontColor(new DeviceRgb(31, 78, 120))
               .SetTextAlignment(TextAlignment.CENTER)
               .SetMarginBottom(3));
            document.Add(new Paragraph($"Fecha de emisión: {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
               .SetFontSize(8)
               .SetFontColor(ColorConstants.GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetMarginBottom(20));
            document.Add(new Paragraph("No se encontraron registros para los filtros seleccionados.")
               .SetFontSize(10)
               .SetItalic()
               .SetTextAlignment(TextAlignment.CENTER));
         }
         return pdfStream.ToArray();
      }
      public byte[] MergePDFFiles(MemoryStream[] source, PdfWaterMark[] waterMarkList)
      {
         MemoryStream memoryStream = new MemoryStream();

         PdfDocument pdfDoc = new PdfDocument(new PdfWriter(memoryStream));
         PdfMerger merger = new PdfMerger(pdfDoc);

         int memTotal = 0;
         foreach (MemoryStream ms in source)
         {

            MemoryStream msNewPdf = null;
            memTotal++;

            if (waterMarkList != null)
            {
               if (!string.IsNullOrEmpty(waterMarkList[memTotal - 1].WaterMark))
                  msNewPdf = new MemoryStream(addPdfStringWatermark(ms, waterMarkList[memTotal - 1]));
               else
                  msNewPdf = ms;
            }
            else
               msNewPdf = ms;

            PdfDocument newDoc = new PdfDocument(new PdfReader(msNewPdf));

            merger.Merge(newDoc, 1, newDoc.GetNumberOfPages());

            newDoc.Close();

         };
         pdfDoc.Close();

         return memoryStream.GetBuffer();
      }
      internal byte[] addPdfStringWatermark(MemoryStream source, PdfWaterMark fdfWaterMark)
      {
         MemoryStream memoryStream = new MemoryStream();

         PdfDocument pdfDoc = new PdfDocument(new PdfReader(source),new PdfWriter(memoryStream));

         Paragraph paragraph = new Paragraph(fdfWaterMark.WaterMark).
                        SetMargin(0).
                        SetMultipliedLeading(1).
                        SetFont(PdfFontFactory.CreateRegisteredFont("Helvetica", PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED)
                        );

         paragraph.SetFontColor(iText.Kernel.Colors.ColorConstants.DARK_GRAY);
         paragraph.SetFontSize(12f);

         int totalPages = pdfDoc.GetNumberOfPages();
         for (int page = 1; page <= totalPages; page++)
         {
            iText.Kernel.Geom.Rectangle pageSize = pdfDoc.GetPage(page).GetPageSize();
            float x = pageSize.GetWidth() / 2;
            float y = pageSize.GetBottom() + fdfWaterMark.Position;

            Canvas canvas = new Canvas(pdfDoc.GetPage(page), pageSize);
            canvas.ShowTextAligned(paragraph, x, y, iText.Layout.Properties.TextAlignment.CENTER);
         }
         pdfDoc.Close();

         return memoryStream.GetBuffer();
      }
   }
}