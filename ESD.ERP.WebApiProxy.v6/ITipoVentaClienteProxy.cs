using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoVentaClienteProxy
	{
		TipoVentaClienteDTO GetById(TipoVentaClienteDTO dto);
		List<TipoVentaClienteDTO> Search(TipoVentaClienteDTO dto);
		TipoVentaClienteDTO SingleSearch(TipoVentaClienteDTO dto);
		int Insert(TipoVentaClienteDTO dto);
		int Update(TipoVentaClienteDTO dto);
		void Delete(TipoVentaClienteDTO dto);
	}
}
