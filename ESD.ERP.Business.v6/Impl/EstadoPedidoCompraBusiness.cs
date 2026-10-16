using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoPedidoCompraBusiness : IEstadoPedidoCompraBusiness
	{
		private readonly IEstadoPedidoCompraRepository estadoPedidoCompraRepository;

		public EstadoPedidoCompraBusiness(
			IEstadoPedidoCompraRepository estadoPedidoCompraRepository
			)
		{
			this.estadoPedidoCompraRepository = estadoPedidoCompraRepository;
		}
		public EstadoPedidoCompraDTO GetById(int id)
		{
			try
			{
				EstadoPedidoCompra estadoPedidoCompra = estadoPedidoCompraRepository.GetById(id);

				EstadoPedidoCompraDTO estadoPedidoCompraDTO = Mapper.Map<EstadoPedidoCompraDTO>(estadoPedidoCompra);

				return estadoPedidoCompraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoPedidoCompraDTO> Search(EstadoPedidoCompraDTO dto)
		{
			try
			{
				List<EstadoPedidoCompra> estadoPedidoCompraList = estadoPedidoCompraRepository.Search(Mapper.Map<EstadoPedidoCompra>(dto));

				List<EstadoPedidoCompraDTO> estadoPedidoCompraDTOList = Mapper.Map<List<EstadoPedidoCompraDTO>>(estadoPedidoCompraList);

				return estadoPedidoCompraDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoPedidoCompraDTO SingleSearch(EstadoPedidoCompraDTO dto)
		{
			try
			{
				EstadoPedidoCompra estadoPedidoCompra = estadoPedidoCompraRepository.SingleSearch(Mapper.Map<EstadoPedidoCompra>(dto));

				EstadoPedidoCompraDTO estadoPedidoCompraDTO = Mapper.Map<EstadoPedidoCompraDTO>(estadoPedidoCompra);

				return estadoPedidoCompraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoPedidoCompraDTO dto)
		{
			try
			{
				EstadoPedidoCompra estadoPedidoCompra = Mapper.Map<EstadoPedidoCompra>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoPedidoCompraRepository.Insert(estadoPedidoCompra);

					dto.EstadoPedidoCompraId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoPedidoCompraDTO dto)
		{
			try
			{
				EstadoPedidoCompra estadoPedidoCompra = Mapper.Map<EstadoPedidoCompra>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoPedidoCompraRepository.Update(estadoPedidoCompra);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoPedidoCompraDTO dto)
		{
			try
			{
				EstadoPedidoCompra estadoPedidoCompra = Mapper.Map<EstadoPedidoCompra>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoPedidoCompraRepository.Delete(estadoPedidoCompra);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public EstadoPedidoCompraDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				EstadoPedidoCompra estadoPedidoCompra = estadoPedidoCompraRepository.ObtenerPorCodigo(codigo);

				EstadoPedidoCompraDTO estadoPedidoCompraDTO = Mapper.Map<EstadoPedidoCompraDTO>(estadoPedidoCompra);

				return estadoPedidoCompraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
