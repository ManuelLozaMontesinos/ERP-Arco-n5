using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ParametroSistemaDTO : DTOBase
	{
		public override int Id => ParametroSistemaId;
		public int ParametroSistemaId { get; set; }
		public int GrupoParametroSistemaId { get; set; }
		public int TipoDatoId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public decimal RangoNumericoDesde { get; set; }
		public decimal RangoNumericoHasta { get; set; }
		public decimal ValorNumerico { get; set; }
		public DateTime? ValorFecha { get; set; }
		public string ValorCadena { get; set; }
		public short OrdenGrupo { get; set; }
		public string TieneMantenimiento { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public GrupoParametroSistemaDTO GrupoParametroSistemaDTO { get; set; }
		public TipoDatoDTO TipoDatoDTO { get; set; }
	}
}
