using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PedidoProductoBusiness : IPedidoProductoBusiness
	{
		private readonly IPedidoProductoRepository pedidoProductoRepository;

		public PedidoProductoBusiness(
			IPedidoProductoRepository pedidoProductoRepository
			)
		{
			this.pedidoProductoRepository = pedidoProductoRepository;
		}
		public PedidoProductoDTO GetById(int id)
		{
			try
			{
				PedidoProducto pedidoProducto = pedidoProductoRepository.GetById(id);

				PedidoProductoDTO pedidoProductoDTO = Mapper.Map<PedidoProductoDTO>(pedidoProducto);

				return pedidoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PedidoProductoDTO> Search(PedidoProductoDTO dto)
		{
			try
			{
				List<PedidoProducto> pedidoProductoList = pedidoProductoRepository.Search(Mapper.Map<PedidoProducto>(dto));

				List<PedidoProductoDTO> pedidoProductoDTOList = Mapper.Map<List<PedidoProductoDTO>>(pedidoProductoList);

				return pedidoProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PedidoProductoDTO SingleSearch(PedidoProductoDTO dto)
		{
			try
			{
				PedidoProducto pedidoProducto = pedidoProductoRepository.SingleSearch(Mapper.Map<PedidoProducto>(dto));

				PedidoProductoDTO pedidoProductoDTO = Mapper.Map<PedidoProductoDTO>(pedidoProducto);

				return pedidoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PedidoProductoDTO dto)
		{
			try
			{
				PedidoProducto pedidoProducto = Mapper.Map<PedidoProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = pedidoProductoRepository.Insert(pedidoProducto);

					dto.PedidoProdId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PedidoProductoDTO dto)
		{
			try
			{
				PedidoProducto pedidoProducto = Mapper.Map<PedidoProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = pedidoProductoRepository.Update(pedidoProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PedidoProductoDTO dto)
		{
			try
			{
				PedidoProducto pedidoProducto = Mapper.Map<PedidoProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					pedidoProductoRepository.Delete(pedidoProducto);

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
