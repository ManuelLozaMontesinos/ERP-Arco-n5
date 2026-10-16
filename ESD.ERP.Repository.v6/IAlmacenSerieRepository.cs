using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IAlmacenSerieRepository : IGenericRepository<AlmacenSerie>
	{
		void ActualizarListaDocumentoComercialSerieId(AlmacenSerie entity);
	}
}
