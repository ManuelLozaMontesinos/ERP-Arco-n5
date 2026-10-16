using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("mov_reclasi")]
	public class MovimientoReclasificacion : BaseEntity
	{
		public override int EntityId => MovimientoReclasificacionId;
		[Key]
		[Column("mov_reclasi_id", Order = 1)]
		public int MovimientoReclasificacionId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("mov_reclasi")]
		public string Codigo { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("mes_contable_id")]
		public int MesContableId { get; set; }

		[Column("mov_almacen_id")]
		public int MovimientoAlmacenId { get; set; }

		[Column("observacion")]
		public string Observacion { get; set; }

		[Column("estado_mov_rec_id")]
		public int EstadoMovimientoReclasificacionId { get; set; }

		[Column("usuario_autoriza_id")]
		public int? UsuarioAutorizaId { get; set; }
		[Column("fecha_auto")]
		public DateTime FechaAuto {  get; set; }

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
		public PeriodoEmpresaSede PeriodoEmpresaSede { get; set; }
		public MovimientoAlmacen MovimientoAlmacen { get; set; }
		public MesContable MesContable { get; set; }
		public EstadoMovimientoReclasificacion EstadoMovimientoReclasificacion { get; set; }
		public Usuario UsuarioAutoriza {  get; set; }

		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
	}
}
