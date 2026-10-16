using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PlanillaProcesoHoraBusiness : IPlanillaProcesoHoraBusiness
	{
		private readonly IPlanillaProcesoHoraRepository planillaProcesoHoraRepository;

		public PlanillaProcesoHoraBusiness(
			IPlanillaProcesoHoraRepository planillaProcesoHoraRepository
			)
		{
			this.planillaProcesoHoraRepository = planillaProcesoHoraRepository;
		}
		public PlanillaProcesoHoraDTO GetById(int id)
		{
			try
			{
				PlanillaProcesoHora planillaProcesoHora = planillaProcesoHoraRepository.GetById(id);

				PlanillaProcesoHoraDTO planillaProcesoHoraDTO = Mapper.Map<PlanillaProcesoHoraDTO>(planillaProcesoHora);

				return planillaProcesoHoraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PlanillaProcesoHoraDTO> Search(PlanillaProcesoHoraDTO dto)
		{
			try
			{
				List<PlanillaProcesoHora> planillaProcesoHoraList = planillaProcesoHoraRepository.Search(Mapper.Map<PlanillaProcesoHora>(dto));

				List<PlanillaProcesoHoraDTO> planillaProcesoHoraDTOList = Mapper.Map<List<PlanillaProcesoHoraDTO>>(planillaProcesoHoraList);

				return planillaProcesoHoraDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PlanillaProcesoHoraDTO SingleSearch(PlanillaProcesoHoraDTO dto)
		{
			try
			{
				PlanillaProcesoHora planillaProcesoHora = planillaProcesoHoraRepository.SingleSearch(Mapper.Map<PlanillaProcesoHora>(dto));

				PlanillaProcesoHoraDTO planillaProcesoHoraDTO = Mapper.Map<PlanillaProcesoHoraDTO>(planillaProcesoHora);

				return planillaProcesoHoraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PlanillaProcesoHoraDTO dto)
		{
			try
			{
				PlanillaProcesoHora planillaProcesoHora = Mapper.Map<PlanillaProcesoHora>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = planillaProcesoHoraRepository.Insert(planillaProcesoHora);

					dto.PlanillaProcesoHoraId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PlanillaProcesoHoraDTO dto)
		{
			try
			{
				PlanillaProcesoHora planillaProcesoHora = Mapper.Map<PlanillaProcesoHora>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = planillaProcesoHoraRepository.Update(planillaProcesoHora);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PlanillaProcesoHoraDTO dto)
		{
			try
			{
				PlanillaProcesoHora planillaProcesoHora = Mapper.Map<PlanillaProcesoHora>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					planillaProcesoHoraRepository.Delete(planillaProcesoHora);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<dynamic> ListarHoraPivot(PlanillaProcesoHoraDTO dto)
		{
			try
			{
				List<dynamic> dynamicList = planillaProcesoHoraRepository.ListarHoraPivot(Mapper.Map<PlanillaProcesoHora>(dto));

				return dynamicList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ActualizarLote(List<PlanillaProcesoHoraDTO> list)
		{
			try
			{
				List<PlanillaProcesoHora> planillaProcesoHoraList = Mapper.Map<List<PlanillaProcesoHora>>(list);

				using (TransactionScope tx = new TransactionScope())
				{
					planillaProcesoHoraRepository.ActualizarLote(planillaProcesoHoraList);

					tx.Complete();
				}

			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ActualizarLotePorPersonal(List<PlanillaProcesoHoraDTO> list)
		{
			try
			{
				List<PlanillaProcesoHora> planillaProcesoHoraList = Mapper.Map<List<PlanillaProcesoHora>>(list);

				using (TransactionScope tx = new TransactionScope())
				{
					planillaProcesoHoraRepository.ActualizarLotePorPersonal(planillaProcesoHoraList);

					tx.Complete();
				}

			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarListaPersonalId(PlanillaProcesoHoraDTO dto)
      {
			try
			{
				PlanillaProcesoHora planillaProcesoHora = Mapper.Map<PlanillaProcesoHora>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					planillaProcesoHoraRepository.ActualizarListaPersonalId(planillaProcesoHora);

					tx.Complete();
				}

			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarDesdeMarcacion(PlanillaProcesoHoraDTO dto)
      {
			try
			{
				PlanillaProcesoHora planillaProcesoHora = Mapper.Map<PlanillaProcesoHora>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					planillaProcesoHoraRepository.ActualizarDesdeMarcacion(planillaProcesoHora);

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
