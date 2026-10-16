using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("almacen")]
	public class Almacen : BaseEntity
	{
		public override int EntityId => AlmacenId;
		[Key]
		[Column("almacen_id", Order = 1)]
		public int AlmacenId { get; set; }

		[Column("empresa_sede_id")]
		public int EmpresaSedeId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("abrev")]
		public string Abreviatura { get; set; }

		[Column("es_venta")]
		public string EsVenta { get; set; }

		[Column("tipo_via_id")]
		public int TipoViaId { get; set; }

		[Column("nombre_via")]
		public string NombreVia { get; set; }

		[Column("numero_via")]
		public string NumeroVia { get; set; }

		[Column("dpto")]
		public string Departamento { get; set; }

		[Column("interior")]
		public string Interior { get; set; }

		[Column("manzana")]
		public string Manzana { get; set; }

		[Column("lote")]
		public string Lote { get; set; }

		[Column("kilometro")]
		public string Kilometro { get; set; }

		[Column("block")]
		public string Block { get; set; }

		[Column("etapa")]
		public string Etapa { get; set; }

		[Column("tipo_zona_id")]
		public int TipoZonaId { get; set; }

		[Column("nombre_zona")]
		public string NombreZona { get; set; }

		[Column("distrito_id")]
		public int DistritoId { get; set; }

		[Column("codigo_sunat")]
		public string CodigoSunat { get; set; }

		[Column("origen_registro_id")]
		public int OrigenRegistroId { get; set; }

		[Column("servidor_imp")]
		public string ServidorImpresion { get; set; }

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

		public EmpresaSede EmpresaSede { get; set; }
		public TipoVia TipoVia { get; set; }
		public TipoZona TipoZona { get; set; }
		public Distrito Distrito { get; set; }
		public OrigenRegistro OrigenRegistro { get; set; }

		public int NotAlmacenId { get; set; }
	}
}
