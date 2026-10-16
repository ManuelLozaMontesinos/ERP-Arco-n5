using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoCuotaProxy
	{
		EstadoCuotaDTO GetById(EstadoCuotaDTO dto);
		List<EstadoCuotaDTO> Search(EstadoCuotaDTO dto);
		EstadoCuotaDTO SingleSearch(EstadoCuotaDTO dto);
		int Insert(EstadoCuotaDTO dto);
		int Update(EstadoCuotaDTO dto);
		void Delete(EstadoCuotaDTO dto);
		EstadoCuotaDTO ObtenerPorCodigo(EstadoCuotaDTO dto);
	}
}
