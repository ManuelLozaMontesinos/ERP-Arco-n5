using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMotivoFinPeriodoBusiness
	{
		MotivoFinPeriodoDTO GetById(int id);
		List<MotivoFinPeriodoDTO> Search(MotivoFinPeriodoDTO dto);
		MotivoFinPeriodoDTO SingleSearch(MotivoFinPeriodoDTO dto);
		int Insert(MotivoFinPeriodoDTO dto);
		int Update(MotivoFinPeriodoDTO dto);
		void Delete(MotivoFinPeriodoDTO dto);
	}
}
