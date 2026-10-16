using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoTrabajadorBusiness : ITipoTrabajadorBusiness
	{
		private readonly ITipoTrabajadorRepository tipoTrabajadorRepository;

		public TipoTrabajadorBusiness(
			ITipoTrabajadorRepository tipoTrabajadorRepository
			)
		{
			this.tipoTrabajadorRepository = tipoTrabajadorRepository;
		}
		public TipoTrabajadorDTO GetById(int id)
		{
			try
			{
				TipoTrabajador tipoTrabajador = tipoTrabajadorRepository.GetById(id);

				TipoTrabajadorDTO tipoTrabajadorDTO = Mapper.Map<TipoTrabajadorDTO>(tipoTrabajador);

				return tipoTrabajadorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoTrabajadorDTO> Search(TipoTrabajadorDTO dto)
		{
			try
			{
				List<TipoTrabajador> tipoTrabajadorList = tipoTrabajadorRepository.Search(Mapper.Map<TipoTrabajador>(dto));

				List<TipoTrabajadorDTO> tipoTrabajadorDTOList = Mapper.Map<List<TipoTrabajadorDTO>>(tipoTrabajadorList);

				return tipoTrabajadorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoTrabajadorDTO SingleSearch(TipoTrabajadorDTO dto)
		{
			try
			{
				TipoTrabajador tipoTrabajador = tipoTrabajadorRepository.SingleSearch(Mapper.Map<TipoTrabajador>(dto));

				TipoTrabajadorDTO tipoTrabajadorDTO = Mapper.Map<TipoTrabajadorDTO>(tipoTrabajador);

				return tipoTrabajadorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoTrabajadorDTO dto)
		{
			try
			{
				TipoTrabajador tipoTrabajador = Mapper.Map<TipoTrabajador>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoTrabajadorRepository.Insert(tipoTrabajador);

					dto.TipoTrabajadorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoTrabajadorDTO dto)
		{
			try
			{
				TipoTrabajador tipoTrabajador = Mapper.Map<TipoTrabajador>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoTrabajadorRepository.Update(tipoTrabajador);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoTrabajadorDTO dto)
		{
			try
			{
				TipoTrabajador tipoTrabajador = Mapper.Map<TipoTrabajador>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoTrabajadorRepository.Delete(tipoTrabajador);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoTrabajadorDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoTrabajador tipoTrabajador = tipoTrabajadorRepository.ObtenerPorCodigo(codigo);

				TipoTrabajadorDTO tipoTrabajadorDTO = Mapper.Map<TipoTrabajadorDTO>(tipoTrabajador);

				return tipoTrabajadorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
