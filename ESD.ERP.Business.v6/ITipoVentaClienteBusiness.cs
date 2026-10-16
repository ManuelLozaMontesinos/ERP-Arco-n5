using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoVentaClienteBusiness
	{
		TipoVentaClienteDTO GetById(int id);
		List<TipoVentaClienteDTO> Search(TipoVentaClienteDTO dto);
		TipoVentaClienteDTO SingleSearch(TipoVentaClienteDTO dto);
		int Insert(TipoVentaClienteDTO dto);
		int Update(TipoVentaClienteDTO dto);
		void Delete(TipoVentaClienteDTO dto);
	}
}
