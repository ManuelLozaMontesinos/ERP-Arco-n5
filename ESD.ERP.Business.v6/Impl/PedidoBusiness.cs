using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PedidoBusiness : IPedidoBusiness
	{
		private readonly IPedidoRepository pedidoRepository;

		public PedidoBusiness(
			IPedidoRepository pedidoRepository
			)
		{
			this.pedidoRepository = pedidoRepository;
		}
		public PedidoDTO GetById(int id)
		{
			try
			{
				Pedido pedido = pedidoRepository.GetById(id);

				PedidoDTO pedidoDTO = Mapper.Map<PedidoDTO>(pedido);

				return pedidoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PedidoDTO> Search(PedidoDTO dto)
		{
			try
			{
				List<Pedido> pedidoList = pedidoRepository.Search(Mapper.Map<Pedido>(dto));

				List<PedidoDTO> pedidoDTOList = Mapper.Map<List<PedidoDTO>>(pedidoList);

				return pedidoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PedidoDTO SingleSearch(PedidoDTO dto)
		{
			try
			{
				Pedido pedido = pedidoRepository.SingleSearch(Mapper.Map<Pedido>(dto));

				PedidoDTO pedidoDTO = Mapper.Map<PedidoDTO>(pedido);

				return pedidoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PedidoDTO dto)
		{
			try
			{
				Pedido pedido = Mapper.Map<Pedido>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = pedidoRepository.Insert(pedido);

					dto.PedidoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PedidoDTO dto)
		{
			try
			{
				Pedido pedido = Mapper.Map<Pedido>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = pedidoRepository.Update(pedido);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PedidoDTO dto)
		{
			try
			{
				Pedido pedido = Mapper.Map<Pedido>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					pedidoRepository.Delete(pedido);

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
