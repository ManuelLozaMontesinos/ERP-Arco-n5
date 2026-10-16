using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace ESD.ERP.Business.Impl
{
	public class ListaPrecioProductoBusiness : IListaPrecioProductoBusiness
	{
		private readonly IListaPrecioProductoRepository listaPrecioProductoRepository;

		public ListaPrecioProductoBusiness(
			IListaPrecioProductoRepository listaPrecioProductoRepository
			)
		{
			this.listaPrecioProductoRepository = listaPrecioProductoRepository;
		}
		public ListaPrecioProductoDTO GetById(int id)
		{
			try
			{
				ListaPrecioProducto listaPrecioProducto = listaPrecioProductoRepository.GetById(id);

				ListaPrecioProductoDTO listaPrecioProductoDTO = Mapper.Map<ListaPrecioProductoDTO>(listaPrecioProducto);

				return listaPrecioProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ListaPrecioProductoDTO> Search(ListaPrecioProductoDTO dto)
		{
			try
			{
				List<ListaPrecioProducto> listaPrecioProductoList = listaPrecioProductoRepository.Search(Mapper.Map<ListaPrecioProducto>(dto));

				List<ListaPrecioProductoDTO> listaPrecioProductoDTOList = Mapper.Map<List<ListaPrecioProductoDTO>>(listaPrecioProductoList);

				return listaPrecioProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ListaPrecioProductoDTO SingleSearch(ListaPrecioProductoDTO dto)
		{
			try
			{
				ListaPrecioProducto listaPrecioProducto = listaPrecioProductoRepository.SingleSearch(Mapper.Map<ListaPrecioProducto>(dto));

				ListaPrecioProductoDTO listaPrecioProductoDTO = Mapper.Map<ListaPrecioProductoDTO>(listaPrecioProducto);

				return listaPrecioProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ListaPrecioProductoDTO dto)
		{
			try
			{
				ListaPrecioProducto listaPrecioProducto = Mapper.Map<ListaPrecioProducto>(dto);

				if (ValidarProductoExiste(dto)!)
                throw new Exception("Producto ya existe en lista de Precio");

                int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					listaPrecioProducto.Estado = EstadoConstante.ACTIVO;
					newId = listaPrecioProductoRepository.Insert(listaPrecioProducto);

					dto.ListaPrecioProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ListaPrecioProductoDTO dto)
		{
			try
			{
				ListaPrecioProducto listaPrecioProducto = Mapper.Map<ListaPrecioProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = listaPrecioProductoRepository.Update(listaPrecioProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ListaPrecioProductoDTO dto)
		{
			try
			{
				ListaPrecioProducto listaPrecioProducto = Mapper.Map<ListaPrecioProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					listaPrecioProductoRepository.Delete(listaPrecioProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarLote(List<ListaPrecioProductoDTO> list)
      {
			try
			{
				List<ListaPrecioProducto> listaPrecioProductoList = Mapper.Map<List<ListaPrecioProducto>>(list);

				using (TransactionScope tx = new TransactionScope())
				{
					listaPrecioProductoRepository.ActualizarLote(listaPrecioProductoList);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
	  }

        public bool ValidarProductoExiste(ListaPrecioProductoDTO dto)
        {
            try
            {
                List<ListaPrecioProductoDTO> lista = Search(dto);

                return lista.Any(x =>
                    x.ProductoId == dto.ProductoId &&
                    x.ListaPrecioId == dto.ListaPrecioId
                );
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<dynamic> ListarSaldoAlmacen(ListaPrecioProductoDTO dto)
		{
			try
			{
				List<dynamic> dynamicList = listaPrecioProductoRepository.ListarSaldoAlmacen(Mapper.Map<ListaPrecioProducto>(dto));

				return dynamicList;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
