using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IIngresoTributoDescuentoProxy
	{
		IngresoTributoDescuentoDTO GetById(IngresoTributoDescuentoDTO dto);
		List<IngresoTributoDescuentoDTO> Search(IngresoTributoDescuentoDTO dto);
		IngresoTributoDescuentoDTO SingleSearch(IngresoTributoDescuentoDTO dto);
		int Insert(IngresoTributoDescuentoDTO dto);
		int Update(IngresoTributoDescuentoDTO dto);
		void Delete(IngresoTributoDescuentoDTO dto);
	}
}
