using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PeriodoDTO : DTOBase
	{
		public override int Id => PeriodoId;
		public int PeriodoId { get; set; }
		public int Codigo { get; set; }
		public string HabilitarProcesoBatch { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
