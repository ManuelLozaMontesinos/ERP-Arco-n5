using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ClaseProductoDTO : DTOBase
	{
		public override int Id => ClaseProductoId;
		public int ClaseProductoId { get; set; }
		public int GrupoProductoId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public short Correlativo { get; set; }
		public int TipoDetraccionId { get; set; }
		public string Estado { get; set; }
		public DateTime FechaAlta { get; set; }
		public DateTime? FechaBaja { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public GrupoProductoDTO GrupoProductoDTO { get; set; }
		public TipoDetraccionDTO TipoDetraccionDTO { get; set; }
		public int NotClaseProductoId { get; set; }
		public int Periodo { get; set; }
	}
}
