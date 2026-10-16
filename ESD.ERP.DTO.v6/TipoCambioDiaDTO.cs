using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TipoCambioDiaDTO : DTOBase
	{
		public override int Id => TipoCambioDiaId;
		public int TipoCambioDiaId { get; set; }
		public int TipoCambioId { get; set; }
		public DateTime? Fecha { get; set; }
		public int MonedaId { get; set; }
		public decimal MontoCompra { get; set; }
		public decimal MontoVenta { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public TipoCambioDTO TipoCambioDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
	}
}
