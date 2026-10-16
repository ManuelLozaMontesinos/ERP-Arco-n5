using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("personal")]
	public class Personal : BaseEntity
	{
		public override int EntityId => PersonalId;
		[Key]
		[Column("personal_id", Order = 1)]
		public int PersonalId { get; set; }

		[Column("personal")]
		public string Codigo { get; set; }

		[Column("empresa_sede_id")]
		public int EmpresaSedeId { get; set; }

		[Column("pais_emisor_id")]
		public int? PaisEmisorId { get; set; }

		[Column("num_fotocheck")]
		public string NumeroFotocheck { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("sexo_id")]
		public int SexoId { get; set; }

		[Column("fecha_nac")]
		public DateTime FechaNacimiento { get; set; }

		[Column("pais_id")]
		public int PaisId { get; set; }

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
		public int DistritoId { get; set; }
		
		[Column("es_centro_asis")]
		public string EsCentroAsistencial { get; set; }

		[Column("domiciliado")]
		public string Domiciliado { get; set; }

		[Column("tipo_trabaja_id")]
		public int TipoTrabajadorId { get; set; }

		[Column("cate_ocupa_id")]
		public int? CategoriaOcupacionalId { get; set; }

		[Column("reg_laboral_id")]
		public int RegimenLaboralId { get; set; }

		[Column("situa_educa_id")]
		public int? SituacionEducativaId { get; set; }

		[Column("cargo_perso_id")]
		public int CargoPersonalId { get; set; }

		[Column("ocupacion_id")]
		public int OcupacionId { get; set; }

		[Column("reg_salud_id")]
		public int RegimenSaludId { get; set; }

		[Column("eps_id")]
		public int? EPSId { get; set; }

		[Column("reg_pension_id")]
		public int RegimenPensionarioId { get; set; }

		[Column("reg_pens_esq_id")]
		public int? RegimenPensionarioEsquemaId { get; set; }

		[Column("fecha_ins_reg_p")]
		public DateTime? FechaInscripcionRegimenPensionario { get; set; }

		[Column("num_cuspp")]
		public string NumeroCuspp { get; set; }

		[Column("aporta_sctr")]
		public string AportaSctr { get; set; }

		[Column("sctr_pension_id")]
		public int? SctrPensionId { get; set; }

		[Column("sctr_salud_id")]
		public int? SctrSaludId { get; set; }

		[Column("tipo_moda_forma_id")]
		public int? TipoModalidadFormativaId { get; set; }

		[Column("es_discapa")]
		public string EsDiscapacitado { get; set; }

		[Column("tipo_contrato_id")]
		public int TipoContratoId { get; set; }

		[Column("situ_espe_trab_id")]
		public int SituacionEspecialTrabajadorId { get; set; }

		[Column("peri_remu_id")]
		public int PeriodicidadRemunerativaId { get; set; }

		[Column("estado_civil_id")]
		public int EstadoCivilId { get; set; }

		[Column("tipo_pago_remu_id")]
		public int TipoPagoRemuneracionId { get; set; }

		[Column("banco_id")]
		public int? BancoId { get; set; }

		[Column("num_cta_abo")]
		public string NumeroCuentaAbono { get; set; }

		[Column("banco_cts_id")]
		public int? BancoCTSId { get; set; }

		[Column("num_cta_cts")]
		public string NumeroCuentaCTS { get; set; }

		[Column("es_regimen_atipico")]
		public string EsRegimenAtipico { get; set; }

		[Column("es_jorn_t_max")]
		public string EsJornadaTrabajoMaxima { get; set; }

		[Column("es_jorn_t_noc")]
		public string EsJornadaTrabajoNocturna { get; set; }

		[Column("es_miemb_sin")]
		public string EsMiembroSindicalizado { get; set; }

		[Column("es_exon_5ta")]
		public string EsExonerado5ta { get; set; }

		[Column("asigna_fami")]
		public string AsignacionFamiliar { get; set; }

		[Column("fecha_ing")]
		public DateTime FechaIngreso { get; set; }

		[Column("fecha_alta")]
		public DateTime? FechaAlta { get; set; }

		[Column("fecha_cese")]
		public DateTime? FechaCese { get; set; }

		[Column("motivo_fin_peri_id")]
		public int? MotivoFinPeriodoId { get; set; }

		[Column("centro_costo_id")]
		public int? CentroCostoId { get; set; }

		[Column("conve_d_trib_id")]
		public int ConvenioDobleTributacionId { get; set; }

		[Column("situa_trab_id")]
		public int SituacionTrabajadorId { get; set; }

		[Column("es_eventual")]
		public string EsEventual { get; set; }

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
		public PaisEmisor PaisEmisor { get; set; }
		public Proveedor Proveedor { get; set; }
		public Sexo Sexo { get; set; }
		public Pais Pais { get; set; }
		public TipoTrabajador TipoTrabajador { get; set; }
		public CategoriaOcupacional CategoriaOcupacional { get; set; }
		public RegimenLaboral RegimenLaboral { get; set; }
		public SituacionEducativa SituacionEducativa { get; set; }
		public CargoPersonal CargoPersonal { get; set; }
		public Ocupacion Ocupacion { get; set; }
		public RegimenSalud RegimenSalud { get; set; }
		public RegimenPensionario RegimenPensionario { get; set; }
		public RegimenPensionarioEsquema RegimenPensionarioEsquema { get; set; }
		public SctrPension SctrPension { get; set; }
		public SctrSalud SctrSalud { get; set; }
		public TipoModalidadFormativa TipoModalidadFormativa { get; set; }
		public TipoContrato TipoContrato { get; set; }
		public SituacionEspecialTrabajador SituacionEspecialTrabajador { get; set; }
		public PeriodicidadRemunerativa PeriodicidadRemunerativa { get; set; }
		public EstadoCivil EstadoCivil { get; set; }
		public TipoPagoRemuneracion TipoPagoRemuneracion { get; set; }
		public Banco Banco { get; set; }
		public Banco BancoCTS { get; set; }
		public MotivoFinPeriodo MotivoFinPeriodo { get; set; }
		public CentroCosto CentroCosto { get; set; }
		public ConvenioDobleTributacion ConvenioDobleTributacion { get; set; }
		public SituacionTrabajador SituacionTrabajador { get; set; }
		public TipoVia TipoVia { get; set; }
		public TipoZona TipoZona { get; set; }
		public Distrito Distrito { get; set; }

		public string NullFechaCese { get; set; }
	}
}
