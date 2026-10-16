using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoViaBusiness : ITipoViaBusiness
	{
		private readonly ITipoViaRepository tipoViaRepository;

		public TipoViaBusiness(
			ITipoViaRepository tipoViaRepository
			)
		{
			this.tipoViaRepository = tipoViaRepository;
		}
		public TipoViaDTO GetById(int id)
		{
			try
			{
				TipoVia tipoVia = tipoViaRepository.GetById(id);

				TipoViaDTO tipoViaDTO = Mapper.Map<TipoViaDTO>(tipoVia);

				return tipoViaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoViaDTO> Search(TipoViaDTO dto)
		{
			try
			{
				List<TipoVia> tipoViaList = tipoViaRepository.Search(Mapper.Map<TipoVia>(dto));

				List<TipoViaDTO> tipoViaDTOList = Mapper.Map<List<TipoViaDTO>>(tipoViaList);

				return tipoViaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoViaDTO SingleSearch(TipoViaDTO dto)
		{
			try
			{
				TipoVia tipoVia = tipoViaRepository.SingleSearch(Mapper.Map<TipoVia>(dto));

				TipoViaDTO tipoViaDTO = Mapper.Map<TipoViaDTO>(tipoVia);

				return tipoViaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoViaDTO dto)
		{
			try
			{
				TipoVia tipoVia = Mapper.Map<TipoVia>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoViaRepository.Insert(tipoVia);

					dto.TipoViaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoViaDTO dto)
		{
			try
			{
				TipoVia tipoVia = Mapper.Map<TipoVia>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoViaRepository.Update(tipoVia);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoViaDTO dto)
		{
			try
			{
				TipoVia tipoVia = Mapper.Map<TipoVia>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoViaRepository.Delete(tipoVia);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoViaDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoVia tipoVia = tipoViaRepository.ObtenerPorCodigo(codigo);

				TipoViaDTO tipoViaDTO = Mapper.Map<TipoViaDTO>(tipoVia);

				return tipoViaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
