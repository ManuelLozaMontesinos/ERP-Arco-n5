using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.FE.Models.DTO
{
   public class InvoiceDTO
   {

      public InvoiceDTO()
      {
         Items = new List<InvoiceItemDTO>();
         TrasladoTramos = new List<TrasladoTramoDTO>();
         ElementosAdicionales = new List<ElementoAdicionalDTO>();
         FormaPagoCuotas = new List<FormaPagoCuotaDTO>();
      }

      public string SignedBy { get; set; }
      public string EnvioEnLinea { get; set; }
      public int TipoComprobante { get; set; }
      public string Serie { get; set; }
      public string Numero { get; set; }
      public DateTime Fecha { get; set; }
      public DateTime Hora { get; set; }
      public string TipoOperacion { get; set; }
      public string TipoMoneda { get; set; }
      public string Observacion { get; set; }
      public string GuiaBajaSerie { get; set; }
      public string GuiaBajaNumero { get; set; }
      public string GuiaBajaTipoDocumento { get; set; }
      public string GuiaBajaNombreDocumento { get; set; }
      public string DocumentoAdicionalSerie { get; set; }
      public string DocumentoAdicionalNumero { get; set; }
      public string DocumentoAdicionalTipoDocumento { get; set; }
      public string TercerosNombre { get; set; }
      public string TercerosNumeroDocumento { get; set; }
      public string TercerosTipoDocumento { get; set; }

      public string TrasladoIdentificador { get; set; }
      public string TrasladoCodigoMotivoTraslado { get; set; }
      public string TrasladoModalidad { get; set; }
      public string TrasladoDescripcionTraslado { get; set; }
      public string TrasladoIndicadorTransbordoProgramado { get; set; }
      public decimal TrasladoPesoBruto { get; set; }
      public string TrasladoUnidadMedida { get; set; }
      public decimal TrasladoCantidad { get; set; }

      public string ProveedorNombre { get; set; }
      public string ProveedorNombreComercial { get; set; }
      public string ProveedorDireccion { get; set; }
      public string ProveedorTipoDocumento { get; set; }
      public string CodigoEstablecimiento { get; set; }
      public string ProveedorRUC { get; set; }
      public string ProveedorUbigeo { get; set; }
      public string ProveedorUrbanizacionZona { get; set; }
      public string ProveedorDepartamento { get; set; }
      public string ProveedorProvincia { get; set; }
      public string ProveedorDistrito { get; set; }
      public string ProveedorPais { get; set; }
      public string ClienteNombre { get; set; }
      public string ClienteDireccion { get; set; }
      public string ClienteNumeroDocumento { get; set; }
      public string ClienteTipoDocumento { get; set; }

      public string DocumentoReferenciaSerie { get; set; }
      public string DocumentoReferenciaNumero { get; set; }
      public string TipoDocumentoReferencia { get; set; }
      public decimal DocumentoReferenciaMontoNeto { get; set; }
      public decimal DocumentoReferenciaMontoIGV { get; set; }
      public decimal DocumentoReferenciaMontoTotal { get; set; }
      public string TipoNota { get; set; }
      public string Sustento { get; set; }

      public decimal TotalGravadoDocumento { get; set; }
      public decimal TotalImpuestoDocumento { get; set; }
      public decimal TotalDocumento { get; set; }

      public decimal PorcentajeIGV { get; set; }
      public decimal TotalGravado { get; set; }
      public decimal TotalInafecto { get; set; }
      public decimal TotalExonerado { get; set; }
      public decimal TotalGratuito { get; set; }
      public decimal PorcentajeDescuentoGlobal { get; set; }
      public decimal TotalDescuentoGlobal { get; set; }
      public decimal TotalDescuentoItem { get; set; }
      public decimal TotalIGV { get; set; }
      public decimal TotalISC { get; set; }
      public decimal Total { get; set; }
      public decimal Percepcion { get; set; }
      public decimal TotalCobrado { get; set; }

      // -------- Para la detraccion ----------------------
      public string TipoDetraccion { get; set; }
      public decimal PorcentajeDetraccion { get; set; }
      public decimal MontoDetraccion { get; set; }
      public string CuentaDetraccion { get; set; }
      // -------------------------------------------------

      public string MontoLetras { get; set; }
      public List<ElementoAdicionalDTO> ElementosAdicionales { get; set; }

      public string OrdenCS { get; set; }
      public string FacturaGuiaRemision { get; set; }

      //public string TieneDeduccionAnticipo { get; set; }
      //public decimal Anticipo { get; set; }
      //public string DocumentoAnticipoNumero { get; set; }
      //public string DocumentoAnticipoTipoDocumento { get; set; }
      //public decimal TotalAnticipo { get; set; }

      public string EmisorItinerante { get; set; }

      public List<InvoiceItemDTO> Items { get; set; }
      public List<TrasladoTramoDTO> TrasladoTramos { get; set; }

      public string FormaPago { get; set; }
      public decimal MontoFormaPago { get; set; }
      public List<FormaPagoCuotaDTO> FormaPagoCuotas { get; set; }
   }
}
