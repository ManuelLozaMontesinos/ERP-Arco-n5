using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoContratoBusiness : ITipoContratoBusiness
	{
		private readonly ITipoContratoRepository tipoContratoRepository;

		public TipoContratoBusiness(
			ITipoContratoRepository tipoContratoRepository
			)
		{
			this.tipoContratoRepository = tipoContratoRepository;
		}
		public TipoContratoDTO GetById(int id)
		{
			try
			{
				TipoContrato tipoContrato = tipoContratoRepository.GetById(id);

				TipoContratoDTO tipoContratoDTO = Mapper.Map<TipoContratoDTO>(tipoContrato);

				return tipoContratoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoContratoDTO> Search(TipoContratoDTO dto)
		{
			try
			{
				List<TipoContrato> tipoContratoList = tipoContratoRepository.Search(Mapper.Map<TipoContrato>(dto));

				List<TipoContratoDTO> tipoContratoDTOList = Mapper.Map<List<TipoContratoDTO>>(tipoContratoList);

				return tipoContratoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoContratoDTO SingleSearch(TipoContratoDTO dto)
		{
			try
			{
				TipoContrato tipoContrato = tipoContratoRepository.SingleSearch(Mapper.Map<TipoContrato>(dto));

				TipoContratoDTO tipoContratoDTO = Mapper.Map<TipoContratoDTO>(tipoContrato);

				return tipoContratoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoContratoDTO dto)
		{
			try
			{
				TipoContrato tipoContrato = Mapper.Map<TipoContrato>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoContratoRepository.Insert(tipoContrato);

					dto.TipoContratoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoContratoDTO dto)
		{
			try
			{
				TipoContrato tipoContrato = Mapper.Map<TipoContrato>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoContratoRepository.Update(tipoContrato);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoContratoDTO dto)
		{
			try
			{
				TipoContrato tipoContrato = Mapper.Map<TipoContrato>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoContratoRepository.Delete(tipoContrato);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoContratoDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoContrato tipoContrato = tipoContratoRepository.ObtenerPorCodigo(codigo);

				TipoContratoDTO tipoContratoDTO = Mapper.Map<TipoContratoDTO>(tipoContrato);

				return tipoContratoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
