using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class MovimientoAlmacenCierreDTO : DTOBase
	{
		public override int Id => MovimientoAlmacenCierreId;
		public int MovimientoAlmacenCierreId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public int AlmacenId { get; set; }
		public int MesContableId { get; set; }
		public int EstadoAperturaCierreId { get; set; }
		public DateTime? FechaCierre { get; set; }
		public string TieneMovimientoPostCierre { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }
		public MesContableDTO MesContableDTO { get; set; }
		public EstadoAperturaCierreDTO EstadoAperturaCierreDTO { get; set; }
	}
}
