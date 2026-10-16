using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PlanillaBusiness : IPlanillaBusiness
	{
		private readonly IPlanillaRepository planillaRepository;

		public PlanillaBusiness(
			IPlanillaRepository planillaRepository
			)
		{
			this.planillaRepository = planillaRepository;
		}
		public PlanillaDTO GetById(int id)
		{
			try
			{
				Planilla planilla = planillaRepository.GetById(id);

				PlanillaDTO planillaDTO = Mapper.Map<PlanillaDTO>(planilla);

				return planillaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PlanillaDTO> Search(PlanillaDTO dto)
		{
			try
			{
				List<Planilla> planillaList = planillaRepository.Search(Mapper.Map<Planilla>(dto));

				List<PlanillaDTO> planillaDTOList = Mapper.Map<List<PlanillaDTO>>(planillaList);

				return planillaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PlanillaDTO SingleSearch(PlanillaDTO dto)
		{
			try
			{
				Planilla planilla = planillaRepository.SingleSearch(Mapper.Map<Planilla>(dto));

				PlanillaDTO planillaDTO = Mapper.Map<PlanillaDTO>(planilla);

				return planillaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PlanillaDTO dto)
		{
			try
			{
				Planilla planilla = Mapper.Map<Planilla>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = planillaRepository.Insert(planilla);

					dto.PlanillaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PlanillaDTO dto)
		{
			try
			{
				Planilla planilla = Mapper.Map<Planilla>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = planillaRepository.Update(planilla);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PlanillaDTO dto)
		{
			try
			{
				Planilla planilla = Mapper.Map<Planilla>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					planillaRepository.Delete(planilla);

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
