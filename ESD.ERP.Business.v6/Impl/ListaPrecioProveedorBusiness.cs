using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ListaPrecioProveedorBusiness : IListaPrecioProveedorBusiness
	{
		private readonly IListaPrecioProveedorRepository listaPrecioProveedorRepository;

		public ListaPrecioProveedorBusiness(
			IListaPrecioProveedorRepository listaPrecioProveedorRepository
			)
		{
			this.listaPrecioProveedorRepository = listaPrecioProveedorRepository;
		}
		public ListaPrecioProveedorDTO GetById(int id)
		{
			try
			{
				ListaPrecioProveedor listaPrecioProveedor = listaPrecioProveedorRepository.GetById(id);

				ListaPrecioProveedorDTO listaPrecioProveedorDTO = Mapper.Map<ListaPrecioProveedorDTO>(listaPrecioProveedor);

				return listaPrecioProveedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ListaPrecioProveedorDTO> Search(ListaPrecioProveedorDTO dto)
		{
			try
			{
				List<ListaPrecioProveedor> listaPrecioProveedorList = listaPrecioProveedorRepository.Search(Mapper.Map<ListaPrecioProveedor>(dto));

				List<ListaPrecioProveedorDTO> listaPrecioProveedorDTOList = Mapper.Map<List<ListaPrecioProveedorDTO>>(listaPrecioProveedorList);

				return listaPrecioProveedorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ListaPrecioProveedorDTO SingleSearch(ListaPrecioProveedorDTO dto)
		{
			try
			{
				ListaPrecioProveedor listaPrecioProveedor = listaPrecioProveedorRepository.SingleSearch(Mapper.Map<ListaPrecioProveedor>(dto));

				ListaPrecioProveedorDTO listaPrecioProveedorDTO = Mapper.Map<ListaPrecioProveedorDTO>(listaPrecioProveedor);

				return listaPrecioProveedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ListaPrecioProveedorDTO dto)
		{
			try
			{
				ListaPrecioProveedor listaPrecioProveedor = Mapper.Map<ListaPrecioProveedor>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = listaPrecioProveedorRepository.Insert(listaPrecioProveedor);

					dto.ListaPrecioProveedorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ListaPrecioProveedorDTO dto)
		{
			try
			{
				ListaPrecioProveedor listaPrecioProveedor = Mapper.Map<ListaPrecioProveedor>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = listaPrecioProveedorRepository.Update(listaPrecioProveedor);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ListaPrecioProveedorDTO dto)
		{
			try
			{
				ListaPrecioProveedor listaPrecioProveedor = Mapper.Map<ListaPrecioProveedor>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					listaPrecioProveedorRepository.Delete(listaPrecioProveedor);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<ListaPrecioProveedorDTO> ListarPorProveedorId(ListaPrecioProveedorDTO dto)
      {
			try
			{
				List<ListaPrecioProveedor> listaPrecioProveedorList = listaPrecioProveedorRepository.ListarPorProveedorId(Mapper.Map<ListaPrecioProveedor>(dto));

				List<ListaPrecioProveedorDTO> listaPrecioProveedorDTOList = Mapper.Map<List<ListaPrecioProveedorDTO>>(listaPrecioProveedorList);

				return listaPrecioProveedorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
