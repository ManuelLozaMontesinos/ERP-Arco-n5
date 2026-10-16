using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoNaturalezaJuridicaBusiness : ITipoNaturalezaJuridicaBusiness
	{
		private readonly ITipoNaturalezaJuridicaRepository tipoNaturalezaJuridicaRepository;

		public TipoNaturalezaJuridicaBusiness(
			ITipoNaturalezaJuridicaRepository tipoNaturalezaJuridicaRepository
			)
		{
			this.tipoNaturalezaJuridicaRepository = tipoNaturalezaJuridicaRepository;
		}
		public TipoNaturalezaJuridicaDTO GetById(int id)
		{
			try
			{
				TipoNaturalezaJuridica tipoNaturalezaJuridica = tipoNaturalezaJuridicaRepository.GetById(id);

				TipoNaturalezaJuridicaDTO tipoNaturalezaJuridicaDTO = Mapper.Map<TipoNaturalezaJuridicaDTO>(tipoNaturalezaJuridica);

				return tipoNaturalezaJuridicaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoNaturalezaJuridicaDTO> Search(TipoNaturalezaJuridicaDTO dto)
		{
			try
			{
				List<TipoNaturalezaJuridica> tipoNaturalezaJuridicaList = tipoNaturalezaJuridicaRepository.Search(Mapper.Map<TipoNaturalezaJuridica>(dto));

				List<TipoNaturalezaJuridicaDTO> tipoNaturalezaJuridicaDTOList = Mapper.Map<List<TipoNaturalezaJuridicaDTO>>(tipoNaturalezaJuridicaList);

				return tipoNaturalezaJuridicaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoNaturalezaJuridicaDTO SingleSearch(TipoNaturalezaJuridicaDTO dto)
		{
			try
			{
				TipoNaturalezaJuridica tipoNaturalezaJuridica = tipoNaturalezaJuridicaRepository.SingleSearch(Mapper.Map<TipoNaturalezaJuridica>(dto));

				TipoNaturalezaJuridicaDTO tipoNaturalezaJuridicaDTO = Mapper.Map<TipoNaturalezaJuridicaDTO>(tipoNaturalezaJuridica);

				return tipoNaturalezaJuridicaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoNaturalezaJuridicaDTO dto)
		{
			try
			{
				TipoNaturalezaJuridica tipoNaturalezaJuridica = Mapper.Map<TipoNaturalezaJuridica>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoNaturalezaJuridicaRepository.Insert(tipoNaturalezaJuridica);

					dto.TipoNaturalezaJuridicaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoNaturalezaJuridicaDTO dto)
		{
			try
			{
				TipoNaturalezaJuridica tipoNaturalezaJuridica = Mapper.Map<TipoNaturalezaJuridica>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoNaturalezaJuridicaRepository.Update(tipoNaturalezaJuridica);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoNaturalezaJuridicaDTO dto)
		{
			try
			{
				TipoNaturalezaJuridica tipoNaturalezaJuridica = Mapper.Map<TipoNaturalezaJuridica>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoNaturalezaJuridicaRepository.Delete(tipoNaturalezaJuridica);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoNaturalezaJuridicaDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoNaturalezaJuridica tipoNaturalezaJuridica = tipoNaturalezaJuridicaRepository.ObtenerPorCodigo(codigo);

				TipoNaturalezaJuridicaDTO tipoNaturalezaJuridicaDTO = Mapper.Map<TipoNaturalezaJuridicaDTO>(tipoNaturalezaJuridica);

				return tipoNaturalezaJuridicaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
