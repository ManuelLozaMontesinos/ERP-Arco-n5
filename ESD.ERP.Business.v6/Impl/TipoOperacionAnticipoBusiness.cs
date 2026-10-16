using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoOperacionAnticipoBusiness : ITipoOperacionAnticipoBusiness
	{
		private readonly ITipoOperacionAnticipoRepository tipoOperacionAnticipoRepository;

		public TipoOperacionAnticipoBusiness(
			ITipoOperacionAnticipoRepository tipoOperacionAnticipoRepository
			)
		{
			this.tipoOperacionAnticipoRepository = tipoOperacionAnticipoRepository;
		}
		public TipoOperacionAnticipoDTO GetById(int id)
		{
			try
			{
				TipoOperacionAnticipo tipoOperacionAnticipo = tipoOperacionAnticipoRepository.GetById(id);

				TipoOperacionAnticipoDTO tipoOperacionAnticipoDTO = Mapper.Map<TipoOperacionAnticipoDTO>(tipoOperacionAnticipo);

				return tipoOperacionAnticipoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoOperacionAnticipoDTO> Search(TipoOperacionAnticipoDTO dto)
		{
			try
			{
				List<TipoOperacionAnticipo> tipoOperacionAnticipoList = tipoOperacionAnticipoRepository.Search(Mapper.Map<TipoOperacionAnticipo>(dto));

				List<TipoOperacionAnticipoDTO> tipoOperacionAnticipoDTOList = Mapper.Map<List<TipoOperacionAnticipoDTO>>(tipoOperacionAnticipoList);

				return tipoOperacionAnticipoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoOperacionAnticipoDTO SingleSearch(TipoOperacionAnticipoDTO dto)
		{
			try
			{
				TipoOperacionAnticipo tipoOperacionAnticipo = tipoOperacionAnticipoRepository.SingleSearch(Mapper.Map<TipoOperacionAnticipo>(dto));

				TipoOperacionAnticipoDTO tipoOperacionAnticipoDTO = Mapper.Map<TipoOperacionAnticipoDTO>(tipoOperacionAnticipo);

				return tipoOperacionAnticipoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoOperacionAnticipoDTO dto)
		{
			try
			{
				TipoOperacionAnticipo tipoOperacionAnticipo = Mapper.Map<TipoOperacionAnticipo>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = tipoOperacionAnticipoRepository.Insert(tipoOperacionAnticipo);

					dto.TipoOperacionAnticipoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoOperacionAnticipoDTO dto)
		{
			try
			{
				TipoOperacionAnticipo tipoOperacionAnticipo = Mapper.Map<TipoOperacionAnticipo>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = tipoOperacionAnticipoRepository.Update(tipoOperacionAnticipo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoOperacionAnticipoDTO dto)
		{
			try
			{
				TipoOperacionAnticipo tipoOperacionAnticipo = Mapper.Map<TipoOperacionAnticipo>(dto);

				using (TransactionScope tx = new())
				{
					tipoOperacionAnticipoRepository.Delete(tipoOperacionAnticipo);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoOperacionAnticipoDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoOperacionAnticipo tipoOperacionAnticipo = tipoOperacionAnticipoRepository.ObtenerPorCodigo(codigo);

				TipoOperacionAnticipoDTO tipoOperacionAnticipoDTO = Mapper.Map<TipoOperacionAnticipoDTO>(tipoOperacionAnticipo);

				return tipoOperacionAnticipoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
