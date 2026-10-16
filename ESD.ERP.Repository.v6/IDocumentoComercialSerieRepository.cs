using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IDocumentoComercialSerieRepository : IGenericRepository<DocumentoComercialSerie>
	{
		void ActualizarCorrelativo(DocumentoComercialSerie entity);
	}
}
