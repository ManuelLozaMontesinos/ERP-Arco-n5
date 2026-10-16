using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tipo_camb_dia")]
	public class TipoCambioDia : BaseEntity
	{
		public override int EntityId => TipoCambioDiaId;
		[Key]
		[Column("tipo_camb_dia_id", Order = 1)]
		public int TipoCambioDiaId { get; set; }

		[Column("tipo_cambio_id")]
		public int TipoCambioId { get; set; }

		[Column("fecha")]
		public DateTime? Fecha { get; set; }

		[Column("moneda_id")]
		public int MonedaId { get; set; }

		[Column("monto_c")]
		public decimal MontoCompra { get; set; }

		[Column("monto_v")]
		public decimal MontoVenta { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public TipoCambio TipoCambio { get; set; }
		public Moneda Moneda { get; set; }
	}
}
