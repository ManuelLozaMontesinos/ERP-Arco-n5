using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoDespachoBusiness : IEstadoDespachoBusiness
	{
		private readonly IEstadoDespachoRepository estadoDespachoRepository;

		public EstadoDespachoBusiness(
			IEstadoDespachoRepository estadoDespachoRepository
			)
		{
			this.estadoDespachoRepository = estadoDespachoRepository;
		}
		public EstadoDespachoDTO GetById(int id)
		{
			try
			{
				EstadoDespacho estadoDespacho = estadoDespachoRepository.GetById(id);

				EstadoDespachoDTO estadoDespachoDTO = Mapper.Map<EstadoDespachoDTO>(estadoDespacho);

				return estadoDespachoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoDespachoDTO> Search(EstadoDespachoDTO dto)
		{
			try
			{
				List<EstadoDespacho> estadoDespachoList = estadoDespachoRepository.Search(Mapper.Map<EstadoDespacho>(dto));

				List<EstadoDespachoDTO> estadoDespachoDTOList = Mapper.Map<List<EstadoDespachoDTO>>(estadoDespachoList);

				return estadoDespachoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoDespachoDTO SingleSearch(EstadoDespachoDTO dto)
		{
			try
			{
				EstadoDespacho estadoDespacho = estadoDespachoRepository.SingleSearch(Mapper.Map<EstadoDespacho>(dto));

				EstadoDespachoDTO estadoDespachoDTO = Mapper.Map<EstadoDespachoDTO>(estadoDespacho);

				return estadoDespachoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoDespachoDTO dto)
		{
			try
			{
				EstadoDespacho estadoDespacho = Mapper.Map<EstadoDespacho>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = estadoDespachoRepository.Insert(estadoDespacho);

					dto.EstadoDespachoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoDespachoDTO dto)
		{
			try
			{
				EstadoDespacho estadoDespacho = Mapper.Map<EstadoDespacho>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = estadoDespachoRepository.Update(estadoDespacho);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoDespachoDTO dto)
		{
			try
			{
				EstadoDespacho estadoDespacho = Mapper.Map<EstadoDespacho>(dto);

				using (TransactionScope tx = new())
				{
					estadoDespachoRepository.Delete(estadoDespacho);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoDespachoDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoDespacho estadoDespacho = estadoDespachoRepository.ObtenerPorCodigo(codigo);

				EstadoDespachoDTO estadoDespachoDTO = Mapper.Map<EstadoDespachoDTO>(estadoDespacho);

				return estadoDespachoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
