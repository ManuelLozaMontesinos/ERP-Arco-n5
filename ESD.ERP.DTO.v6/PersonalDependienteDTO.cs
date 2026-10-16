using System;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PersonalDependienteDTO : DTOBase
	{
		public override int Id => PersonalDependienteId;
		public int PersonalDependienteId { get; set; }
		public int PersonalId { get; set; }
		public int TipoDocumentoIdentidadId { get; set; }
		public string NumeroDocumentoIdentidad { get; set; }
		public int? PaisEmisorId { get; set; }
		public string ApellidoPaterno { get; set; }
		public string ApellidoMaterno { get; set; }
		public string Nombres { get; set; }
		public DateTime FechaNacimiento { get; set; }
		public int SexoId { get; set; }
		public int VinculoFamiliarId { get; set; }
		public int TipoDocumentoVinculoFamiliarId { get; set; }
		public string NumeroDocumentoVinculoFamiliar { get; set; }
		public DateTime? FechaConcepcion { get; set; }
		public string Direccion
		{
			get
			{
				if (TipoViaDTO == null)
					return string.Empty;

				return string.Concat(TipoViaDTO.Abreviatura, NombreVia,
					(string.IsNullOrEmpty(NumeroVia) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, "NRO.", SeparadorConstante.ESPACIO, NumeroVia)),
					(string.IsNullOrEmpty(Manzana) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, "MZA.", SeparadorConstante.ESPACIO, Manzana)),
					(string.IsNullOrEmpty(Lote) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, "LOTE.", SeparadorConstante.ESPACIO, Lote)),
					(string.IsNullOrEmpty(Interior) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, "INT.", SeparadorConstante.ESPACIO, Interior)),
					(string.IsNullOrEmpty(Kilometro) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, "KM.", SeparadorConstante.ESPACIO, Kilometro)),
					(string.Concat(SeparadorConstante.ESPACIO, TipoZonaDTO.Abreviatura, SeparadorConstante.ESPACIO, NombreZona)),
					(string.Concat(SeparadorConstante.ESPACIO, DistritoDTO.ProvinciaDTO.DepartamentoDTO.Nombre, SeparadorConstante.ESPACIO, SeparadorConstante.GUION, SeparadorConstante.ESPACIO,
						DistritoDTO.ProvinciaDTO.Nombre, SeparadorConstante.ESPACIO, SeparadorConstante.GUION, SeparadorConstante.ESPACIO, DistritoDTO.Nombre))
					);
			}
		}
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
		public int? DistritoId { get; set; }
		public string Telefono { get; set; }
		public string Email { get; set; }
		public DateTime? FechaAlta { get; set; }
		public DateTime? FechaBaja { get; set; }
		public int? MotivoBajaDependienteId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public TipoDocumentoIdentidadDTO TipoDocumentoIdentidadDTO { get; set; }
		public PaisEmisorDTO PaisEmisorDTO { get; set; }
		public SexoDTO SexoDTO { get; set; }
		public VinculoFamiliarDTO VinculoFamiliarDTO { get; set; }
		public TipoDocumentoVinculoFamiliarDTO TipoDocumentoVinculoFamiliarDTO { get; set; }
		public TipoViaDTO TipoViaDTO { get; set; }
		public TipoZonaDTO TipoZonaDTO { get; set; }
		public DistritoDTO DistritoDTO { get; set; }
		public MotivoBajaDependienteDTO MotivoBajaDependienteDTO { get; set; }
	}
}
