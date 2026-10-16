using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("mov_alm_cierre")]
	public class MovimientoAlmacenCierre : BaseEntity
	{
		public override int EntityId => MovimientoAlmacenCierreId;
		[Key]
		[Column("mov_alm_cierre_id", Order = 1)]
		public int MovimientoAlmacenCierreId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("almacen_id")]
		public int AlmacenId { get; set; }

		[Column("mes_contable_id")]
		public int MesContableId { get; set; }

		[Column("estado_ape_cie_id")]
		public int EstadoAperturaCierreId { get; set; }

		[Column("fecha_cierre")]
		public DateTime? FechaCierre { get; set; }

		[Column("tiene_mov_p_cierre")]
		public string TieneMovimientoPostCierre { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public PeriodoEmpresaSede PeriodoEmpresaSede { get; set; }
		public Almacen Almacen { get; set; }
		public MesContable MesContable { get; set; }
		public EstadoAperturaCierre EstadoAperturaCierre { get; set; }
	}
}
