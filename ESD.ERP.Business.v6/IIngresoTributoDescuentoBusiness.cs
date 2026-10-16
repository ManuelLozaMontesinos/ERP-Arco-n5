using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IIngresoTributoDescuentoBusiness
	{
		IngresoTributoDescuentoDTO GetById(int id);
		List<IngresoTributoDescuentoDTO> Search(IngresoTributoDescuentoDTO dto);
		IngresoTributoDescuentoDTO SingleSearch(IngresoTributoDescuentoDTO dto);
		int Insert(IngresoTributoDescuentoDTO dto);
		int Update(IngresoTributoDescuentoDTO dto);
		void Delete(IngresoTributoDescuentoDTO dto);
	}
}
