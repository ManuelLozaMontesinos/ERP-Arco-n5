using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoOrdenProduccionBusiness : IEstadoOrdenProduccionBusiness
	{
		private readonly IEstadoOrdenProduccionRepository estadoOrdenProduccionRepository;

		public EstadoOrdenProduccionBusiness(
			IEstadoOrdenProduccionRepository estadoOrdenProduccionRepository
			)
		{
			this.estadoOrdenProduccionRepository = estadoOrdenProduccionRepository;
		}
		public EstadoOrdenProduccionDTO GetById(int id)
		{
			try
			{
				EstadoOrdenProduccion estadoOrdenProduccion = estadoOrdenProduccionRepository.GetById(id);

				EstadoOrdenProduccionDTO estadoOrdenProduccionDTO = Mapper.Map<EstadoOrdenProduccionDTO>(estadoOrdenProduccion);

				return estadoOrdenProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoOrdenProduccionDTO> Search(EstadoOrdenProduccionDTO dto)
		{
			try
			{
				List<EstadoOrdenProduccion> estadoOrdenProduccionList = estadoOrdenProduccionRepository.Search(Mapper.Map<EstadoOrdenProduccion>(dto));

				List<EstadoOrdenProduccionDTO> estadoOrdenProduccionDTOList = Mapper.Map<List<EstadoOrdenProduccionDTO>>(estadoOrdenProduccionList);

				return estadoOrdenProduccionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoOrdenProduccionDTO SingleSearch(EstadoOrdenProduccionDTO dto)
		{
			try
			{
				EstadoOrdenProduccion estadoOrdenProduccion = estadoOrdenProduccionRepository.SingleSearch(Mapper.Map<EstadoOrdenProduccion>(dto));

				EstadoOrdenProduccionDTO estadoOrdenProduccionDTO = Mapper.Map<EstadoOrdenProduccionDTO>(estadoOrdenProduccion);

				return estadoOrdenProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoOrdenProduccionDTO dto)
		{
			try
			{
				EstadoOrdenProduccion estadoOrdenProduccion = Mapper.Map<EstadoOrdenProduccion>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = estadoOrdenProduccionRepository.Insert(estadoOrdenProduccion);

					dto.EstadoOrdenProduccionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoOrdenProduccionDTO dto)
		{
			try
			{
				EstadoOrdenProduccion estadoOrdenProduccion = Mapper.Map<EstadoOrdenProduccion>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = estadoOrdenProduccionRepository.Update(estadoOrdenProduccion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoOrdenProduccionDTO dto)
		{
			try
			{
				EstadoOrdenProduccion estadoOrdenProduccion = Mapper.Map<EstadoOrdenProduccion>(dto);

				using (TransactionScope tx = new())
				{
					estadoOrdenProduccionRepository.Delete(estadoOrdenProduccion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoOrdenProduccionDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoOrdenProduccion estadoOrdenProduccion = estadoOrdenProduccionRepository.ObtenerPorCodigo(codigo);

				EstadoOrdenProduccionDTO estadoOrdenProduccionDTO = Mapper.Map<EstadoOrdenProduccionDTO>(estadoOrdenProduccion);

				return estadoOrdenProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
