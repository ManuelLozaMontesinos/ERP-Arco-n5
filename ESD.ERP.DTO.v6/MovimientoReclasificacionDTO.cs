using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class MovimientoReclasificacionDTO : DTOBase
	{
		public override int Id => MovimientoReclasificacionId;
		public int MovimientoReclasificacionId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public string Codigo { get; set; }
		public DateTime Fecha { get; set; }
		public int MesContableId { get; set; }
		public int MovimientoAlmacenId { get; set; }
		public string Observacion { get; set; }
		public int EstadoMovimientoReclasificacionId { get; set; }
		public int? UsuarioAutorizaId { get; set; }
		public DateTime FechaAutoriza { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public MovimientoAlmacenDTO MovimientoAlmacenDTO { get; set; }
		public MesContableDTO MesContableDTO { get; set; }
		public EstadoMovimientoReclasificacionDTO EstadoMovimientoReclasificacionDTO { get; set; }
		public UsuarioDTO UsuarioAutorizaDTO { get; set; }

		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }

	}
}
