using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IConvenioDobleTributacionBusiness
	{
		ConvenioDobleTributacionDTO GetById(int id);
		List<ConvenioDobleTributacionDTO> Search(ConvenioDobleTributacionDTO dto);
		ConvenioDobleTributacionDTO SingleSearch(ConvenioDobleTributacionDTO dto);
		int Insert(ConvenioDobleTributacionDTO dto);
		int Update(ConvenioDobleTributacionDTO dto);
		void Delete(ConvenioDobleTributacionDTO dto);
		ConvenioDobleTributacionDTO ObtenerPorCodigo(string codigo);
	}
}
