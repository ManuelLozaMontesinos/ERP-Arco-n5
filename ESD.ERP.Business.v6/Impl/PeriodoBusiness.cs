using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PeriodoBusiness : IPeriodoBusiness
	{
		private readonly IPeriodoRepository periodoRepository;

		public PeriodoBusiness(
			IPeriodoRepository periodoRepository
			)
		{
			this.periodoRepository = periodoRepository;
		}
		public PeriodoDTO GetById(int id)
		{
			try
			{
				Periodo periodo = periodoRepository.GetById(id);

				PeriodoDTO periodoDTO = Mapper.Map<PeriodoDTO>(periodo);

				return periodoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PeriodoDTO> Search(PeriodoDTO dto)
		{
			try
			{
				List<Periodo> periodoList = periodoRepository.Search(Mapper.Map<Periodo>(dto));

				List<PeriodoDTO> periodoDTOList = Mapper.Map<List<PeriodoDTO>>(periodoList);

				return periodoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PeriodoDTO SingleSearch(PeriodoDTO dto)
		{
			try
			{
				Periodo periodo = periodoRepository.SingleSearch(Mapper.Map<Periodo>(dto));

				PeriodoDTO periodoDTO = Mapper.Map<PeriodoDTO>(periodo);

				return periodoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PeriodoDTO dto)
		{
			try
			{
				Periodo periodo = periodoRepository.ObtenerPorCodigo(dto.Codigo);

				if (periodo != null)
					throw new Exception(SR.GetString("EX_periodo_ya_registrado", dto.Codigo));

				if (dto.HabilitarProcesoBatch == SiNoConstante.S)
				{
					periodo = new Periodo()
					{
						HabilitarProcesoBatch = dto.HabilitarProcesoBatch,
						NotPeriodoId = dto.PeriodoId
					};

					periodo = periodoRepository.SingleSearch(periodo);
					if (periodo != null)
						throw new Exception(SR.GetString("EX_proc_lotes_ya_habilitado"));
				}

				periodo = Mapper.Map<Periodo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = periodoRepository.Insert(periodo);

					dto.PeriodoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PeriodoDTO dto)
		{
			try
			{
				Periodo periodo = Mapper.Map<Periodo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = periodoRepository.Update(periodo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PeriodoDTO dto)
		{
			try
			{
				Periodo periodo = Mapper.Map<Periodo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					periodoRepository.Delete(periodo);

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
