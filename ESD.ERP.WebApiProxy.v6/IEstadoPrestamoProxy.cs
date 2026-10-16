using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoPrestamoProxy
	{
		EstadoPrestamoDTO GetById(EstadoPrestamoDTO dto);
		List<EstadoPrestamoDTO> Search(EstadoPrestamoDTO dto);
		EstadoPrestamoDTO SingleSearch(EstadoPrestamoDTO dto);
		int Insert(EstadoPrestamoDTO dto);
		int Update(EstadoPrestamoDTO dto);
		void Delete(EstadoPrestamoDTO dto);
		EstadoPrestamoDTO ObtenerPorCodigo(EstadoPrestamoDTO dto);
	}
}
