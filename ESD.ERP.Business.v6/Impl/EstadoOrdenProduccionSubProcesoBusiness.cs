using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoOrdenProduccionSubProcesoBusiness : IEstadoOrdenProduccionSubProcesoBusiness
	{
		private readonly IEstadoOrdenProduccionSubProcesoRepository estadoOrdenProduccionSubProcesoRepository;

		public EstadoOrdenProduccionSubProcesoBusiness(
			IEstadoOrdenProduccionSubProcesoRepository estadoOrdenProduccionSubProcesoRepository
			)
		{
			this.estadoOrdenProduccionSubProcesoRepository = estadoOrdenProduccionSubProcesoRepository;
		}
		public EstadoOrdenProduccionSubProcesoDTO GetById(int id)
		{
			try
			{
				EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = estadoOrdenProduccionSubProcesoRepository.GetById(id);

				EstadoOrdenProduccionSubProcesoDTO estadoOrdenProduccionSubProcesoDTO = Mapper.Map<EstadoOrdenProduccionSubProcesoDTO>(estadoOrdenProduccionSubProceso);

				return estadoOrdenProduccionSubProcesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoOrdenProduccionSubProcesoDTO> Search(EstadoOrdenProduccionSubProcesoDTO dto)
		{
			try
			{
				List<EstadoOrdenProduccionSubProceso> estadoOrdenProduccionSubProcesoList = estadoOrdenProduccionSubProcesoRepository.Search(Mapper.Map<EstadoOrdenProduccionSubProceso>(dto));

				List<EstadoOrdenProduccionSubProcesoDTO> estadoOrdenProduccionSubProcesoDTOList = Mapper.Map<List<EstadoOrdenProduccionSubProcesoDTO>>(estadoOrdenProduccionSubProcesoList);

				return estadoOrdenProduccionSubProcesoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoOrdenProduccionSubProcesoDTO SingleSearch(EstadoOrdenProduccionSubProcesoDTO dto)
		{
			try
			{
				EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = estadoOrdenProduccionSubProcesoRepository.SingleSearch(Mapper.Map<EstadoOrdenProduccionSubProceso>(dto));

				EstadoOrdenProduccionSubProcesoDTO estadoOrdenProduccionSubProcesoDTO = Mapper.Map<EstadoOrdenProduccionSubProcesoDTO>(estadoOrdenProduccionSubProceso);

				return estadoOrdenProduccionSubProcesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoOrdenProduccionSubProcesoDTO dto)
		{
			try
			{
				EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = Mapper.Map<EstadoOrdenProduccionSubProceso>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = estadoOrdenProduccionSubProcesoRepository.Insert(estadoOrdenProduccionSubProceso);

					dto.EstadoOrdenProduccionSubProcesoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoOrdenProduccionSubProcesoDTO dto)
		{
			try
			{
				EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = Mapper.Map<EstadoOrdenProduccionSubProceso>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = estadoOrdenProduccionSubProcesoRepository.Update(estadoOrdenProduccionSubProceso);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoOrdenProduccionSubProcesoDTO dto)
		{
			try
			{
				EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = Mapper.Map<EstadoOrdenProduccionSubProceso>(dto);

				using (TransactionScope tx = new())
				{
					estadoOrdenProduccionSubProcesoRepository.Delete(estadoOrdenProduccionSubProceso);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoOrdenProduccionSubProcesoDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = estadoOrdenProduccionSubProcesoRepository.ObtenerPorCodigo(codigo);

				EstadoOrdenProduccionSubProcesoDTO estadoOrdenProduccionSubProcesoDTO = Mapper.Map<EstadoOrdenProduccionSubProcesoDTO>(estadoOrdenProduccionSubProceso);

				return estadoOrdenProduccionSubProcesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
