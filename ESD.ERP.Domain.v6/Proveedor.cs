using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("proveedor")]
	public class Proveedor : BaseEntity
	{
		public override int EntityId => ProveedorId;
		[Key]
		[Column("proveedor_id", Order = 1)]
		public int ProveedorId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("proveedor")]
		public string Codigo { get; set; }

		[Column("tipo_nat_juri_id")]
		public int TipoNaturalezaJuridicaId { get; set; }

		[Column("razon_social")]
		public string RazonSocial { get; set; }

		[Column("abrev")]
		public string Abreviatura { get; set; }

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

		[Column("numero_ruc")]
		public string NumeroRuc { get; set; }

		[Column("numero_cci")]
		public string NumeroCCI { get; set; }

		[Column("contacto")]
		public string Contacto { get; set; }

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

		[Column("telefono")]
		public string Telefono { get; set; }

		[Column("distrito_id")]
		public int DistritoId { get; set; }

		[Column("email")]
		public string Email { get; set; }

		[Column("web")]
		public string Web { get; set; }

		[Column("usuario_id")]
		public int? UsuarioId { get; set; }

		[Column("tipo_prove_id")]
		public int TipoProveedorId { get; set; }

		[Column("cond_pago_id")]
		public int? CondicionPagoId { get; set; }

		[Column("origen_registro_id")]
		public int OrigenRegistroId { get; set; }

		[Column("es_local")]
		public string EsLocal { get; set; }
		[Column("es_prospecto")]
		public string EsProspecto { get; set; }
		[Column("tipo_doc_iden2_id")]
		public int? TipoDocumentoIdentidad2Id { get; set; }
		[Column("num_doc_iden2")]
		public string NumeroDocumentoIdentidad2 { get; set; }
		[Column("cargo")]
		public string Cargo { get; set; }
		[Column("fecha_nac")]
		public DateTime? FechaNacimiento { get; set; }
		[Column("tipo_prof_ofi_id")]
		public int? TipoProfesionOficioId { get; set; }
		[Column("empresa_sede_id")]
		public int? EmpresaSedeId { get; set; }
		[Column("tipo_venta_c_id")]
		public int? TipoVentaClienteId { get; set; }
		[Column("lead_source_id")]
		public int? LeadSourceId { get; set; }

		[Column("cod_externo")]
		public string CodigoExterno { get; set; }

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

		[Column("ultima_venta")]
		public DateTime? FechaUltimaVenta {  get; set; }

		public Empresa Empresa { get; set; }
		public TipoNaturalezaJuridica TipoNaturalezaJuridica { get; set; }
		public TipoDocumentoIdentidad TipoDocumentoIdentidad { get; set; }
		public TipoVia TipoVia { get; set; }
		public TipoZona TipoZona { get; set; }
		public Distrito Distrito { get; set; }
		public Usuario Usuario { get; set; }
		public OrigenRegistro OrigenRegistro { get; set; }
		public TipoProveedor TipoProveedor { get; set; }
		public CondicionPago CondicionPago { get; set; }
		public TipoDocumentoIdentidad TipoDocumentoIdentidad2 { get; set; }
		public TipoProfesionOficio TipoProfesionOficio { get; set; }
		public TipoVentaCliente TipoVentaCliente { get; set; }
		public LeadSource LeadSource { get; set; }
		public EmpresaSede EmpresaSede { get; set; }
		public int NotProveedorId { get; set; }
	}
}
