using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.FE.Models
{
   public class DocEmitidoViewModel : ViewModel
   {

      public ComprobanteDTO ComprobanteDTO { get; set; }

      public List<SelectListItem> TipoDocumentoComercialList { get; set; }

      public DateTime FechaDesde { get; set; }
      public DateTime FechaHasta { get; set; }
      public string TicketLote { get; set; }
      public List<ComprobanteDTO> ComprobanteDTOList { get; set; }
      public int DocumentosValidos { get; set; }
      public int DocumentosConError { get; set; }
      public string EstadoDocumento { get;set; }
   }
}
