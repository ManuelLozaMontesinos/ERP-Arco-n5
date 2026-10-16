using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoCuotaBusiness
	{
		EstadoCuotaDTO GetById(int id);
		List<EstadoCuotaDTO> Search(EstadoCuotaDTO dto);
		EstadoCuotaDTO SingleSearch(EstadoCuotaDTO dto);
		int Insert(EstadoCuotaDTO dto);
		int Update(EstadoCuotaDTO dto);
		void Delete(EstadoCuotaDTO dto);
		EstadoCuotaDTO ObtenerPorCodigo(string codigo);
	}
}
