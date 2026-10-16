using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMovimientoAlmacenCorrelativoBusiness
	{
		MovimientoAlmacenCorrelativoDTO GetById(int id);
		List<MovimientoAlmacenCorrelativoDTO> Search(MovimientoAlmacenCorrelativoDTO dto);
		MovimientoAlmacenCorrelativoDTO SingleSearch(MovimientoAlmacenCorrelativoDTO dto);
		int Insert(MovimientoAlmacenCorrelativoDTO dto);
		int Update(MovimientoAlmacenCorrelativoDTO dto);
		void Delete(MovimientoAlmacenCorrelativoDTO dto);
		string ObtenerCorrelativo(MovimientoAlmacenCorrelativoDTO dto);
	}
}
