using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class RelojMarcacionBusiness : IRelojMarcacionBusiness
	{
		private readonly IRelojMarcacionRepository relojMarcacionRepository;

		public RelojMarcacionBusiness(
			IRelojMarcacionRepository relojMarcacionRepository
			)
		{
			this.relojMarcacionRepository = relojMarcacionRepository;
		}
		public RelojMarcacionDTO GetById(int id)
		{
			try
			{
				RelojMarcacion relojMarcacion = relojMarcacionRepository.GetById(id);

				RelojMarcacionDTO relojMarcacionDTO = Mapper.Map<RelojMarcacionDTO>(relojMarcacion);

				return relojMarcacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RelojMarcacionDTO> Search(RelojMarcacionDTO dto)
		{
			try
			{
				List<RelojMarcacion> relojMarcacionList = relojMarcacionRepository.Search(Mapper.Map<RelojMarcacion>(dto));

				List<RelojMarcacionDTO> relojMarcacionDTOList = Mapper.Map<List<RelojMarcacionDTO>>(relojMarcacionList);

				return relojMarcacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RelojMarcacionDTO SingleSearch(RelojMarcacionDTO dto)
		{
			try
			{
				RelojMarcacion relojMarcacion = relojMarcacionRepository.SingleSearch(Mapper.Map<RelojMarcacion>(dto));

				RelojMarcacionDTO relojMarcacionDTO = Mapper.Map<RelojMarcacionDTO>(relojMarcacion);

				return relojMarcacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RelojMarcacionDTO dto)
		{
			try
			{
				RelojMarcacion relojMarcacion = Mapper.Map<RelojMarcacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = relojMarcacionRepository.Insert(relojMarcacion);

					dto.RelojMarcacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RelojMarcacionDTO dto)
		{
			try
			{
				RelojMarcacion relojMarcacion = Mapper.Map<RelojMarcacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = relojMarcacionRepository.Update(relojMarcacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RelojMarcacionDTO dto)
		{
			try
			{
				RelojMarcacion relojMarcacion = Mapper.Map<RelojMarcacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					relojMarcacionRepository.Delete(relojMarcacion);

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
