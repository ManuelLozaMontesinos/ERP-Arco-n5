using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("orden_p_s_p")]
	public class OrdenProduccionSubProceso : BaseEntity
	{
		public override int EntityId => OrdenProduccionSubProcesoId;
		[Key]
		[Column("orden_p_s_p_id", Order = 1)]
		public int OrdenProduccionSubProcesoId { get; set; }

		[Column("orden_pro_id")]
		public int OrdenProduccionId { get; set; }

		[Column("sub_proce_p_id")]
		public int SubProcesoProduccionId { get; set; }

		[Column("fecha_ini")]
		public DateTime? FechaInicio { get; set; }

		[Column("fecha_fin")]
		public DateTime? FechaFin { get; set; }

		[Column("orden")]
		public int Orden { get; set; }

		[Column("personal_id")]
		public int? PersonalId { get; set; }

		[Column("maquina_id")]
		public int? MaquinaId { get; set; }

		[Column("sub_p_p_ant_id")]
		public int? SubProcesoProduccionAnteriorId { get; set; }

		[Column("sub_pro_ant_c")]
		public string SubProcesoProduccionAnteriorConcluido { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

		[Column("estado_o_p_s_p_id")]
		public int EstadoOrdenProduccionSubProcesoId { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("almacen_id")]
		public int AlmacenId { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		[Column("maquina_real_id")]
		public int MaquinaRealId { get; set; }
		
		public OrdenProduccion OrdenProduccion { get; set; }
		public SubProcesoProduccion SubProcesoProduccion { get; set; }
		public EstadoOrdenProduccionSubProceso EstadoOrdenProduccionSubProceso { get; set; }
		public Personal Personal { get; set; }
		public Maquina Maquina { get; set; }
		public Almacen Almacen { get; set; }
	}
}
