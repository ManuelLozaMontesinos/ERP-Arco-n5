using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class MesContableDTO : DTOBase
	{
		public override int Id => MesContableId;
		public int MesContableId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string NombreInventario { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public List<string> NotMesContableList { get; set;}
	}
}
