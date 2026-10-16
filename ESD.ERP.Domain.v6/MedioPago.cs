using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("medio_pago")]
	public class MedioPago : BaseEntity
	{
		public override int EntityId => MedioPagoId;
		[Key]
		[Column("medio_pago_id", Order = 1)]
		public int MedioPagoId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("medio_pago")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("porcen_tarj")]
		public decimal PorcentajeTarjeta { get; set; }

		[Column("abrev")]
		public string Abreviatura { get; set; }

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

		public Empresa Empresa { get; set; }
	}
}
