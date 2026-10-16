using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoComercialProductoBusiness : IEstadoComercialProductoBusiness
	{
		private readonly IEstadoComercialProductoRepository estadoComercialProductoRepository;

		public EstadoComercialProductoBusiness(
			IEstadoComercialProductoRepository estadoComercialProductoRepository
			)
		{
			this.estadoComercialProductoRepository = estadoComercialProductoRepository;
		}
		public EstadoComercialProductoDTO GetById(int id)
		{
			try
			{
				EstadoComercialProducto estadoComercialProducto = estadoComercialProductoRepository.GetById(id);

				EstadoComercialProductoDTO estadoComercialProductoDTO = Mapper.Map<EstadoComercialProductoDTO>(estadoComercialProducto);

				return estadoComercialProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoComercialProductoDTO> Search(EstadoComercialProductoDTO dto)
		{
			try
			{
				List<EstadoComercialProducto> estadoComercialProductoList = estadoComercialProductoRepository.Search(Mapper.Map<EstadoComercialProducto>(dto));

				List<EstadoComercialProductoDTO> estadoComercialProductoDTOList = Mapper.Map<List<EstadoComercialProductoDTO>>(estadoComercialProductoList);

				return estadoComercialProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoComercialProductoDTO SingleSearch(EstadoComercialProductoDTO dto)
		{
			try
			{
				EstadoComercialProducto estadoComercialProducto = estadoComercialProductoRepository.SingleSearch(Mapper.Map<EstadoComercialProducto>(dto));

				EstadoComercialProductoDTO estadoComercialProductoDTO = Mapper.Map<EstadoComercialProductoDTO>(estadoComercialProducto);

				return estadoComercialProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoComercialProductoDTO dto)
		{
			try
			{
				EstadoComercialProducto estadoComercialProducto = Mapper.Map<EstadoComercialProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoComercialProductoRepository.Insert(estadoComercialProducto);

					dto.EstadoComercialProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoComercialProductoDTO dto)
		{
			try
			{
				EstadoComercialProducto estadoComercialProducto = Mapper.Map<EstadoComercialProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoComercialProductoRepository.Update(estadoComercialProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoComercialProductoDTO dto)
		{
			try
			{
				EstadoComercialProducto estadoComercialProducto = Mapper.Map<EstadoComercialProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoComercialProductoRepository.Delete(estadoComercialProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public EstadoComercialProductoDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				EstadoComercialProducto estadoComercialProducto = estadoComercialProductoRepository.ObtenerPorCodigo(codigo);

				EstadoComercialProductoDTO estadoComercialProductoDTO = Mapper.Map<EstadoComercialProductoDTO>(estadoComercialProducto);

				return estadoComercialProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
