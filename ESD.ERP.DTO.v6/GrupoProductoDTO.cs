using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class GrupoProductoDTO : DTOBase
	{
		public override int Id => GrupoProductoId;
		public int GrupoProductoId { get; set; }
		public int TipoProductoId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public short Correlativo { get; set; }
		public string UsarIncrementoFraccion { get; set; }
		public string EsCombo { get; set; }
		public string EsInsumo { get; set; }
		public string Estado { get; set; }
		public DateTime FechaAlta { get; set; }
		public DateTime? FechaBaja { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public TipoProductoDTO TipoProductoDTO { get; set; }
		public int NotGrupoProductoId { get; set; }
		public int Periodo { get; set; }
	}
}
