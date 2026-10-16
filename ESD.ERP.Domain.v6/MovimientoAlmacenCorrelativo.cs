using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("mov_alm_corre")]
	public class MovimientoAlmacenCorrelativo : BaseEntity
	{
		public override int EntityId => MovimientoAlmacenCorrelativoId;
		[Key]
		[Column("mov_alm_corre_id", Order = 1)]
		public int MovimientoAlmacenCorrelativoId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("almacen_id")]
		public int AlmacenId { get; set; }

		[Column("tipo_mov_alma_id")]
		public int TipoMovimientoAlmacenId { get; set; }

		[Column("correla")]
		public int Correlativo { get; set; }

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
		public TipoMovimientoAlmacen TipoMovimientoAlmacen { get; set; }
	}
}
