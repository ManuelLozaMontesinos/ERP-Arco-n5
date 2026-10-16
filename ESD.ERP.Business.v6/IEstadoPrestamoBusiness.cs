using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoPrestamoBusiness
	{
		EstadoPrestamoDTO GetById(int id);
		List<EstadoPrestamoDTO> Search(EstadoPrestamoDTO dto);
		EstadoPrestamoDTO SingleSearch(EstadoPrestamoDTO dto);
		int Insert(EstadoPrestamoDTO dto);
		int Update(EstadoPrestamoDTO dto);
		void Delete(EstadoPrestamoDTO dto);
		EstadoPrestamoDTO ObtenerPorCodigo(string codigo);
	}
}
