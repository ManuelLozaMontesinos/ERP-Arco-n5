using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoGuiaRemisionBusiness : IEstadoGuiaRemisionBusiness
	{
		private readonly IEstadoGuiaRemisionRepository estadoGuiaRemisionRepository;

		public EstadoGuiaRemisionBusiness(
			IEstadoGuiaRemisionRepository estadoGuiaRemisionRepository
			)
		{
			this.estadoGuiaRemisionRepository = estadoGuiaRemisionRepository;
		}
		public EstadoGuiaRemisionDTO GetById(int id)
		{
			try
			{
				EstadoGuiaRemision estadoGuiaRemision = estadoGuiaRemisionRepository.GetById(id);

				EstadoGuiaRemisionDTO estadoGuiaRemisionDTO = Mapper.Map<EstadoGuiaRemisionDTO>(estadoGuiaRemision);

				return estadoGuiaRemisionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoGuiaRemisionDTO> Search(EstadoGuiaRemisionDTO dto)
		{
			try
			{
				List<EstadoGuiaRemision> estadoGuiaRemisionList = estadoGuiaRemisionRepository.Search(Mapper.Map<EstadoGuiaRemision>(dto));

				List<EstadoGuiaRemisionDTO> estadoGuiaRemisionDTOList = Mapper.Map<List<EstadoGuiaRemisionDTO>>(estadoGuiaRemisionList);

				return estadoGuiaRemisionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoGuiaRemisionDTO SingleSearch(EstadoGuiaRemisionDTO dto)
		{
			try
			{
				EstadoGuiaRemision estadoGuiaRemision = estadoGuiaRemisionRepository.SingleSearch(Mapper.Map<EstadoGuiaRemision>(dto));

				EstadoGuiaRemisionDTO estadoGuiaRemisionDTO = Mapper.Map<EstadoGuiaRemisionDTO>(estadoGuiaRemision);

				return estadoGuiaRemisionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoGuiaRemisionDTO dto)
		{
			try
			{
				EstadoGuiaRemision estadoGuiaRemision = Mapper.Map<EstadoGuiaRemision>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoGuiaRemisionRepository.Insert(estadoGuiaRemision);

					dto.EstadoGuiaRemisionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoGuiaRemisionDTO dto)
		{
			try
			{
				EstadoGuiaRemision estadoGuiaRemision = Mapper.Map<EstadoGuiaRemision>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoGuiaRemisionRepository.Update(estadoGuiaRemision);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoGuiaRemisionDTO dto)
		{
			try
			{
				EstadoGuiaRemision estadoGuiaRemision = Mapper.Map<EstadoGuiaRemision>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoGuiaRemisionRepository.Delete(estadoGuiaRemision);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoGuiaRemisionDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoGuiaRemision estadoGuiaRemision = estadoGuiaRemisionRepository.ObtenerPorCodigo(codigo);

				EstadoGuiaRemisionDTO estadoGuiaRemisionDTO = Mapper.Map<EstadoGuiaRemisionDTO>(estadoGuiaRemision);

				return estadoGuiaRemisionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
