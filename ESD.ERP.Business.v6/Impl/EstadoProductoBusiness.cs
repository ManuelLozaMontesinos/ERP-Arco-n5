using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoProductoBusiness : IEstadoProductoBusiness
	{
		private readonly IEstadoProductoRepository estadoProductoRepository;

		public EstadoProductoBusiness(
			IEstadoProductoRepository estadoProductoRepository
			)
		{
			this.estadoProductoRepository = estadoProductoRepository;
		}
		public EstadoProductoDTO GetById(int id)
		{
			try
			{
				EstadoProducto estadoProducto = estadoProductoRepository.GetById(id);

				EstadoProductoDTO estadoProductoDTO = Mapper.Map<EstadoProductoDTO>(estadoProducto);

				return estadoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoProductoDTO> Search(EstadoProductoDTO dto)
		{
			try
			{
				List<EstadoProducto> estadoProductoList = estadoProductoRepository.Search(Mapper.Map<EstadoProducto>(dto));

				List<EstadoProductoDTO> estadoProductoDTOList = Mapper.Map<List<EstadoProductoDTO>>(estadoProductoList);

				return estadoProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoProductoDTO SingleSearch(EstadoProductoDTO dto)
		{
			try
			{
				EstadoProducto estadoProducto = estadoProductoRepository.SingleSearch(Mapper.Map<EstadoProducto>(dto));

				EstadoProductoDTO estadoProductoDTO = Mapper.Map<EstadoProductoDTO>(estadoProducto);

				return estadoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoProductoDTO dto)
		{
			try
			{
				EstadoProducto estadoProducto = Mapper.Map<EstadoProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoProductoRepository.Insert(estadoProducto);

					dto.EstadoProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoProductoDTO dto)
		{
			try
			{
				EstadoProducto estadoProducto = Mapper.Map<EstadoProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoProductoRepository.Update(estadoProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoProductoDTO dto)
		{
			try
			{
				EstadoProducto estadoProducto = Mapper.Map<EstadoProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoProductoRepository.Delete(estadoProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public EstadoProductoDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				EstadoProducto estadoProducto = estadoProductoRepository.ObtenerPorCodigo(codigo);

				EstadoProductoDTO estadoProductoDTO = Mapper.Map<EstadoProductoDTO>(estadoProducto);

				return estadoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
