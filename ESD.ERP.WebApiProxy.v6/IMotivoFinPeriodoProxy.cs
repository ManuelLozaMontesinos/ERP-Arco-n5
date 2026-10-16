using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMotivoFinPeriodoProxy
	{
		MotivoFinPeriodoDTO GetById(MotivoFinPeriodoDTO dto);
		List<MotivoFinPeriodoDTO> Search(MotivoFinPeriodoDTO dto);
		MotivoFinPeriodoDTO SingleSearch(MotivoFinPeriodoDTO dto);
		int Insert(MotivoFinPeriodoDTO dto);
		int Update(MotivoFinPeriodoDTO dto);
		void Delete(MotivoFinPeriodoDTO dto);
	}
}
