using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PlanillaDTO : DTOBase
	{
		public override int Id => PlanillaId;
		public int PlanillaId { get; set; }
		public int EmpresaId { get; set; }
		public string Nombre { get; set; }
		public short ProcesoAnual { get; set; }
		public int TipoTrabajadorId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
		public TipoTrabajadorDTO TipoTrabajadorDTO { get; set; }
	}
}
