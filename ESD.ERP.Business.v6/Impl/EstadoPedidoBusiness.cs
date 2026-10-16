using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoPedidoBusiness : IEstadoPedidoBusiness
	{
		private readonly IEstadoPedidoRepository estadoPedidoRepository;

		public EstadoPedidoBusiness(
			IEstadoPedidoRepository estadoPedidoRepository
			)
		{
			this.estadoPedidoRepository = estadoPedidoRepository;
		}
		public EstadoPedidoDTO GetById(int id)
		{
			try
			{
				EstadoPedido estadoPedido = estadoPedidoRepository.GetById(id);

				EstadoPedidoDTO estadoPedidoDTO = Mapper.Map<EstadoPedidoDTO>(estadoPedido);

				return estadoPedidoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoPedidoDTO> Search(EstadoPedidoDTO dto)
		{
			try
			{
				List<EstadoPedido> estadoPedidoList = estadoPedidoRepository.Search(Mapper.Map<EstadoPedido>(dto));

				List<EstadoPedidoDTO> estadoPedidoDTOList = Mapper.Map<List<EstadoPedidoDTO>>(estadoPedidoList);

				return estadoPedidoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoPedidoDTO SingleSearch(EstadoPedidoDTO dto)
		{
			try
			{
				EstadoPedido estadoPedido = estadoPedidoRepository.SingleSearch(Mapper.Map<EstadoPedido>(dto));

				EstadoPedidoDTO estadoPedidoDTO = Mapper.Map<EstadoPedidoDTO>(estadoPedido);

				return estadoPedidoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoPedidoDTO dto)
		{
			try
			{
				EstadoPedido estadoPedido = Mapper.Map<EstadoPedido>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoPedidoRepository.Insert(estadoPedido);

					dto.EstadoPedidoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoPedidoDTO dto)
		{
			try
			{
				EstadoPedido estadoPedido = Mapper.Map<EstadoPedido>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoPedidoRepository.Update(estadoPedido);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoPedidoDTO dto)
		{
			try
			{
				EstadoPedido estadoPedido = Mapper.Map<EstadoPedido>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoPedidoRepository.Delete(estadoPedido);

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
