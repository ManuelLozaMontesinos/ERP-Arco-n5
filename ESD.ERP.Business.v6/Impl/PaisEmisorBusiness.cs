using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PaisEmisorBusiness : IPaisEmisorBusiness
	{
		private readonly IPaisEmisorRepository paisEmisorRepository;

		public PaisEmisorBusiness(
			IPaisEmisorRepository paisEmisorRepository
			)
		{
			this.paisEmisorRepository = paisEmisorRepository;
		}
		public PaisEmisorDTO GetById(int id)
		{
			try
			{
				PaisEmisor paisEmisor = paisEmisorRepository.GetById(id);

				PaisEmisorDTO paisEmisorDTO = Mapper.Map<PaisEmisorDTO>(paisEmisor);

				return paisEmisorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PaisEmisorDTO> Search(PaisEmisorDTO dto)
		{
			try
			{
				List<PaisEmisor> paisEmisorList = paisEmisorRepository.Search(Mapper.Map<PaisEmisor>(dto));

				List<PaisEmisorDTO> paisEmisorDTOList = Mapper.Map<List<PaisEmisorDTO>>(paisEmisorList);

				return paisEmisorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PaisEmisorDTO SingleSearch(PaisEmisorDTO dto)
		{
			try
			{
				PaisEmisor paisEmisor = paisEmisorRepository.SingleSearch(Mapper.Map<PaisEmisor>(dto));

				PaisEmisorDTO paisEmisorDTO = Mapper.Map<PaisEmisorDTO>(paisEmisor);

				return paisEmisorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PaisEmisorDTO dto)
		{
			try
			{
				PaisEmisor paisEmisor = Mapper.Map<PaisEmisor>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = paisEmisorRepository.Insert(paisEmisor);

					dto.PaisEmisorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PaisEmisorDTO dto)
		{
			try
			{
				PaisEmisor paisEmisor = Mapper.Map<PaisEmisor>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = paisEmisorRepository.Update(paisEmisor);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PaisEmisorDTO dto)
		{
			try
			{
				PaisEmisor paisEmisor = Mapper.Map<PaisEmisor>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					paisEmisorRepository.Delete(paisEmisor);

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
