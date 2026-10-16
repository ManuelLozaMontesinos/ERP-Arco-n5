using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("impuesto_tasa")]
	public class ImpuestoTasa : BaseEntity
	{
		public override int EntityId => ImpuestoTasaId;
		[Key]
		[Column("impuesto_tasa_id", Order = 1)]
		public int ImpuestoTasaId { get; set; }

		[Column("tipo_impuesto_id")]
		public int TipoImpuestoId { get; set; }

		[Column("tasa")]
		public decimal Tasa { get; set; }

		[Column("fecha_alta")]
		public DateTime FechaAlta { get; set; }

		[Column("fecha_baja")]
		public DateTime? FechaBaja { get; set; }

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

		public TipoImpuesto TipoImpuesto { get; set; }
		public int Periodo { get; set; }
	}
}
