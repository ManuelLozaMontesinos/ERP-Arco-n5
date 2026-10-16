using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PlanillaCalculoBusiness : IPlanillaCalculoBusiness
	{
		private readonly IPlanillaCalculoRepository planillaCalculoRepository;

		public PlanillaCalculoBusiness(
			IPlanillaCalculoRepository planillaCalculoRepository
			)
		{
			this.planillaCalculoRepository = planillaCalculoRepository;
		}
		public PlanillaCalculoDTO GetById(int id)
		{
			try
			{
				PlanillaCalculo planillaCalculo = planillaCalculoRepository.GetById(id);

				PlanillaCalculoDTO planillaCalculoDTO = Mapper.Map<PlanillaCalculoDTO>(planillaCalculo);

				return planillaCalculoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PlanillaCalculoDTO> Search(PlanillaCalculoDTO dto)
		{
			try
			{
				List<PlanillaCalculo> planillaCalculoList = planillaCalculoRepository.Search(Mapper.Map<PlanillaCalculo>(dto));

				List<PlanillaCalculoDTO> planillaCalculoDTOList = Mapper.Map<List<PlanillaCalculoDTO>>(planillaCalculoList);

				return planillaCalculoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PlanillaCalculoDTO SingleSearch(PlanillaCalculoDTO dto)
		{
			try
			{
				PlanillaCalculo planillaCalculo = planillaCalculoRepository.SingleSearch(Mapper.Map<PlanillaCalculo>(dto));

				PlanillaCalculoDTO planillaCalculoDTO = Mapper.Map<PlanillaCalculoDTO>(planillaCalculo);

				return planillaCalculoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PlanillaCalculoDTO dto)
		{
			try
			{
				PlanillaCalculo planillaCalculo = Mapper.Map<PlanillaCalculo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = planillaCalculoRepository.Insert(planillaCalculo);

					dto.PlanillaCalculoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PlanillaCalculoDTO dto)
		{
			try
			{
				PlanillaCalculo planillaCalculo = Mapper.Map<PlanillaCalculo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = planillaCalculoRepository.Update(planillaCalculo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PlanillaCalculoDTO dto)
		{
			try
			{
				PlanillaCalculo planillaCalculo = Mapper.Map<PlanillaCalculo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					planillaCalculoRepository.Delete(planillaCalculo);

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
