using ESD.Core;
using ESD.Core.Logging;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.WebApiProxy;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace ESD.ERP.Web.Infrastructure
{
   public class AppHelper
   {
      private readonly IParametroSistemaProxy parametroSistemaProxy;
      private readonly IExportarDataProxy exportarDataProxy;

      public AppHelper(
         IParametroSistemaProxy parametroSistemaProxy,
         IExportarDataProxy exportarDataProxy
         )
      {
         this.parametroSistemaProxy = parametroSistemaProxy;
         this.exportarDataProxy = exportarDataProxy;
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

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.Single(i => i.Codigo == ParametroSistemaConstante.HABILITAR_ENVIO_EMAIL);
         bool envioCorreoHabilitado = Convert.ToBoolean(ObtenerParametroSistemaValor(parametroSistemaDTO));

         if (!envioCorreoHabilitado)
         {
            return;
         }

         parametroSistemaDTO = parametroSistemaDTOList.Single(i => i.Codigo == ParametroSistemaConstante.SERVIDOR_EMAIL);
         string host = parametroSistemaDTO.ValorCadena;

         parametroSistemaDTO = parametroSistemaDTOList.Single(i => i.Codigo == ParametroSistemaConstante.PUERTO_SMTP);
         int port = Convert.ToInt32(parametroSistemaDTO.ValorNumerico);

         parametroSistemaDTO = parametroSistemaDTOList.Single(i => i.Codigo == ParametroSistemaConstante.USUARIO_EMAIL);
         string emailUserName = parametroSistemaDTO.ValorCadena;

         parametroSistemaDTO = parametroSistemaDTOList.Single(i => i.Codigo == ParametroSistemaConstante.CONTRASENA_EMAIL);
         string emailPassword = parametroSistemaDTO.ValorCadena;

         parametroSistemaDTO = parametroSistemaDTOList.Single(i => i.Codigo == ParametroSistemaConstante.USAR_SSL);
         bool usarSSL = Convert.ToBoolean(ObtenerParametroSistemaValor(parametroSistemaDTO));

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

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.Single(i => i.Codigo == ParametroSistemaConstante.SSRS_BASE_PATH);
         baseURL = parametroSistemaDTO.ValorCadena;

         parametroSistemaDTO = parametroSistemaDTOList.Single(i => i.Codigo == ParametroSistemaConstante.SSRS_USE_CREDENTIAL);
         useWebAppCredential = Convert.ToBoolean(ObtenerParametroSistemaValor(parametroSistemaDTO));

         parametroSistemaDTO = parametroSistemaDTOList.Single(i => i.Codigo == ParametroSistemaConstante.SSRS_USER);
         userName = Convert.ToString(ObtenerParametroSistemaValor(parametroSistemaDTO));

         parametroSistemaDTO = parametroSistemaDTOList.Single(i => i.Codigo == ParametroSistemaConstante.SSRS_PASSWORD);
         password = Convert.ToString(ObtenerParametroSistemaValor(parametroSistemaDTO));

         try
         {
            if (useWebAppCredential)
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

            byte[] fileBytes = webClient.DownloadData(reportURL);

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
            parameters.Add("reportName", reportName);

            byte[] fileBytes = exportarDataProxy.Exportar(parameters);

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
            parameters.Add("reportName", reportName);

            byte[] fileBytes = exportarDataProxy.ExportarXml(parameters);

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