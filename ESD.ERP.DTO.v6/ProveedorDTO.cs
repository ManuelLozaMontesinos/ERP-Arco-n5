using System;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProveedorDTO : DTOBase
	{
		public override int Id => ProveedorId;
		public int ProveedorId { get; set; }
		public int EmpresaId { get; set; }
		public string Codigo { get; set; }
		public int TipoNaturalezaJuridicaId { get; set; }
		public string RazonSocial { get; set; }
		public string Abreviatura { get; set; }
		public string ApellidoPaterno { get; set; }
		public string ApellidoMaterno { get; set; }
		public string Nombres { get; set; }
		public string Direccion
		{
			get
			{
				if (TipoViaDTO == null)
					return string.Empty;

				return string.Concat(TipoViaDTO.Sigla, NombreVia,
					(string.IsNullOrEmpty(NumeroVia) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, "NRO.", SeparadorConstante.ESPACIO, NumeroVia)),
					(string.IsNullOrEmpty(Manzana) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, "MZA.", SeparadorConstante.ESPACIO, Manzana)),
					(string.IsNullOrEmpty(Lote) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, "LOTE.", SeparadorConstante.ESPACIO, Lote)),
					(string.IsNullOrEmpty(Interior) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, "INT.", SeparadorConstante.ESPACIO, Interior)),
					(string.IsNullOrEmpty(Kilometro) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, "KM.", SeparadorConstante.ESPACIO, Kilometro)),
					(string.Concat(SeparadorConstante.ESPACIO, TipoZonaDTO.Sigla, SeparadorConstante.ESPACIO, NombreZona)),
					(string.Concat(SeparadorConstante.ESPACIO, DistritoDTO.ProvinciaDTO.DepartamentoDTO.Nombre, SeparadorConstante.ESPACIO, SeparadorConstante.GUION, SeparadorConstante.ESPACIO,
						DistritoDTO.ProvinciaDTO.Nombre, SeparadorConstante.ESPACIO, SeparadorConstante.GUION, SeparadorConstante.ESPACIO, DistritoDTO.Nombre))
					);
			}
		}
		public int TipoDocumentoIdentidadId { get; set; }
		public string NumeroDocumentoIdentidad { get; set; }
		public string NumeroRuc { get; set; }
		public string NumeroCCI { get; set; }
		public string Contacto { get; set; }
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
		public string Telefono { get; set; }
		public int DistritoId { get; set; }
		public string Email { get; set; }
		public string Web { get; set; }
		public int? UsuarioId { get; set; }
		public int TipoProveedorId { get; set; }
		public int? CondicionPagoId { get; set; }
		public int OrigenRegistroId { get; set; }
		public string CodigoExterno { get; set; }
		public string EsLocal { get; set; }
		public string EsProspecto { get; set; }
		public int? TipoDocumentoIdentidad2Id { get; set; }
		public string NumeroDocumentoIdentidad2 { get; set; }
		public string Cargo { get; set; }
		public DateTime? FechaNacimiento { get; set; }
		public int? TipoProfesionOficioId { get; set; }
		public int? EmpresaSedeId { get; set; }
		public int? TipoVentaClienteId { get; set; }
		public int? LeadSourceId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public DateTime? FechaUltimaVenta { get; set; }

        public EmpresaDTO EmpresaDTO { get; set; }
		public TipoNaturalezaJuridicaDTO TipoNaturalezaJuridicaDTO { get; set; }
		public TipoDocumentoIdentidadDTO TipoDocumentoIdentidadDTO { get; set; }
		public TipoViaDTO TipoViaDTO { get; set; }
		public TipoZonaDTO TipoZonaDTO { get; set; }
		public DistritoDTO DistritoDTO { get; set; }
		public UsuarioDTO UsuarioDTO { get; set; }
		public OrigenRegistroDTO OrigenRegistroDTO { get; set; }
		public TipoProveedorDTO TipoProveedorDTO { get; set; }
		public CondicionPagoDTO CondicionPagoDTO { get; set; }
		public TipoDocumentoIdentidadDTO TipoDocumentoIdentidad2DTO { get; set; }
		public TipoProfesionOficioDTO TipoProfesionOficioDTO { get; set; }
		public TipoVentaClienteDTO TipoVentaClienteDTO { get; set; }
		public LeadSourceDTO LeadSourceDTO { get; set; }
		public EmpresaSedeDTO EmpresaSedeDTO { get; set; }
		public ContactosDTO ContactosDTO { get; set; }
    }
}
