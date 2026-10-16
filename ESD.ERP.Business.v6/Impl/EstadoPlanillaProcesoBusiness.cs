using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoPlanillaProcesoBusiness : IEstadoPlanillaProcesoBusiness
	{
		private readonly IEstadoPlanillaProcesoRepository estadoPlanillaProcesoRepository;

		public EstadoPlanillaProcesoBusiness(
			IEstadoPlanillaProcesoRepository estadoPlanillaProcesoRepository
			)
		{
			this.estadoPlanillaProcesoRepository = estadoPlanillaProcesoRepository;
		}
		public EstadoPlanillaProcesoDTO GetById(int id)
		{
			try
			{
				EstadoPlanillaProceso estadoPlanillaProceso = estadoPlanillaProcesoRepository.GetById(id);

				EstadoPlanillaProcesoDTO estadoPlanillaProcesoDTO = Mapper.Map<EstadoPlanillaProcesoDTO>(estadoPlanillaProceso);

				return estadoPlanillaProcesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoPlanillaProcesoDTO> Search(EstadoPlanillaProcesoDTO dto)
		{
			try
			{
				List<EstadoPlanillaProceso> estadoPlanillaProcesoList = estadoPlanillaProcesoRepository.Search(Mapper.Map<EstadoPlanillaProceso>(dto));

				List<EstadoPlanillaProcesoDTO> estadoPlanillaProcesoDTOList = Mapper.Map<List<EstadoPlanillaProcesoDTO>>(estadoPlanillaProcesoList);

				return estadoPlanillaProcesoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoPlanillaProcesoDTO SingleSearch(EstadoPlanillaProcesoDTO dto)
		{
			try
			{
				EstadoPlanillaProceso estadoPlanillaProceso = estadoPlanillaProcesoRepository.SingleSearch(Mapper.Map<EstadoPlanillaProceso>(dto));

				EstadoPlanillaProcesoDTO estadoPlanillaProcesoDTO = Mapper.Map<EstadoPlanillaProcesoDTO>(estadoPlanillaProceso);

				return estadoPlanillaProcesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoPlanillaProcesoDTO dto)
		{
			try
			{
				EstadoPlanillaProceso estadoPlanillaProceso = Mapper.Map<EstadoPlanillaProceso>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoPlanillaProcesoRepository.Insert(estadoPlanillaProceso);

					dto.EstadoPlanillaProcesoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoPlanillaProcesoDTO dto)
		{
			try
			{
				EstadoPlanillaProceso estadoPlanillaProceso = Mapper.Map<EstadoPlanillaProceso>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoPlanillaProcesoRepository.Update(estadoPlanillaProceso);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoPlanillaProcesoDTO dto)
		{
			try
			{
				EstadoPlanillaProceso estadoPlanillaProceso = Mapper.Map<EstadoPlanillaProceso>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoPlanillaProcesoRepository.Delete(estadoPlanillaProceso);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoPlanillaProcesoDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoPlanillaProceso estadoPlanillaProceso = estadoPlanillaProcesoRepository.ObtenerPorCodigo(codigo);

				EstadoPlanillaProcesoDTO estadoPlanillaProcesoDTO = Mapper.Map<EstadoPlanillaProcesoDTO>(estadoPlanillaProceso);

				return estadoPlanillaProcesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
