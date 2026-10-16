using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoCuentaBancoBusiness
	{
		TipoCuentaBancoDTO GetById(int id);
		List<TipoCuentaBancoDTO> Search(TipoCuentaBancoDTO dto);
		TipoCuentaBancoDTO SingleSearch(TipoCuentaBancoDTO dto);
		int Insert(TipoCuentaBancoDTO dto);
		int Update(TipoCuentaBancoDTO dto);
		void Delete(TipoCuentaBancoDTO dto);
	}
}
