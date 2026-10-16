using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoCancelacionCuotaProxy
	{
		TipoCancelacionCuotaDTO GetById(TipoCancelacionCuotaDTO dto);
		List<TipoCancelacionCuotaDTO> Search(TipoCancelacionCuotaDTO dto);
		TipoCancelacionCuotaDTO SingleSearch(TipoCancelacionCuotaDTO dto);
		int Insert(TipoCancelacionCuotaDTO dto);
		int Update(TipoCancelacionCuotaDTO dto);
		void Delete(TipoCancelacionCuotaDTO dto);
		TipoCancelacionCuotaDTO ObtenerPorCodigo(TipoCancelacionCuotaDTO dto);
	}
}
