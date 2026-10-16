using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPrestamoCuotaBusiness
	{
		PrestamoCuotaDTO GetById(int id);
		List<PrestamoCuotaDTO> Search(PrestamoCuotaDTO dto);
		PrestamoCuotaDTO SingleSearch(PrestamoCuotaDTO dto);
		int Insert(PrestamoCuotaDTO dto);
		int Update(PrestamoCuotaDTO dto);
		void Delete(PrestamoCuotaDTO dto);
		void BulkMerge(List<PrestamoCuotaDTO> list);
	}
}
