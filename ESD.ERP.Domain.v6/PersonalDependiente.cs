using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("personal_depen")]
	public class PersonalDependiente : BaseEntity
	{
		public override int EntityId => PersonalDependienteId;
		[Key]
		[Column("personal_depen_id", Order = 1)]
		public int PersonalDependienteId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("tipo_doc_iden_id")]
		public int TipoDocumentoIdentidadId { get; set; }

		[Column("num_doc_iden")]
		public string NumeroDocumentoIdentidad { get; set; }

		[Column("pais_emisor_id")]
		public int? PaisEmisorId { get; set; }

		[Column("apellido_pat")]
		public string ApellidoPaterno { get; set; }

		[Column("apellido_mat")]
		public string ApellidoMaterno { get; set; }

		[Column("nombres")]
		public string Nombres { get; set; }

		[Column("fecha_nac")]
		public DateTime FechaNacimiento { get; set; }

		[Column("sexo_id")]
		public int SexoId { get; set; }

		[Column("vincu_fami_id")]
		public int VinculoFamiliarId { get; set; }

		[Column("tipo_d_v_fam_id")]
		public int TipoDocumentoVinculoFamiliarId { get; set; }

		[Column("num_doc_v_fam")]
		public string NumeroDocumentoVinculoFamiliar { get; set; }

		[Column("fecha_concep")]
		public DateTime? FechaConcepcion { get; set; }

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

		[Column("referencia")]
		public string Referencia { get; set; }

		[Column("distrito_id")]
		public int? DistritoId { get; set; }

		[Column("telefono")]
		public string Telefono { get; set; }

		[Column("email")]
		public string Email { get; set; }

		[Column("fecha_alta")]
		public DateTime? FechaAlta { get; set; }

		[Column("fecha_baja")]
		public DateTime? FechaBaja { get; set; }

		[Column("motivo_b_depen_id")]
		public int? MotivoBajaDependienteId { get; set; }

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
		public TipoDocumentoIdentidad TipoDocumentoIdentidad { get; set; }
		public PaisEmisor PaisEmisor { get; set; }
		public Sexo Sexo { get; set; }
		public VinculoFamiliar VinculoFamiliar { get; set; }
		public TipoDocumentoVinculoFamiliar TipoDocumentoVinculoFamiliar { get; set; }
		public TipoVia TipoVia { get; set; }
		public TipoZona TipoZona { get; set; }
		public Distrito Distrito { get; set; }
		public MotivoBajaDependiente MotivoBajaDependiente { get; set; }
	}
}
