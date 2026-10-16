using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class FeriadoBusiness : IFeriadoBusiness
	{
		private readonly IFeriadoRepository feriadoRepository;

		public FeriadoBusiness(
			IFeriadoRepository feriadoRepository
			)
		{
			this.feriadoRepository = feriadoRepository;
		}
		public FeriadoDTO GetById(int id)
		{
			try
			{
				Feriado feriado = feriadoRepository.GetById(id);

				FeriadoDTO feriadoDTO = Mapper.Map<FeriadoDTO>(feriado);

				return feriadoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<FeriadoDTO> Search(FeriadoDTO dto)
		{
			try
			{
				List<Feriado> feriadoList = feriadoRepository.Search(Mapper.Map<Feriado>(dto));

				List<FeriadoDTO> feriadoDTOList = Mapper.Map<List<FeriadoDTO>>(feriadoList);

				return feriadoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public FeriadoDTO SingleSearch(FeriadoDTO dto)
		{
			try
			{
				Feriado feriado = feriadoRepository.SingleSearch(Mapper.Map<Feriado>(dto));

				FeriadoDTO feriadoDTO = Mapper.Map<FeriadoDTO>(feriado);

				return feriadoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(FeriadoDTO dto)
		{
			try
			{
				Feriado feriado = Mapper.Map<Feriado>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = feriadoRepository.Insert(feriado);

					dto.FeriadoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(FeriadoDTO dto)
		{
			try
			{
				Feriado feriado = Mapper.Map<Feriado>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = feriadoRepository.Update(feriado);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(FeriadoDTO dto)
		{
			try
			{
				Feriado feriado = Mapper.Map<Feriado>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					feriadoRepository.Delete(feriado);

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
