using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class OrdenProduccionSubProcesoBusiness : IOrdenProduccionSubProcesoBusiness
	{
		private readonly IOrdenProduccionSubProcesoRepository ordenProduccionSubProcesoRepository;
		private readonly IEstadoOrdenProduccionSubProcesoRepository estadoOrdenProduccionSubProcesoRepository;
		private readonly IEstadoOrdenProduccionRepository estadoOrdenProduccionRepository;
		private readonly IOrdenProduccionRepository ordenProduccionRepository;

		public OrdenProduccionSubProcesoBusiness(
			IOrdenProduccionSubProcesoRepository ordenProduccionSubProcesoRepository,
			IEstadoOrdenProduccionSubProcesoRepository estadoOrdenProduccionSubProcesoRepository,
			IEstadoOrdenProduccionRepository estadoOrdenProduccionRepository,
			IOrdenProduccionRepository ordenProduccionRepository
			)
		{
			this.ordenProduccionSubProcesoRepository = ordenProduccionSubProcesoRepository;
			this.estadoOrdenProduccionSubProcesoRepository = estadoOrdenProduccionSubProcesoRepository;
			this.estadoOrdenProduccionRepository = estadoOrdenProduccionRepository;
			this.ordenProduccionRepository = ordenProduccionRepository;
		}
		public OrdenProduccionSubProcesoDTO GetById(int id)
		{
			try
			{
				OrdenProduccionSubProceso ordenProduccionSubProceso = ordenProduccionSubProcesoRepository.GetById(id);

				OrdenProduccionSubProcesoDTO ordenProduccionSubProcesoDTO = Mapper.Map<OrdenProduccionSubProcesoDTO>(ordenProduccionSubProceso);

				return ordenProduccionSubProcesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OrdenProduccionSubProcesoDTO> Search(OrdenProduccionSubProcesoDTO dto)
		{
			try
			{
				List<OrdenProduccionSubProceso> ordenProduccionSubProcesoList = ordenProduccionSubProcesoRepository.Search(Mapper.Map<OrdenProduccionSubProceso>(dto));

				List<OrdenProduccionSubProcesoDTO> ordenProduccionSubProcesoDTOList = Mapper.Map<List<OrdenProduccionSubProcesoDTO>>(ordenProduccionSubProcesoList);

				return ordenProduccionSubProcesoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OrdenProduccionSubProcesoDTO SingleSearch(OrdenProduccionSubProcesoDTO dto)
		{
			try
			{
				OrdenProduccionSubProceso ordenProduccionSubProceso = ordenProduccionSubProcesoRepository.SingleSearch(Mapper.Map<OrdenProduccionSubProceso>(dto));

				OrdenProduccionSubProcesoDTO ordenProduccionSubProcesoDTO = Mapper.Map<OrdenProduccionSubProcesoDTO>(ordenProduccionSubProceso);

				return ordenProduccionSubProcesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OrdenProduccionSubProcesoDTO dto)
		{
			try
			{
				OrdenProduccionSubProceso ordenProduccionSubProceso = Mapper.Map<OrdenProduccionSubProceso>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = ordenProduccionSubProcesoRepository.Insert(ordenProduccionSubProceso);

					dto.OrdenProduccionSubProcesoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OrdenProduccionSubProcesoDTO dto)
		{
			try
			{
				OrdenProduccionSubProceso ordenProduccionSubProceso = Mapper.Map<OrdenProduccionSubProceso>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = ordenProduccionSubProcesoRepository.Update(ordenProduccionSubProceso);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(OrdenProduccionSubProcesoDTO dto)
		{
			try
			{
				OrdenProduccionSubProceso ordenProduccionSubProceso = Mapper.Map<OrdenProduccionSubProceso>(dto);

				using (TransactionScope tx = new())
				{
					ordenProduccionSubProcesoRepository.Delete(ordenProduccionSubProceso);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarInicio(OrdenProduccionSubProcesoDTO dto)
      {
			try
			{
				OrdenProduccionSubProceso ordenProduccionSubProceso = Mapper.Map<OrdenProduccionSubProceso>(dto);

				EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = estadoOrdenProduccionSubProcesoRepository
					.ObtenerPorCodigo(EstadoOrdenProduccionSubProcesoConstante.EN_PROCESO);

				using (TransactionScope tx = new())
				{

					ordenProduccionSubProceso.EstadoOrdenProduccionSubProcesoId = estadoOrdenProduccionSubProceso.EntityId;
					ordenProduccionSubProcesoRepository.ActualizarInicio(ordenProduccionSubProceso);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarFin(OrdenProduccionSubProcesoDTO dto)
      {
			try
			{
				OrdenProduccionSubProceso ordenProduccionSubProceso = ordenProduccionSubProcesoRepository.GetById(dto.Id);

				EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = estadoOrdenProduccionSubProcesoRepository
					.ObtenerPorCodigo(EstadoOrdenProduccionSubProcesoConstante.CULMINADA);

				using (TransactionScope tx = new())
				{

					ordenProduccionSubProceso.EstadoOrdenProduccionSubProcesoId = estadoOrdenProduccionSubProceso.EntityId;
					ordenProduccionSubProcesoRepository.ActualizarFin(ordenProduccionSubProceso);

					actualizarEstadoOrdenProduccion(ordenProduccionSubProceso.OrdenProduccionId, ordenProduccionSubProceso.UsuarioCreacionId);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		private void actualizarEstadoOrdenProduccion(int ordenProduccionId, int usuarioAccionId)
		{

			string codigo = string.Empty;

			List<OrdenProduccionSubProceso> ordenProduccionSubProcesoList = ordenProduccionSubProcesoRepository.Search(new OrdenProduccionSubProceso()
			{
				OrdenProduccionId = ordenProduccionId,
				Estado = EstadoConstante.ACTIVO
			});

			int total = ordenProduccionSubProcesoList.Count;
			int totalFinalizado = ordenProduccionSubProcesoList.Count(x => x.EstadoOrdenProduccionSubProceso.Codigo == EstadoOrdenProduccionSubProcesoConstante.CULMINADA);

			if (totalFinalizado == decimal.Zero)
				codigo = EstadoOrdenProduccionConstante.EN_PROCESO;
			else
			{
				if (totalFinalizado == total)
					codigo = EstadoOrdenProduccionConstante.CULMINADA;
				else
					codigo = EstadoOrdenProduccionConstante.EN_PROCESO;
			}

			EstadoOrdenProduccion estadoOrdenProduccion = estadoOrdenProduccionRepository.ObtenerPorCodigo(codigo);

			ordenProduccionRepository.ActualizarEstado(new OrdenProduccion()
			{
				CotizacionId = ordenProduccionId,
				EstadoOrdenProduccionId = estadoOrdenProduccion.EntityId,
				UsuarioModificacionId = usuarioAccionId
			});
		}
        public void Maquina(OrdenProduccionSubProcesoDTO dto)
        {
            try
            {
                OrdenProduccionSubProceso ordenProduccionSubProceso = Mapper.Map<OrdenProduccionSubProceso>(dto);

                using (TransactionScope tx = new())
                {
                    ordenProduccionSubProcesoRepository.Maquina(ordenProduccionSubProceso);

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
