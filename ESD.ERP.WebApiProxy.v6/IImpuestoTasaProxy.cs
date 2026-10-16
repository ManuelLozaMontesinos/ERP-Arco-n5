using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IImpuestoTasaProxy
	{
		ImpuestoTasaDTO GetById(ImpuestoTasaDTO dto);
		List<ImpuestoTasaDTO> Search(ImpuestoTasaDTO dto);
		ImpuestoTasaDTO SingleSearch(ImpuestoTasaDTO dto);
		int Insert(ImpuestoTasaDTO dto);
		int Update(ImpuestoTasaDTO dto);
		void Delete(ImpuestoTasaDTO dto);
	}
}
