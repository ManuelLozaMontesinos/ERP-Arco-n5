using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PlanillaProcesoRegularizacionBusiness : IPlanillaProcesoRegularizacionBusiness
	{
		private readonly IPlanillaProcesoRegularizacionRepository planillaProcesoRegularizacionRepository;

		public PlanillaProcesoRegularizacionBusiness(
			IPlanillaProcesoRegularizacionRepository planillaProcesoRegularizacionRepository
			)
		{
			this.planillaProcesoRegularizacionRepository = planillaProcesoRegularizacionRepository;
		}
		public PlanillaProcesoRegularizacionDTO GetById(int id)
		{
			try
			{
				PlanillaProcesoRegularizacion planillaProcesoRegularizacion = planillaProcesoRegularizacionRepository.GetById(id);

				PlanillaProcesoRegularizacionDTO planillaProcesoRegularizacionDTO = Mapper.Map<PlanillaProcesoRegularizacionDTO>(planillaProcesoRegularizacion);

				return planillaProcesoRegularizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PlanillaProcesoRegularizacionDTO> Search(PlanillaProcesoRegularizacionDTO dto)
		{
			try
			{
				List<PlanillaProcesoRegularizacion> planillaProcesoRegularizacionList = planillaProcesoRegularizacionRepository.Search(Mapper.Map<PlanillaProcesoRegularizacion>(dto));

				List<PlanillaProcesoRegularizacionDTO> planillaProcesoRegularizacionDTOList = Mapper.Map<List<PlanillaProcesoRegularizacionDTO>>(planillaProcesoRegularizacionList);

				return planillaProcesoRegularizacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PlanillaProcesoRegularizacionDTO SingleSearch(PlanillaProcesoRegularizacionDTO dto)
		{
			try
			{
				PlanillaProcesoRegularizacion planillaProcesoRegularizacion = planillaProcesoRegularizacionRepository.SingleSearch(Mapper.Map<PlanillaProcesoRegularizacion>(dto));

				PlanillaProcesoRegularizacionDTO planillaProcesoRegularizacionDTO = Mapper.Map<PlanillaProcesoRegularizacionDTO>(planillaProcesoRegularizacion);

				return planillaProcesoRegularizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PlanillaProcesoRegularizacionDTO dto)
		{
			try
			{
				PlanillaProcesoRegularizacion planillaProcesoRegularizacion = Mapper.Map<PlanillaProcesoRegularizacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = planillaProcesoRegularizacionRepository.Insert(planillaProcesoRegularizacion);

					dto.PlanillaProcesoRegularizacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PlanillaProcesoRegularizacionDTO dto)
		{
			try
			{
				PlanillaProcesoRegularizacion planillaProcesoRegularizacion = Mapper.Map<PlanillaProcesoRegularizacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = planillaProcesoRegularizacionRepository.Update(planillaProcesoRegularizacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PlanillaProcesoRegularizacionDTO dto)
		{
			try
			{
				PlanillaProcesoRegularizacion planillaProcesoRegularizacion = Mapper.Map<PlanillaProcesoRegularizacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					planillaProcesoRegularizacionRepository.Delete(planillaProcesoRegularizacion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
