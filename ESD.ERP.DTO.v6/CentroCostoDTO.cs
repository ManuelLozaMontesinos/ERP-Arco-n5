using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class CentroCostoDTO : DTOBase
	{
		public override int Id => CentroCostoId;
		public int CentroCostoId { get; set; }
		public int EmpresaId { get; set; }
		public int? CentroCostoPadreId { get; set; }
		public string Nombre { get; set; }
		public string Sigla { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
		public CentroCostoDTO CentroCostoPadreDTO { get; set; }
	}
}
