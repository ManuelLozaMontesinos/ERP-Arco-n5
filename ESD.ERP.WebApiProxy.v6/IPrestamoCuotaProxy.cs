using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPrestamoCuotaProxy
	{
		PrestamoCuotaDTO GetById(PrestamoCuotaDTO dto);
		List<PrestamoCuotaDTO> Search(PrestamoCuotaDTO dto);
		PrestamoCuotaDTO SingleSearch(PrestamoCuotaDTO dto);
		int Insert(PrestamoCuotaDTO dto);
		int Update(PrestamoCuotaDTO dto);
		void Delete(PrestamoCuotaDTO dto);
	}
}
