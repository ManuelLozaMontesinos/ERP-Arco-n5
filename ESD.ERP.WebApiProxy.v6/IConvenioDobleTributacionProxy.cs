using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IConvenioDobleTributacionProxy
	{
		ConvenioDobleTributacionDTO GetById(ConvenioDobleTributacionDTO dto);
		List<ConvenioDobleTributacionDTO> Search(ConvenioDobleTributacionDTO dto);
		ConvenioDobleTributacionDTO SingleSearch(ConvenioDobleTributacionDTO dto);
		int Insert(ConvenioDobleTributacionDTO dto);
		int Update(ConvenioDobleTributacionDTO dto);
		void Delete(ConvenioDobleTributacionDTO dto);
		ConvenioDobleTributacionDTO ObtenerPorCodigo(ConvenioDobleTributacionDTO dto);
	}
}
