using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TipoProductoDTO : DTOBase
	{
		public override int Id => TipoProductoId;
		public int EmpresaId { get; set; }
		public int TipoProductoId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public short Correlativo { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
	}
}
