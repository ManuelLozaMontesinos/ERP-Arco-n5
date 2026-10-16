using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class AtributoConfiguracionDTO : DTOBase
	{
		public override int Id => AtributoConfiguracionId;
		public int AtributoConfiguracionId { get; set; }
		public string Nombre { get; set; }
		public string Atributo { get; set; }
		public int Orden { get; set; }
		public string Editable { get; set; }
		public string TieneBoton { get; set; }
		public string EsGrupo { get; set; }
		public string AtributoLista { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
