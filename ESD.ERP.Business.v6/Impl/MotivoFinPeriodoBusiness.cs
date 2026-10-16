using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MotivoFinPeriodoBusiness : IMotivoFinPeriodoBusiness
	{
		private readonly IMotivoFinPeriodoRepository motivoFinPeriodoRepository;

		public MotivoFinPeriodoBusiness(
			IMotivoFinPeriodoRepository motivoFinPeriodoRepository
			)
		{
			this.motivoFinPeriodoRepository = motivoFinPeriodoRepository;
		}
		public MotivoFinPeriodoDTO GetById(int id)
		{
			try
			{
				MotivoFinPeriodo motivoFinPeriodo = motivoFinPeriodoRepository.GetById(id);

				MotivoFinPeriodoDTO motivoFinPeriodoDTO = Mapper.Map<MotivoFinPeriodoDTO>(motivoFinPeriodo);

				return motivoFinPeriodoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MotivoFinPeriodoDTO> Search(MotivoFinPeriodoDTO dto)
		{
			try
			{
				List<MotivoFinPeriodo> motivoFinPeriodoList = motivoFinPeriodoRepository.Search(Mapper.Map<MotivoFinPeriodo>(dto));

				List<MotivoFinPeriodoDTO> motivoFinPeriodoDTOList = Mapper.Map<List<MotivoFinPeriodoDTO>>(motivoFinPeriodoList);

				return motivoFinPeriodoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MotivoFinPeriodoDTO SingleSearch(MotivoFinPeriodoDTO dto)
		{
			try
			{
				MotivoFinPeriodo motivoFinPeriodo = motivoFinPeriodoRepository.SingleSearch(Mapper.Map<MotivoFinPeriodo>(dto));

				MotivoFinPeriodoDTO motivoFinPeriodoDTO = Mapper.Map<MotivoFinPeriodoDTO>(motivoFinPeriodo);

				return motivoFinPeriodoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MotivoFinPeriodoDTO dto)
		{
			try
			{
				MotivoFinPeriodo motivoFinPeriodo = Mapper.Map<MotivoFinPeriodo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = motivoFinPeriodoRepository.Insert(motivoFinPeriodo);

					dto.MotivoFinPeriodoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MotivoFinPeriodoDTO dto)
		{
			try
			{
				MotivoFinPeriodo motivoFinPeriodo = Mapper.Map<MotivoFinPeriodo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = motivoFinPeriodoRepository.Update(motivoFinPeriodo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MotivoFinPeriodoDTO dto)
		{
			try
			{
				MotivoFinPeriodo motivoFinPeriodo = Mapper.Map<MotivoFinPeriodo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					motivoFinPeriodoRepository.Delete(motivoFinPeriodo);

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
