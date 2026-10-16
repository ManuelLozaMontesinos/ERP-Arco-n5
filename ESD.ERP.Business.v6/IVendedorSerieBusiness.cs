using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IVendedorSerieBusiness
	{
		VendedorSerieDTO GetById(int id);
		List<VendedorSerieDTO> Search(VendedorSerieDTO dto);
		VendedorSerieDTO SingleSearch(VendedorSerieDTO dto);
		int Insert(VendedorSerieDTO dto);
		int Update(VendedorSerieDTO dto);
		void Delete(VendedorSerieDTO dto);
		void ActualizarListaDocumentoComercialSerieId(VendedorSerieDTO dto);
		List<TipoDocumentoComercialDTO> ListarTipoDocumentoComercialPorVendedor(VendedorSerieDTO dto);
		List<AlmacenDTO> ListarAlmacenPorVendedor(VendedorSerieDTO dto);
	}
}
