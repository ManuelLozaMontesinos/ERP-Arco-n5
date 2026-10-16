using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("presta_cuota")]
	public class PrestamoCuota : BaseEntity
	{
		public override int EntityId => PrestamoCuotaId;
		[Key]
		[Column("presta_cuota_id", Order = 1)]
		public int PrestamoCuotaId { get; set; }

		[Column("presta_perso_id")]
		public int PrestamoPersonalId { get; set; }

		[Column("cuota")]
		public int Cuota { get; set; }

		[Column("monto")]
		public decimal Monto { get; set; }

		[Column("fecha_vcto")]
		public DateTime FechaVencimiento { get; set; }

		[Column("fecha_cancela")]
		public DateTime? FechaCancelacion { get; set; }

		[Column("tipo_can_cuota_id")]
		public int TipoCancelacionCuotaId { get; set; }

		[Column("estado_cuota_id")]
		public int EstadoCuotaId { get; set; }

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

		public PrestamoPersonal PrestamoPersonal { get; set; }
		public TipoCancelacionCuota TipoCancelacionCuota { get; set; }
		public EstadoCuota EstadoCuota { get; set; }
	}
}
