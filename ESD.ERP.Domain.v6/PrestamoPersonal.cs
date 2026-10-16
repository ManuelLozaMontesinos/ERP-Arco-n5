using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("presta_perso")]
	public class PrestamoPersonal : BaseEntity
	{
		public override int EntityId => PrestamoPersonalId;
		[Key]
		[Column("presta_perso_id", Order = 1)]
		public int PrestamoPersonalId { get; set; }

		[Column("presta_perso")]
		public string Codigo { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("moneda_id")]
		public int MonedaId { get; set; }

		[Column("monto_tc")]
		public decimal MontoTipoCambio { get; set; }

		[Column("monto")]
		public decimal Monto { get; set; }

		[Column("cant_cuotas")]
		public int CantidadCuotas { get; set; }

		[Column("fecha_inicio")]
		public DateTime FechaInicio { get; set; }

		[Column("dias_cuota")]
		public int DiasCuota { get; set; }

		[Column("tipo_prestamo_id")]
		public int TipoPrestamoId { get; set; }

		[Column("estado_prestamo_id")]
		public int EstadoPrestamoId { get; set; }

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

		public Personal Personal { get; set; }
		public Moneda Moneda { get; set; }
		public TipoPrestamo TipoPrestamo { get; set; }
		public EstadoPrestamo EstadoPrestamo { get; set; }
	}
}
