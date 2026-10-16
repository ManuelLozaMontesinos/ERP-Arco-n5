using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("doc_com_serie")]
	public class DocumentoComercialSerie : BaseEntity
	{
		public override int EntityId => DocumentoComercialSerieId;
		[Key]
		[Column("doc_com_serie_id", Order = 1)]
		public int DocumentoComercialSerieId { get; set; }

		[Column("empresa_sede_id")]
		public int EmpresaSedeId { get; set; }

		[Column("tipo_doc_com_id")]
		public int TipoDocumentoComercialId { get; set; }

		[Column("serie")]
		public string Serie { get; set; }

		[Column("correlativo")]
		public int Correlativo { get; set; }

		[Column("es_contingencia")]
		public string EsContingencia { get; set; }

		[Column("envio_en_linea")]
		public string EnvioEnLinea { get; set; }

		[Column("formato")]
		public string Formato { get; set; }

		[Column("max_item_imp")]
		public short MaximoItemImpresion { get; set; }

		[Column("archivo_formato")]
		public string ArchivoFormato { get; set; }

		[Column("tipo_d_c_rel_id")]
		public int? TipoDocumentoComercialRelacionadoId { get; set; }

		[Column("almacen_id")]
		public int? AlmacenId { get; set; }

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
		public TipoDocumentoComercial TipoDocumentoComercial { get; set; }
		public Almacen Almacen { get; set; }
		public string AlmacenIdNotNull { get; set; }
	}
}
