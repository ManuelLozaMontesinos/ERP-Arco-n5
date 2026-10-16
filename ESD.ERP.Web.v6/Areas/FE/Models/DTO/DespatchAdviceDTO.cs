using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.FE.Models.DTO
{
   public class DespatchAdviceDTO
   {

      public DespatchAdviceDTO()
      {
         Items = new List<DespatchLineDTO>();
      }

      public string SignedBy { get; set; }
      public int TipoComprobante { get; set; }
      public string Serie { get; set; }
      public string Numero { get; set; }
      public DateTime Fecha { get; set; }
      public DateTime Hora { get; set; }
      public string Observacion { get; set; }
      public string EmpresaRUC { get; set; }
      public string EmpresaNombre { get; set; }
      public string EmpresaNombreComercial { get; set; }
      public string ProveedorTipoDocumento { get; set; }
      public string ProveedorNombre { get; set; }
      public string ProveedorNumeroDocumento { get; set; }
      public string ClienteTipoDocumento { get; set; }
      public string ClienteNombre { get; set; }
      public string ClienteNumeroDocumento { get; set; }
      public string MotivoTrasladoCodigo { get; set; }
      public string EsTrasladoDesdeProveedor { get; set; }
      public decimal PesoBrutoTotal { get; set; }
      public DateTime FechaTraslado { get; set; }
      public string VehiculoPlaca { get; set; }
      public string VehiculoPlaca2 { get; set; }
      public string ConductorTipoDocumento { get; set; }
      public string ConductorNombre { get; set; }
      public string ConductorApellidoPaternoMaterno { get; set; }
      public string ConductorNumeroDocumento { get; set; }
      public string ConductorNumeroLicencia { get; set; }
      public string PuntoPartidaUbigeo { get; set; }
      public string PuntoPartidaDireccion { get; set; }
      public string PuntoPartidaCodigoSunat { get; set; }
      public string PuntoLlegadaUbigeo { get; set; }
      public string PuntoLlegadaDireccion { get; set; }
      public string PuntoLlegadaCodigoSunat { get; set; }
      public string TipoDocumentoComercialReferencia { get; set; }
      public string SerieDocumentoReferencia { get; set; }
      public string NumeroDocumentoReferencia { get; set; }

      public List<DespatchLineDTO> Items { get; set; }
   }
}