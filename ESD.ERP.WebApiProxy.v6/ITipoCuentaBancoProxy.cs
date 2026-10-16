using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoCuentaBancoProxy
	{
		TipoCuentaBancoDTO GetById(TipoCuentaBancoDTO dto);
		List<TipoCuentaBancoDTO> Search(TipoCuentaBancoDTO dto);
		TipoCuentaBancoDTO SingleSearch(TipoCuentaBancoDTO dto);
		int Insert(TipoCuentaBancoDTO dto);
		int Update(TipoCuentaBancoDTO dto);
		void Delete(TipoCuentaBancoDTO dto);
	}
}
