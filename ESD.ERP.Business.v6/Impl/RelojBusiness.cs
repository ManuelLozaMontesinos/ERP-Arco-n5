using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class RelojBusiness : IRelojBusiness
	{
		private readonly IRelojRepository relojRepository;

		public RelojBusiness(
			IRelojRepository relojRepository
			)
		{
			this.relojRepository = relojRepository;
		}
		public RelojDTO GetById(int id)
		{
			try
			{
				Reloj reloj = relojRepository.GetById(id);

				RelojDTO relojDTO = Mapper.Map<RelojDTO>(reloj);

				return relojDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RelojDTO> Search(RelojDTO dto)
		{
			try
			{
				List<Reloj> relojList = relojRepository.Search(Mapper.Map<Reloj>(dto));

				List<RelojDTO> relojDTOList = Mapper.Map<List<RelojDTO>>(relojList);

				return relojDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RelojDTO SingleSearch(RelojDTO dto)
		{
			try
			{
				Reloj reloj = relojRepository.SingleSearch(Mapper.Map<Reloj>(dto));

				RelojDTO relojDTO = Mapper.Map<RelojDTO>(reloj);

				return relojDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RelojDTO dto)
		{
			try
			{
				Reloj reloj = Mapper.Map<Reloj>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = relojRepository.Insert(reloj);

					dto.RelojId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RelojDTO dto)
		{
			try
			{
				Reloj reloj = Mapper.Map<Reloj>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = relojRepository.Update(reloj);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RelojDTO dto)
		{
			try
			{
				Reloj reloj = Mapper.Map<Reloj>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					relojRepository.Delete(reloj);

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
