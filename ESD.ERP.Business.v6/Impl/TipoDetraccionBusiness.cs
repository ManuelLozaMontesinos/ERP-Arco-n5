using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoDetraccionBusiness : ITipoDetraccionBusiness
	{
		private readonly ITipoDetraccionRepository tipoDetraccionRepository;

		public TipoDetraccionBusiness(
			ITipoDetraccionRepository tipoDetraccionRepository
			)
		{
			this.tipoDetraccionRepository = tipoDetraccionRepository;
		}
		public TipoDetraccionDTO GetById(int id)
		{
			try
			{
				TipoDetraccion tipoDetrac = tipoDetraccionRepository.GetById(id);

				TipoDetraccionDTO tipoDetracDTO = Mapper.Map<TipoDetraccionDTO>(tipoDetrac);

				return tipoDetracDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoDetraccionDTO> Search(TipoDetraccionDTO dto)
		{
			try
			{
				List<TipoDetraccion> tipoDetraccionList = tipoDetraccionRepository.Search(Mapper.Map<TipoDetraccion>(dto));

				List<TipoDetraccionDTO> tipoDetracDTOList = Mapper.Map<List<TipoDetraccionDTO>>(tipoDetraccionList);

				return tipoDetracDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoDetraccionDTO SingleSearch(TipoDetraccionDTO dto)
		{
			try
			{
				TipoDetraccion tipoDetrac = tipoDetraccionRepository.SingleSearch(Mapper.Map<TipoDetraccion>(dto));

				TipoDetraccionDTO tipoDetracDTO = Mapper.Map<TipoDetraccionDTO>(tipoDetrac);

				return tipoDetracDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoDetraccionDTO dto)
		{
			try
			{
				TipoDetraccion tipoDetrac = Mapper.Map<TipoDetraccion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoDetraccionRepository.Insert(tipoDetrac);

					dto.TipoDetraccionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoDetraccionDTO dto)
		{
			try
			{
				TipoDetraccion tipoDetrac = Mapper.Map<TipoDetraccion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoDetraccionRepository.Update(tipoDetrac);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoDetraccionDTO dto)
		{
			try
			{
				TipoDetraccion tipoDetrac = Mapper.Map<TipoDetraccion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoDetraccionRepository.Delete(tipoDetrac);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoDetraccionDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoDetraccion tipoDetraccion = tipoDetraccionRepository.ObtenerPorCodigo(codigo);

				TipoDetraccionDTO tipoDetraccionDTO = Mapper.Map<TipoDetraccionDTO>(tipoDetraccion);

				return tipoDetraccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
