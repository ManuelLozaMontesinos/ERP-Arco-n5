using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IImpuestoTasaBusiness
	{
		ImpuestoTasaDTO GetById(int id);
		List<ImpuestoTasaDTO> Search(ImpuestoTasaDTO dto);
		ImpuestoTasaDTO SingleSearch(ImpuestoTasaDTO dto);
		int Insert(ImpuestoTasaDTO dto);
		int Update(ImpuestoTasaDTO dto);
		void Delete(ImpuestoTasaDTO dto);
	}
}
