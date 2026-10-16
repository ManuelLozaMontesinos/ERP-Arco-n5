using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMovimientoAlmacenCorrelativoProxy
	{
		MovimientoAlmacenCorrelativoDTO GetById(MovimientoAlmacenCorrelativoDTO dto);
		List<MovimientoAlmacenCorrelativoDTO> Search(MovimientoAlmacenCorrelativoDTO dto);
		MovimientoAlmacenCorrelativoDTO SingleSearch(MovimientoAlmacenCorrelativoDTO dto);
		int Insert(MovimientoAlmacenCorrelativoDTO dto);
		int Update(MovimientoAlmacenCorrelativoDTO dto);
		void Delete(MovimientoAlmacenCorrelativoDTO dto);
	}
}
