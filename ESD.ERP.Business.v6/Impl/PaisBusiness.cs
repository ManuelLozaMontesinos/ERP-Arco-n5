using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PaisBusiness : IPaisBusiness
	{
		private readonly IPaisRepository paisRepository;

		public PaisBusiness(
			IPaisRepository paisRepository
			)
		{
			this.paisRepository = paisRepository;
		}
		public PaisDTO GetById(int id)
		{
			try
			{
				Pais pais = paisRepository.GetById(id);

				PaisDTO paisDTO = Mapper.Map<PaisDTO>(pais);

				return paisDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PaisDTO> Search(PaisDTO dto)
		{
			try
			{
				List<Pais> paisList = paisRepository.Search(Mapper.Map<Pais>(dto));

				List<PaisDTO> paisDTOList = Mapper.Map<List<PaisDTO>>(paisList);

				return paisDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PaisDTO SingleSearch(PaisDTO dto)
		{
			try
			{
				Pais pais = paisRepository.SingleSearch(Mapper.Map<Pais>(dto));

				PaisDTO paisDTO = Mapper.Map<PaisDTO>(pais);

				return paisDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PaisDTO dto)
		{
			try
			{
				Pais pais = Mapper.Map<Pais>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = paisRepository.Insert(pais);

					dto.PaisId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PaisDTO dto)
		{
			try
			{
				Pais pais = Mapper.Map<Pais>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = paisRepository.Update(pais);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PaisDTO dto)
		{
			try
			{
				Pais pais = Mapper.Map<Pais>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					paisRepository.Delete(pais);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PaisDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				Pais pais = paisRepository.ObtenerPorCodigo(codigo);

				PaisDTO paisDTO = Mapper.Map<PaisDTO>(pais);

				return paisDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
