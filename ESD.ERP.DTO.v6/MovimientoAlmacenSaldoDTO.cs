using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class MovimientoAlmacenSaldoDTO : DTOBase
	{
		public override int Id => MovimientoAlmacenSaldoId;
		public int MovimientoAlmacenSaldoId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public int AlmacenId { get; set; }
		public int ProductoId { get; set; }
		public int MesContableId { get; set; }
		public decimal CantidadAnterior { get; set; }
		public decimal ValorUnitarioAnterior { get; set; }
		public decimal CantidadIngreso { get; set; }
		public decimal CantidadSalida { get; set; }
		public decimal CantidadReserva { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal Saldo
      {
         get { return (CantidadAnterior + CantidadIngreso) - (CantidadSalida + CantidadReserva); }
      }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public MesContableDTO MesContableDTO { get; set; }
	}
}
