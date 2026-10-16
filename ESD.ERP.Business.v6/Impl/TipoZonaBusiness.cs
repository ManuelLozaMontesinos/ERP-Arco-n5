using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoZonaBusiness : ITipoZonaBusiness
	{
		private readonly ITipoZonaRepository tipoZonaRepository;

		public TipoZonaBusiness(
			ITipoZonaRepository tipoZonaRepository
			)
		{
			this.tipoZonaRepository = tipoZonaRepository;
		}
		public TipoZonaDTO GetById(int id)
		{
			try
			{
				TipoZona tipoZona = tipoZonaRepository.GetById(id);

				TipoZonaDTO tipoZonaDTO = Mapper.Map<TipoZonaDTO>(tipoZona);

				return tipoZonaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoZonaDTO> Search(TipoZonaDTO dto)
		{
			try
			{
				List<TipoZona> tipoZonaList = tipoZonaRepository.Search(Mapper.Map<TipoZona>(dto));

				List<TipoZonaDTO> tipoZonaDTOList = Mapper.Map<List<TipoZonaDTO>>(tipoZonaList);

				return tipoZonaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoZonaDTO SingleSearch(TipoZonaDTO dto)
		{
			try
			{
				TipoZona tipoZona = tipoZonaRepository.SingleSearch(Mapper.Map<TipoZona>(dto));

				TipoZonaDTO tipoZonaDTO = Mapper.Map<TipoZonaDTO>(tipoZona);

				return tipoZonaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoZonaDTO dto)
		{
			try
			{
				TipoZona tipoZona = Mapper.Map<TipoZona>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoZonaRepository.Insert(tipoZona);

					dto.TipoZonaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoZonaDTO dto)
		{
			try
			{
				TipoZona tipoZona = Mapper.Map<TipoZona>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoZonaRepository.Update(tipoZona);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoZonaDTO dto)
		{
			try
			{
				TipoZona tipoZona = Mapper.Map<TipoZona>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoZonaRepository.Delete(tipoZona);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoZonaDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoZona tipoZona = tipoZonaRepository.ObtenerPorCodigo(codigo);

				TipoZonaDTO tipoZonaDTO = Mapper.Map<TipoZonaDTO>(tipoZona);

				return tipoZonaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
