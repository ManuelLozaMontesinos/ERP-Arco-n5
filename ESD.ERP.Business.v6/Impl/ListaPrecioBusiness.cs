using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ListaPrecioBusiness : IListaPrecioBusiness
	{
		private readonly IListaPrecioRepository listaPrecioRepository;

		public ListaPrecioBusiness(
			IListaPrecioRepository listaPrecioRepository
			)
		{
			this.listaPrecioRepository = listaPrecioRepository;
		}
		public ListaPrecioDTO GetById(int id)
		{
			try
			{
				ListaPrecio listaPrecio = listaPrecioRepository.GetById(id);

				ListaPrecioDTO listaPrecioDTO = Mapper.Map<ListaPrecioDTO>(listaPrecio);

				return listaPrecioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ListaPrecioDTO> Search(ListaPrecioDTO dto)
		{
			try
			{
				List<ListaPrecio> listaPrecioList = listaPrecioRepository.Search(Mapper.Map<ListaPrecio>(dto));

				List<ListaPrecioDTO> listaPrecioDTOList = Mapper.Map<List<ListaPrecioDTO>>(listaPrecioList);

				return listaPrecioDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ListaPrecioDTO SingleSearch(ListaPrecioDTO dto)
		{
			try
			{
				ListaPrecio listaPrecio = listaPrecioRepository.SingleSearch(Mapper.Map<ListaPrecio>(dto));

				ListaPrecioDTO listaPrecioDTO = Mapper.Map<ListaPrecioDTO>(listaPrecio);

				return listaPrecioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ListaPrecioDTO dto)
		{
			try
			{
				ListaPrecio listaPrecio = Mapper.Map<ListaPrecio>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = listaPrecioRepository.Insert(listaPrecio);

					dto.ListaPrecioId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ListaPrecioDTO dto)
		{
			try
			{
				ListaPrecio listaPrecio = Mapper.Map<ListaPrecio>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = listaPrecioRepository.Update(listaPrecio);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ListaPrecioDTO dto)
		{
			try
			{
				ListaPrecio listaPrecio = Mapper.Map<ListaPrecio>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					listaPrecioRepository.Delete(listaPrecio);

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
