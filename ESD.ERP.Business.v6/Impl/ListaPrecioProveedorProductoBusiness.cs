using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ListaPrecioProveedorProductoBusiness : IListaPrecioProveedorProductoBusiness
	{
		private readonly IListaPrecioProveedorProductoRepository listaPrecioProveedorProductoRepository;

		public ListaPrecioProveedorProductoBusiness(
			IListaPrecioProveedorProductoRepository listaPrecioProveedorProductoRepository
			)
		{
			this.listaPrecioProveedorProductoRepository = listaPrecioProveedorProductoRepository;
		}
		public ListaPrecioProveedorProductoDTO GetById(int id)
		{
			try
			{
				ListaPrecioProveedorProducto listaPrecioProveedorProducto = listaPrecioProveedorProductoRepository.GetById(id);

				ListaPrecioProveedorProductoDTO listaPrecioProveedorProductoDTO = Mapper.Map<ListaPrecioProveedorProductoDTO>(listaPrecioProveedorProducto);

				return listaPrecioProveedorProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ListaPrecioProveedorProductoDTO> Search(ListaPrecioProveedorProductoDTO dto)
		{
			try
			{
				List<ListaPrecioProveedorProducto> listaPrecioProveedorProductoList = listaPrecioProveedorProductoRepository.Search(Mapper.Map<ListaPrecioProveedorProducto>(dto));

				List<ListaPrecioProveedorProductoDTO> listaPrecioProveedorProductoDTOList = Mapper.Map<List<ListaPrecioProveedorProductoDTO>>(listaPrecioProveedorProductoList);

				return listaPrecioProveedorProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ListaPrecioProveedorProductoDTO SingleSearch(ListaPrecioProveedorProductoDTO dto)
		{
			try
			{
				ListaPrecioProveedorProducto listaPrecioProveedorProducto = listaPrecioProveedorProductoRepository.SingleSearch(Mapper.Map<ListaPrecioProveedorProducto>(dto));

				ListaPrecioProveedorProductoDTO listaPrecioProveedorProductoDTO = Mapper.Map<ListaPrecioProveedorProductoDTO>(listaPrecioProveedorProducto);

				return listaPrecioProveedorProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ListaPrecioProveedorProductoDTO dto)
		{
			try
			{
				ListaPrecioProveedorProducto listaPrecioProveedorProducto = Mapper.Map<ListaPrecioProveedorProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = listaPrecioProveedorProductoRepository.Insert(listaPrecioProveedorProducto);

					dto.ListaPrecioProveedorProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ListaPrecioProveedorProductoDTO dto)
		{
			try
			{
				ListaPrecioProveedorProducto listaPrecioProveedorProducto = Mapper.Map<ListaPrecioProveedorProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = listaPrecioProveedorProductoRepository.Update(listaPrecioProveedorProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ListaPrecioProveedorProductoDTO dto)
		{
			try
			{
				ListaPrecioProveedorProducto listaPrecioProveedorProducto = Mapper.Map<ListaPrecioProveedorProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					listaPrecioProveedorProductoRepository.Delete(listaPrecioProveedorProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<dynamic> ListarSaldoAlmacen(ListaPrecioProveedorProductoDTO dto)
      {
			try
			{
				List<dynamic> dynamicList = listaPrecioProveedorProductoRepository.ListarSaldoAlmacen(Mapper.Map<ListaPrecioProveedorProducto>(dto));

				return dynamicList;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
