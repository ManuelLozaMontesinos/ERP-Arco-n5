using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TipoNotaCreditoDTO : DTOBase
	{
		public override int Id => TipoNotaCreditoId;
		public int TipoNotaCreditoId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string AfectaAlmacen { get; set; }
		public string EsIngreso { get; set; }
		public string PermiteEditItem { get; set; }
		public string PermiteEditarCantidad { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
