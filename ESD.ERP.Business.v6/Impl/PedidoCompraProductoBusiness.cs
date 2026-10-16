using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PedidoCompraProductoBusiness : IPedidoCompraProductoBusiness
	{
		private readonly IPedidoCompraProductoRepository pedidoCompraProductoRepository;

		public PedidoCompraProductoBusiness(
			IPedidoCompraProductoRepository pedidoCompraProductoRepository
			)
		{
			this.pedidoCompraProductoRepository = pedidoCompraProductoRepository;
		}
		public PedidoCompraProductoDTO GetById(int id)
		{
			try
			{
				PedidoCompraProducto pedidoCompraProducto = pedidoCompraProductoRepository.GetById(id);

				PedidoCompraProductoDTO pedidoCompraProductoDTO = Mapper.Map<PedidoCompraProductoDTO>(pedidoCompraProducto);

				return pedidoCompraProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PedidoCompraProductoDTO> Search(PedidoCompraProductoDTO dto)
		{
			try
			{
				List<PedidoCompraProducto> pedidoCompraProductoList = pedidoCompraProductoRepository.Search(Mapper.Map<PedidoCompraProducto>(dto));

				List<PedidoCompraProductoDTO> pedidoCompraProductoDTOList = Mapper.Map<List<PedidoCompraProductoDTO>>(pedidoCompraProductoList);

				return pedidoCompraProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PedidoCompraProductoDTO SingleSearch(PedidoCompraProductoDTO dto)
		{
			try
			{
				PedidoCompraProducto pedidoCompraProducto = pedidoCompraProductoRepository.SingleSearch(Mapper.Map<PedidoCompraProducto>(dto));

				PedidoCompraProductoDTO pedidoCompraProductoDTO = Mapper.Map<PedidoCompraProductoDTO>(pedidoCompraProducto);

				return pedidoCompraProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PedidoCompraProductoDTO dto)
		{
			try
			{
				PedidoCompraProducto pedidoCompraProducto = Mapper.Map<PedidoCompraProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = pedidoCompraProductoRepository.Insert(pedidoCompraProducto);

					dto.PedidoCompraProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PedidoCompraProductoDTO dto)
		{
			try
			{
				PedidoCompraProducto pedidoCompraProducto = Mapper.Map<PedidoCompraProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = pedidoCompraProductoRepository.Update(pedidoCompraProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PedidoCompraProductoDTO dto)
		{
			try
			{
				PedidoCompraProducto pedidoCompraProducto = Mapper.Map<PedidoCompraProducto>(dto);

				using (TransactionScope tx = new())
				{
					pedidoCompraProductoRepository.Delete(pedidoCompraProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarProducto(PedidoCompraProductoDTO dto)
      {
			try
			{
				PedidoCompraProducto pedidoCompraProducto = Mapper.Map<PedidoCompraProducto>(dto);

				using (TransactionScope tx = new())
				{
					pedidoCompraProductoRepository.ActualizarProducto(pedidoCompraProducto);

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
