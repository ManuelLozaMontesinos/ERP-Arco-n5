using System;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class EmpresaSedeDTO : DTOBase
	{
		public override int Id => EmpresaSedeId;
		public int EmpresaSedeId { get; set; }
		public int EmpresaId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
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
		public string CodigoSunat { get; set; }
		public string EsCentral { get; set; }
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
		public int DistritoId { get; set; }
		public string Telefono { get; set; }
		public string Telefono2 { get; set; }
		public string Email { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
		public TipoViaDTO TipoViaDTO { get; set; }
		public TipoZonaDTO TipoZonaDTO { get; set; }
		public DistritoDTO DistritoDTO { get; set; }
	}
}
