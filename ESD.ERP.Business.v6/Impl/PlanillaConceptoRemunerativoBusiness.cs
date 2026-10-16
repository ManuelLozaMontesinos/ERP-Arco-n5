using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PlanillaConceptoRemunerativoBusiness : IPlanillaConceptoRemunerativoBusiness
	{
		private readonly IPlanillaConceptoRemunerativoRepository planillaConceptoRemunerativoRepository;

		public PlanillaConceptoRemunerativoBusiness(
			IPlanillaConceptoRemunerativoRepository planillaConceptoRemunerativoRepository
			)
		{
			this.planillaConceptoRemunerativoRepository = planillaConceptoRemunerativoRepository;
		}
		public PlanillaConceptoRemunerativoDTO GetById(int id)
		{
			try
			{
				PlanillaConceptoRemunerativo planillaConceptoRemunerativo = planillaConceptoRemunerativoRepository.GetById(id);

				PlanillaConceptoRemunerativoDTO planillaConceptoRemunerativoDTO = Mapper.Map<PlanillaConceptoRemunerativoDTO>(planillaConceptoRemunerativo);

				return planillaConceptoRemunerativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PlanillaConceptoRemunerativoDTO> Search(PlanillaConceptoRemunerativoDTO dto)
		{
			try
			{
				List<PlanillaConceptoRemunerativo> planillaConceptoRemunerativoList = planillaConceptoRemunerativoRepository.Search(Mapper.Map<PlanillaConceptoRemunerativo>(dto));

				List<PlanillaConceptoRemunerativoDTO> planillaConceptoRemunerativoDTOList = Mapper.Map<List<PlanillaConceptoRemunerativoDTO>>(planillaConceptoRemunerativoList);

				return planillaConceptoRemunerativoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PlanillaConceptoRemunerativoDTO SingleSearch(PlanillaConceptoRemunerativoDTO dto)
		{
			try
			{
				PlanillaConceptoRemunerativo planillaConceptoRemunerativo = planillaConceptoRemunerativoRepository.SingleSearch(Mapper.Map<PlanillaConceptoRemunerativo>(dto));

				PlanillaConceptoRemunerativoDTO planillaConceptoRemunerativoDTO = Mapper.Map<PlanillaConceptoRemunerativoDTO>(planillaConceptoRemunerativo);

				return planillaConceptoRemunerativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PlanillaConceptoRemunerativoDTO dto)
		{
			try
			{
				PlanillaConceptoRemunerativo planillaConceptoRemunerativo = Mapper.Map<PlanillaConceptoRemunerativo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = planillaConceptoRemunerativoRepository.Insert(planillaConceptoRemunerativo);

					dto.PlanillaConceptoRemunerativoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PlanillaConceptoRemunerativoDTO dto)
		{
			try
			{
				PlanillaConceptoRemunerativo planillaConceptoRemunerativo = Mapper.Map<PlanillaConceptoRemunerativo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = planillaConceptoRemunerativoRepository.Update(planillaConceptoRemunerativo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PlanillaConceptoRemunerativoDTO dto)
		{
			try
			{
				PlanillaConceptoRemunerativo planillaConceptoRemunerativo = Mapper.Map<PlanillaConceptoRemunerativo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					planillaConceptoRemunerativoRepository.Delete(planillaConceptoRemunerativo);

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
