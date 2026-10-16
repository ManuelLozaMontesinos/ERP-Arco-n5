using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class CategoriaOcupacionalBusiness : ICategoriaOcupacionalBusiness
	{
		private readonly ICategoriaOcupacionalRepository categoriaOcupacionalRepository;

		public CategoriaOcupacionalBusiness(
			ICategoriaOcupacionalRepository categoriaOcupacionalRepository
			)
		{
			this.categoriaOcupacionalRepository = categoriaOcupacionalRepository;
		}
		public CategoriaOcupacionalDTO GetById(int id)
		{
			try
			{
				CategoriaOcupacional categoriaOcupacional = categoriaOcupacionalRepository.GetById(id);

				CategoriaOcupacionalDTO categoriaOcupacionalDTO = Mapper.Map<CategoriaOcupacionalDTO>(categoriaOcupacional);

				return categoriaOcupacionalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<CategoriaOcupacionalDTO> Search(CategoriaOcupacionalDTO dto)
		{
			try
			{
				List<CategoriaOcupacional> categoriaOcupacionalList = categoriaOcupacionalRepository.Search(Mapper.Map<CategoriaOcupacional>(dto));

				List<CategoriaOcupacionalDTO> categoriaOcupacionalDTOList = Mapper.Map<List<CategoriaOcupacionalDTO>>(categoriaOcupacionalList);

				return categoriaOcupacionalDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public CategoriaOcupacionalDTO SingleSearch(CategoriaOcupacionalDTO dto)
		{
			try
			{
				CategoriaOcupacional categoriaOcupacional = categoriaOcupacionalRepository.SingleSearch(Mapper.Map<CategoriaOcupacional>(dto));

				CategoriaOcupacionalDTO categoriaOcupacionalDTO = Mapper.Map<CategoriaOcupacionalDTO>(categoriaOcupacional);

				return categoriaOcupacionalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(CategoriaOcupacionalDTO dto)
		{
			try
			{
				CategoriaOcupacional categoriaOcupacional = Mapper.Map<CategoriaOcupacional>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = categoriaOcupacionalRepository.Insert(categoriaOcupacional);

					dto.CategoriaOcupacionalId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(CategoriaOcupacionalDTO dto)
		{
			try
			{
				CategoriaOcupacional categoriaOcupacional = Mapper.Map<CategoriaOcupacional>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = categoriaOcupacionalRepository.Update(categoriaOcupacional);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(CategoriaOcupacionalDTO dto)
		{
			try
			{
				CategoriaOcupacional categoriaOcupacional = Mapper.Map<CategoriaOcupacional>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					categoriaOcupacionalRepository.Delete(categoriaOcupacional);

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
