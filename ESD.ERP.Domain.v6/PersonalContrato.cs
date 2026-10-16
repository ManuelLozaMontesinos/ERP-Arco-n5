using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("personal_cont")]
	public class PersonalContrato : BaseEntity
	{
		public override int EntityId => PersonalContratoId;
		[Key]
		[Column("personal_cont_id", Order = 1)]
		public int PersonalContratoId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("fecha_susc")]
		public DateTime FechaSuscripcion { get; set; }

		[Column("fecha_inicio")]
		public DateTime FechaInicio { get; set; }

		[Column("fecha_fin")]
		public DateTime? FechaFin { get; set; }

		[Column("motivo_fin_peri_id")]
		public int? MotivoFinPeriodoId { get; set; }

		[Column("cargo_perso_id")]
		public int CargoPersonalId { get; set; }

		[Column("reg_laboral_id")]
		public int RegimenLaboralId { get; set; }

		[Column("tipo_contrato_id")]
		public int TipoContratoId { get; set; }

		[Column("es_discapa")]
		public string EsDiscapa { get; set; }

		[Column("moneda_id")]
		public int MonedaId { get; set; }

		[Column("monto")]
		public decimal Monto { get; set; }

		[Column("observa")]
		public string Observa { get; set; }

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
		public MotivoFinPeriodo MotivoFinPeriodo { get; set; }
		public CargoPersonal CargoPersonal { get; set; }
		public RegimenLaboral RegimenLaboral { get; set; }
		public TipoContrato TipoContrato { get; set; }
		public Moneda Moneda { get; set; }
	}
}
