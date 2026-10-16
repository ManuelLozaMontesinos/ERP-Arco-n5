using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IPlanillaCalculoRepository : IGenericRepository<PlanillaCalculo>
	{
		void CrearBase(PlanillaCalculo entity);
		void LimpiarBase(PlanillaCalculo entity);
		void CalcularConcepto(PlanillaCalculo entity);
		void AgregarPrestamoCuota(PlanillaCalculo entity);
		void AgregarRegularizacion(PlanillaCalculo entity);
	}
}
