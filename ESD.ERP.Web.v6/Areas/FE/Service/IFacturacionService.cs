using ESD.ERP.Web.Areas.FE.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace ESD.ERP.Web.Areas.FE.Service
{
   public interface IFacturacionService
   {
      void GenerarEnviarDocumento(string baseAddress, string tipoDocumentoComercial, int comprobanteId, string envioEnLinea);
      InvoiceDTO CreateInvoiceDTOFromData(DataSet data);
      DespatchAdviceDTO CreateDespatchAdviceDTOFromData(DataSet data);
      string GenerarDocumento(string baseAddress, string tipoDocumentoComercial, int comprobanteId);
      string EnviarDocumento(string baseAddress, string tipoDocumentoComercial, int comprobanteId);
      string ObtenerResumenTicket(string baseAddress, string ruc, DateTime fecha);
      string EnviarResumen(string baseAddress, int empresaId, string ruc, DateTime fecha);
      string ActualizarResumenCDR(string baseAddress, string ruc, string numeroTicket, List<int> comprobanteElectronicoIdList,
         List<int> notaElectronicaIdList);
      string ActualizarCDR(string baseAddress, string tipoDocumentoComercial, int comprobanteId);
      ResponseDTO ConsultarDocumentoEnviado(string baseAddress, int comprobanteElectronicoId);
   }
}