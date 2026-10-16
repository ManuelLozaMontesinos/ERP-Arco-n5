using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("compro_cobra")]
	public class ComprobanteCobranza : BaseEntity
	{
		public override int EntityId => ComprobanteCobranzaId;
		[Key]
		[Column("compro_cobra_id", Order = 1)]
		public int ComprobanteCobranzaId { get; set; }

		[Column("comprobante_id")]
		public int ComprobanteId { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("numero_opera")]
		public string NumeroOperacion { get; set; }

		[Column("monto")]
		public decimal Monto { get; set; }

		[Column("empresa_cta_id")]
		public int EmpresaCuentaBancariaId { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificaId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModifica { get; set; }

		public Comprobante Comprobante { get; set; }
		public EmpresaCuentaBancaria EmpresaCuentaBancaria { get; set; }
	}
}
