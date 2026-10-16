using ESD.Core.Service.DTO;
using System;

namespace ESD.ERP.DTO
{
	public class CuadroCostoDTO : DTOBase
	{
		public override int Id => CuadroCostoId;

		public int CuadroCostoId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public int ListaPrecioId { get; set; }
		public int MesContableId { get; set; }
		public int ProductoId { get; set; }
		public decimal ValorPromedio { get; set; }
		public decimal ValorReposicion { get; set; }
		public DateTime? FechaUltimaCompra { get; set; }
		public decimal ValorUltimaCompra { get; set; }
		public decimal PrecioUnitario { get; set; }
		public decimal MargenA { get; set; }
		public decimal MargenB { get; set; }

		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
	}
}
