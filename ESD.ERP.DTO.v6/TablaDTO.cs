using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TablaDTO : DTOBase
	{
		public override int Id => TablaId;
		public int TablaId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string CorrelativoPorEmpresa { get; set; }
		public string CorrelativoPorEmpresaSede { get; set; }
		public int Correlativo { get; set; }
		public string Prefijo { get; set; }
		public string FormatoCorrelativo { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
