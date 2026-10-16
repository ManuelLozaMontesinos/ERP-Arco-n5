using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("cond_pago")]
	public class CondicionPago : BaseEntity
	{
		public override int EntityId => CondicionPagoId;
		[Key]
		[Column("cond_pago_id", Order = 1)]
		public int CondicionPagoId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("cond_pago")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("tipo_c_pago_id")]
		public int TipoCondicionPagoId { get; set; }

		[Column("dia")]
		public int Dia { get; set; }

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

		public Empresa Empresa { get; set; }
		public TipoCondicionPago TipoCondicionPago { get; set; }
	}
}
