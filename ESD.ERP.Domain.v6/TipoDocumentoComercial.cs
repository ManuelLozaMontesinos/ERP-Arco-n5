using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tipo_doc_com")]
	public class TipoDocumentoComercial : BaseEntity
	{
		public override int EntityId => TipoDocumentoComercialId;
		[Key]
		[Column("tipo_doc_com_id", Order = 1)]
		public int TipoDocumentoComercialId { get; set; }

		[Column("tipo_doc_com")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("abrev")]
		public string Abreviatura { get; set; }

		[Column("codigo_sunat")]
		public string CodigoSunat { get; set; }

		[Column("es_interno")]
		public string EsInterno { get; set; }

		[Column("es_comp_pago")]
		public string EsComprobantePago { get; set; }

		[Column("es_electro")]
		public string EsElectronico { get; set; }

		[Column("es_canjeable")]
		public string EsCanjeable { get; set; }

		[Column("monto_max_sin_prov")]
		public decimal MontoMaximoSinProveedor { get; set; }

		[Column("validar_prove")]
		public string ValidarProveedor { get; set; }

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

		public List<string> TipoDocumentoComercialList { get; set; }
	}
}
