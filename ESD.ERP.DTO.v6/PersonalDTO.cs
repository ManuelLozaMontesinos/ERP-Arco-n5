using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PersonalDTO : DTOBase
	{
		public override int Id => PersonalId;
		public int PersonalId { get; set; }
		public string Codigo { get; set; }
		public int EmpresaSedeId { get; set; }
		public int? PaisEmisorId { get; set; }
		public string NumeroFotocheck { get; set; }
		public int ProveedorId { get; set; }
		public int SexoId { get; set; }
		public DateTime FechaNacimiento { get; set; }
		public int PaisId { get; set; }
		public int TipoViaId { get; set; }
		public string NombreVia { get; set; }
		public string NumeroVia { get; set; }
		public string Departamento { get; set; }
		public string Interior { get; set; }
		public string Manzana { get; set; }
		public string Lote { get; set; }
		public string Kilometro { get; set; }
		public string Block { get; set; }
		public string Etapa { get; set; }
		public int TipoZonaId { get; set; }
		public string NombreZona { get; set; }
		public string Referencia { get; set; }
		public int DistritoId { get; set; }
		public string EsCentroAsistencial { get; set; }
		public string Domiciliado { get; set; }
		public int TipoTrabajadorId { get; set; }
		public int? CategoriaOcupacionalId { get; set; }
		public int RegimenLaboralId { get; set; }
		public int? SituacionEducativaId { get; set; }
		public int CargoPersonalId { get; set; }
		public int OcupacionId { get; set; }
		public int RegimenSaludId { get; set; }
		public int? EPSId { get; set; }
		public int RegimenPensionarioId { get; set; }
		public int? RegimenPensionarioEsquemaId { get; set; }
		public DateTime? FechaInscripcionRegimenPensionario { get; set; }
		public string NumeroCuspp { get; set; }
		public string AportaSctr { get; set; }
		public int? SctrPensionId { get; set; }
		public int? SctrSaludId { get; set; }
		public int? TipoModalidadFormativaId { get; set; }
		public string EsDiscapacitado { get; set; }
		public int TipoContratoId { get; set; }
		public int SituacionEspecialTrabajadorId { get; set; }
		public int PeriodicidadRemunerativaId { get; set; }
		public int EstadoCivilId { get; set; }
		public int TipoPagoRemuneracionId { get; set; }
		public int? BancoId { get; set; }
		public string NumeroCuentaAbono { get; set; }
		public int? BancoCTSId { get; set; }
		public string NumeroCuentaCTS { get; set; }
		public string EsRegimenAtipico { get; set; }
		public string EsJornadaTrabajoMaxima { get; set; }
		public string EsJornadaTrabajoNocturna { get; set; }
		public string EsMiembroSindicalizado { get; set; }
		public string EsExonerado5ta { get; set; }
		public string AsignacionFamiliar { get; set; }
		public DateTime FechaIngreso { get; set; }
		public DateTime? FechaAlta { get; set; }
		public DateTime? FechaCese { get; set; }
		public int? MotivoFinPeriodoId { get; set; }
		public int? CentroCostoId { get; set; }
		public int ConvenioDobleTributacionId { get; set; }
		public int SituacionTrabajadorId { get; set; }
		public string EsEventual { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaSedeDTO EmpresaSedeDTO { get; set; }
		public PaisEmisorDTO PaisEmisorDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public SexoDTO SexoDTO { get; set; }
		public PaisDTO PaisDTO { get; set; }
		public TipoTrabajadorDTO TipoTrabajaDTO { get; set; }
		public CategoriaOcupacionalDTO CategoriaOcupacionalDTO { get; set; }
		public RegimenLaboralDTO RegimenLaboralDTO { get; set; }
		public SituacionEducativaDTO SituacionEducativaDTO { get; set; }
		public CargoPersonalDTO CargoPersonalDTO { get; set; }
		public OcupacionDTO OcupacionDTO { get; set; }
		public RegimenSaludDTO RegimenSaludDTO { get; set; }
		public RegimenPensionarioDTO RegimenPensionarioDTO { get; set; }
		public RegimenPensionarioEsquemaDTO RegimenPensionarioEsquemaDTO { get; set; }
		public SctrPensionDTO SctrPensionDTO { get; set; }
		public SctrSaludDTO SctrSaludDTO { get; set; }
		public TipoModalidadFormativaDTO TipoModalidadFormativaDTO { get; set; }
		public TipoContratoDTO TipoContratoDTO { get; set; }
		public SituacionEspecialTrabajadorDTO SituacionEspecialTrabajadorDTO { get; set; }
		public PeriodicidadRemunerativaDTO PeriodicidadRemunerativaDTO { get; set; }
		public EstadoCivilDTO EstadoCivilDTO { get; set; }
		public TipoPagoRemuneracionDTO TipoPagoRemuneracionDTO { get; set; }
		public BancoDTO BancoDTO { get; set; }
		public BancoDTO BancoCTSDTO { get; set; }
		public MotivoFinPeriodoDTO MotivoFinPeriodoDTO { get; set; }
		public CentroCostoDTO CentroCostoDTO { get; set; }
		public SituacionTrabajadorDTO SituacionTrabajadorDTO { get; set; }
		public TipoViaDTO TipoViaDTO { get; set; }
		public TipoZonaDTO TipoZonaDTO { get; set; }
		public DistritoDTO DistritoDTO { get; set; }

		public int NotProveedorId { get; set; }
		public string NullFechaCese { get; set; }
	}
}
