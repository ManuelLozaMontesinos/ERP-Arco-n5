using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoOrdenCompraBusiness : IEstadoOrdenCompraBusiness
	{
		private readonly IEstadoOrdenCompraRepository estadoOrdenCompraRepository;

		public EstadoOrdenCompraBusiness(
			IEstadoOrdenCompraRepository estadoOrdenCompraRepository
			)
		{
			this.estadoOrdenCompraRepository = estadoOrdenCompraRepository;
		}
		public EstadoOrdenCompraDTO GetById(int id)
		{
			try
			{
				EstadoOrdenCompra estadoOrdenCompra = estadoOrdenCompraRepository.GetById(id);

				EstadoOrdenCompraDTO estadoOrdenCompraDTO = Mapper.Map<EstadoOrdenCompraDTO>(estadoOrdenCompra);

				return estadoOrdenCompraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoOrdenCompraDTO> Search(EstadoOrdenCompraDTO dto)
		{
			try
			{
				List<EstadoOrdenCompra> estadoOrdenCompraList = estadoOrdenCompraRepository.Search(Mapper.Map<EstadoOrdenCompra>(dto));

				List<EstadoOrdenCompraDTO> estadoOrdenCompraDTOList = Mapper.Map<List<EstadoOrdenCompraDTO>>(estadoOrdenCompraList);

				return estadoOrdenCompraDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoOrdenCompraDTO SingleSearch(EstadoOrdenCompraDTO dto)
		{
			try
			{
				EstadoOrdenCompra estadoOrdenCompra = estadoOrdenCompraRepository.SingleSearch(Mapper.Map<EstadoOrdenCompra>(dto));

				EstadoOrdenCompraDTO estadoOrdenCompraDTO = Mapper.Map<EstadoOrdenCompraDTO>(estadoOrdenCompra);

				return estadoOrdenCompraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoOrdenCompraDTO dto)
		{
			try
			{
				EstadoOrdenCompra estadoOrdenCompra = Mapper.Map<EstadoOrdenCompra>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoOrdenCompraRepository.Insert(estadoOrdenCompra);

					dto.EstadoOrdenCompraId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoOrdenCompraDTO dto)
		{
			try
			{
				EstadoOrdenCompra estadoOrdenCompra = Mapper.Map<EstadoOrdenCompra>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoOrdenCompraRepository.Update(estadoOrdenCompra);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoOrdenCompraDTO dto)
		{
			try
			{
				EstadoOrdenCompra estadoOrdenCompra = Mapper.Map<EstadoOrdenCompra>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoOrdenCompraRepository.Delete(estadoOrdenCompra);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoOrdenCompraDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoOrdenCompra estadoOrdenCompra = estadoOrdenCompraRepository.ObtenerPorCodigo(codigo);

				EstadoOrdenCompraDTO estadoOrdenCompraDTO = Mapper.Map<EstadoOrdenCompraDTO>(estadoOrdenCompra);

				return estadoOrdenCompraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
