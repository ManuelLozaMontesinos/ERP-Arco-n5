using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IVendedorSerieRepository : IGenericRepository<VendedorSerie>
	{
		void ActualizarListaDocumentoComercialSerieId(VendedorSerie entity);
	}
}
