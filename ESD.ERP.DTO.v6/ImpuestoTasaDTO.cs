using System;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ImpuestoTasaDTO : DTOBase
	{
		public override int Id => ImpuestoTasaId;
		public int ImpuestoTasaId { get; set; }
		public int TipoImpuestoId { get; set; }
		public string ImpuestoNombreTasa
      {
         get
         {
				if (TipoImpuestoDTO == null)
					return string.Empty;

				return string.Concat(TipoImpuestoDTO.Abreviatura,SeparadorConstante.ESPACIO,"(",
					string.Format(StringFormatConstant.DECIMAL, Tasa), SignoConstante.PORCENTAJE,")"
					);
         }
      }
		public decimal Tasa { get; set; }
		public DateTime FechaAlta { get; set; }
		public DateTime? FechaBaja { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public TipoImpuestoDTO TipoImpuestoDTO { get; set; }
		public int Periodo { get; set; }
	}
}
