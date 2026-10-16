using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoCancelacionCuotaBusiness
	{
		TipoCancelacionCuotaDTO GetById(int id);
		List<TipoCancelacionCuotaDTO> Search(TipoCancelacionCuotaDTO dto);
		TipoCancelacionCuotaDTO SingleSearch(TipoCancelacionCuotaDTO dto);
		int Insert(TipoCancelacionCuotaDTO dto);
		int Update(TipoCancelacionCuotaDTO dto);
		void Delete(TipoCancelacionCuotaDTO dto);
		TipoCancelacionCuotaDTO ObtenerPorCodigo(string codigo);
	}
}
