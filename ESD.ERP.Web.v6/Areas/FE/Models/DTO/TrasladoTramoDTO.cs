using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.FE.Models.DTO
{
   public class TrasladoTramoDTO
   {

      public TrasladoTramoDTO()
      {
         this.Conductores = new List<ConductorDTO>();
         this.VehiculoSecundarios = new List<VehiculoSecundarioDTO>();
      }
      public string PuntoPartidaUbigeo { get; set; }
      public string PuntoPartidaDireccion { get; set; }
      public string PuntoPartidaUrbanizacion { get; set; }
      public string PuntoPartidaDepartamento { get; set; }
      public string PuntoPartidaProvincia { get; set; }
      public string PuntoPartidaDistrito { get; set; }
      public string PuntoPartidaPais { get; set; }

      public string PuntoLlegadaUbigeo { get; set; }
      public string PuntoLlegadaDireccion { get; set; }
      public string PuntoLlegadaUrbanizacion { get; set; }
      public string PuntoLlegadaDepartamento { get; set; }
      public string PuntoLlegadaProvincia { get; set; }
      public string PuntoLlegadaDistrito { get; set; }
      public string PuntoLlegadaPais { get; set; }

      public string TransportistaCHV { get; set; } //Constancia de Habilitacion Vehicular
      public string VehiculoPlaca { get; set; }
      public string VehiculoMarca { get; set; }

      public string TransportistaNumeroDocumento { get; set; }
      public string TransportistaTipoDocumento { get; set; }
      public string TransportistaNombre { get; set; }

      public string ConductorNumeroDocumento { get; set; }
      public string ConductorTipoDocumento { get; set; }
      public string ConductorLicencia { get; set; }

      /********************/
      public string TipoMedioTransporte { get; set; }
      public string TransitoFechaTraslado { get; set; }
      public string DatosContenedorNumeroContenedor { get; set; }
      public string PuertoAeropuertoEmbarqueCodigo { get; set; }
      public string PuertoAeropuertoEmbarqueNombre { get; set; }
      public string PuertoAeropuertoDesmbarqueCodigo { get; set; }
      public List<ConductorDTO> Conductores { get; set; }
      public List<VehiculoSecundarioDTO> VehiculoSecundarios { get; set; }
   }
}
