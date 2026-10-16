using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class DiaBusiness : IDiaBusiness
	{
		private readonly IDiaRepository diaRepository;

		public DiaBusiness(
			IDiaRepository diaRepository
			)
		{
			this.diaRepository = diaRepository;
		}
		public DiaDTO GetById(int id)
		{
			try
			{
				Dia dia = diaRepository.GetById(id);

				DiaDTO diaDTO = Mapper.Map<DiaDTO>(dia);

				return diaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<DiaDTO> Search(DiaDTO dto)
		{
			try
			{
				List<Dia> diaList = diaRepository.Search(Mapper.Map<Dia>(dto));

				List<DiaDTO> diaDTOList = Mapper.Map<List<DiaDTO>>(diaList);

				return diaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public DiaDTO SingleSearch(DiaDTO dto)
		{
			try
			{
				Dia dia = diaRepository.SingleSearch(Mapper.Map<Dia>(dto));

				DiaDTO diaDTO = Mapper.Map<DiaDTO>(dia);

				return diaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(DiaDTO dto)
		{
			try
			{
				Dia dia = Mapper.Map<Dia>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = diaRepository.Insert(dia);

					dto.DiaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(DiaDTO dto)
		{
			try
			{
				Dia dia = Mapper.Map<Dia>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = diaRepository.Update(dia);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(DiaDTO dto)
		{
			try
			{
				Dia dia = Mapper.Map<Dia>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					diaRepository.Delete(dia);

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
