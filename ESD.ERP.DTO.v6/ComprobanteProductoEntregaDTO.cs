using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ComprobanteProductoEntregaDTO : DTOBase
	{
		public override int Id => ComprobanteProductoEntregaId;
		public int ComprobanteProductoEntregaId { get; set; }
		public int ComprobanteProductoId { get; set; }
		public decimal Cantidad { get; set; }
		public string EntregaDespacho { get; set; }
		public DateTime Fecha { get; set; }
		public string Observacion { get; set; }
		public int PersonalId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ComprobanteProductoDTO ComprobanteProductoDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
	}
}
