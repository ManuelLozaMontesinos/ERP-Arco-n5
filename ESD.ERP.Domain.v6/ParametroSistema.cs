using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("parametro_sistema")]
	public class ParametroSistema : BaseEntity
	{
		public override int EntityId => ParametroSistemaId;
		[Key]
		[Column("parametro_sistema_id", Order = 1)]
		public int ParametroSistemaId { get; set; }

		[Column("grupo_param_sistema_id")]
		public int? GrupoParametroSistemaId { get; set; }

		[Column("tipo_dato_id")]
		public int TipoDatoId { get; set; }

		[Column("parametro_sistema")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("rango_num_desde")]
		public decimal RangoNumericoDesde { get; set; }

		[Column("rango_num_hasta")]
		public decimal RangoNumericoHasta { get; set; }

		[Column("valor_numerico")]
		public decimal ValorNumerico { get; set; }

		[Column("valor_fecha")]
		public DateTime? ValorFecha { get; set; }

		[Column("valor_cadena")]
		public string ValorCadena { get; set; }

		[Column("orden_grupo")]
		public short OrdenGrupo { get; set; }

		[Column("tiene_mant")]
		public string TieneMantenimiento { get; set; }

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

		public GrupoParametroSistema GrupoParametroSistema { get; set; }
		public TipoDato TipoDato { get; set; }
	}
}
