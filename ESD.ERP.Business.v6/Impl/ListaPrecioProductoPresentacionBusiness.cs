using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ListaPrecioProductoPresentacionBusiness : IListaPrecioProductoPresentacionBusiness
	{
		private readonly IListaPrecioProductoPresentacionRepository listaPrecioProductoPresentacionRepository;

		public ListaPrecioProductoPresentacionBusiness(
			IListaPrecioProductoPresentacionRepository listaPrecioProductoPresentacionRepository
			)
		{
			this.listaPrecioProductoPresentacionRepository = listaPrecioProductoPresentacionRepository;
		}
		public ListaPrecioProductoPresentacionDTO GetById(int id)
		{
			try
			{
				ListaPrecioProductoPresentacion listaPrecioProductoPresentacion = listaPrecioProductoPresentacionRepository.GetById(id);

				ListaPrecioProductoPresentacionDTO listaPrecioProductoPresentacionDTO = Mapper.Map<ListaPrecioProductoPresentacionDTO>(listaPrecioProductoPresentacion);

				return listaPrecioProductoPresentacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ListaPrecioProductoPresentacionDTO> Search(ListaPrecioProductoPresentacionDTO dto)
		{
			try
			{
				List<ListaPrecioProductoPresentacion> listaPrecioProductoPresentacionList = listaPrecioProductoPresentacionRepository.Search(Mapper.Map<ListaPrecioProductoPresentacion>(dto));

				List<ListaPrecioProductoPresentacionDTO> listaPrecioProductoPresentacionDTOList = Mapper.Map<List<ListaPrecioProductoPresentacionDTO>>(listaPrecioProductoPresentacionList);

				return listaPrecioProductoPresentacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ListaPrecioProductoPresentacionDTO SingleSearch(ListaPrecioProductoPresentacionDTO dto)
		{
			try
			{
				ListaPrecioProductoPresentacion listaPrecioProductoPresentacion = listaPrecioProductoPresentacionRepository.SingleSearch(Mapper.Map<ListaPrecioProductoPresentacion>(dto));

				ListaPrecioProductoPresentacionDTO listaPrecioProductoPresentacionDTO = Mapper.Map<ListaPrecioProductoPresentacionDTO>(listaPrecioProductoPresentacion);

				return listaPrecioProductoPresentacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ListaPrecioProductoPresentacionDTO dto)
		{
			try
			{
				ListaPrecioProductoPresentacion listaPrecioProductoPresentacion = Mapper.Map<ListaPrecioProductoPresentacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = listaPrecioProductoPresentacionRepository.Insert(listaPrecioProductoPresentacion);

					dto.ListaPrecioProductoPresentacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ListaPrecioProductoPresentacionDTO dto)
		{
			try
			{
				ListaPrecioProductoPresentacion listaPrecioProductoPresentacion = Mapper.Map<ListaPrecioProductoPresentacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = listaPrecioProductoPresentacionRepository.Update(listaPrecioProductoPresentacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ListaPrecioProductoPresentacionDTO dto)
		{
			try
			{
				ListaPrecioProductoPresentacion listaPrecioProductoPresentacion = Mapper.Map<ListaPrecioProductoPresentacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					listaPrecioProductoPresentacionRepository.Delete(listaPrecioProductoPresentacion);

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
