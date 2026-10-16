using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class EstadoOrdenCompraDTO : DTOBase
	{
		public override int Id => EstadoOrdenCompraId;
		public int EstadoOrdenCompraId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }

		public List<string> EstadoOrdenCompraList { get; set; }
	}
}
