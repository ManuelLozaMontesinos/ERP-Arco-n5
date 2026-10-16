using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("transp_condu")]
	public class TransportistaConductor : BaseEntity
	{
		public override int EntityId => TransportistaConductorId;
		[Key]
		[Column("transp_condu_id", Order = 1)]
		public int TransportistaConductorId { get; set; }

		[Column("transpo_id")]
		public int TransportistaId { get; set; }

		[Column("apellido_pat")]
		public string ApellidoPaterno { get; set; }

		[Column("apellido_mat")]
		public string ApellidoMaterno { get; set; }

		[Column("nombres")]
		public string Nombres { get; set; }

		[Column("tipo_doc_iden_id")]
		public int TipoDocumentoIdentidadId { get; set; }

		[Column("num_doc_iden")]
		public string NumeroDocumentoIdentidad { get; set; }

		[Column("num_licencia")]
		public string NumeroLicencia { get; set; }

		[Column("telefono")]
		public string Telefono { get; set; }

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

		public Transportista Transportista { get; set; }
		public TipoDocumentoIdentidad TipoDocumentoIdentidad { get; set; }
	}
}
