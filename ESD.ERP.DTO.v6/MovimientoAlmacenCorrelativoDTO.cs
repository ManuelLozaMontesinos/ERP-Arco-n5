using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class MovimientoAlmacenCorrelativoDTO : DTOBase
	{
		public override int Id => MovimientoAlmacenCorrelativoId;
		public int MovimientoAlmacenCorrelativoId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public int AlmacenId { get; set; }
		public int TipoMovimientoAlmacenId { get; set; }
		public int Correlativo { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaIciond { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }
		public TipoMovimientoAlmacenDTO TipoMovimientoAlmacenDTO { get; set; }
	}
}
