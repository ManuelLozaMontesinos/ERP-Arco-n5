using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoAccionAuditoriaBusiness : ITipoAccionAuditoriaBusiness
	{
		private readonly ITipoAccionAuditoriaRepository tipoAccionAuditoriaRepository;

		public TipoAccionAuditoriaBusiness(
			ITipoAccionAuditoriaRepository tipoAccionAuditoriaRepository
			)
		{
			this.tipoAccionAuditoriaRepository = tipoAccionAuditoriaRepository;
		}
		public TipoAccionAuditoriaDTO GetById(int id)
		{
			try
			{
				TipoAccionAuditoria tipoAccionAuditoria = tipoAccionAuditoriaRepository.GetById(id);

				TipoAccionAuditoriaDTO tipoAccionAuditoriaDTO = Mapper.Map<TipoAccionAuditoriaDTO>(tipoAccionAuditoria);

				return tipoAccionAuditoriaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoAccionAuditoriaDTO> Search(TipoAccionAuditoriaDTO dto)
		{
			try
			{
				List<TipoAccionAuditoria> tipoAccionAuditoriaList = tipoAccionAuditoriaRepository.Search(Mapper.Map<TipoAccionAuditoria>(dto));

				List<TipoAccionAuditoriaDTO> tipoAccionAuditoriaDTOList = Mapper.Map<List<TipoAccionAuditoriaDTO>>(tipoAccionAuditoriaList);

				return tipoAccionAuditoriaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoAccionAuditoriaDTO SingleSearch(TipoAccionAuditoriaDTO dto)
		{
			try
			{
				TipoAccionAuditoria tipoAccionAuditoria = tipoAccionAuditoriaRepository.SingleSearch(Mapper.Map<TipoAccionAuditoria>(dto));

				TipoAccionAuditoriaDTO tipoAccionAuditoriaDTO = Mapper.Map<TipoAccionAuditoriaDTO>(tipoAccionAuditoria);

				return tipoAccionAuditoriaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoAccionAuditoriaDTO dto)
		{
			try
			{
				TipoAccionAuditoria tipoAccionAuditoria = Mapper.Map<TipoAccionAuditoria>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoAccionAuditoriaRepository.Insert(tipoAccionAuditoria);

					dto.TipoAccionAuditoriaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoAccionAuditoriaDTO dto)
		{
			try
			{
				TipoAccionAuditoria tipoAccionAuditoria = Mapper.Map<TipoAccionAuditoria>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoAccionAuditoriaRepository.Update(tipoAccionAuditoria);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoAccionAuditoriaDTO dto)
		{
			try
			{
				TipoAccionAuditoria tipoAccionAuditoria = Mapper.Map<TipoAccionAuditoria>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoAccionAuditoriaRepository.Delete(tipoAccionAuditoria);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoAccionAuditoriaDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoAccionAuditoria tipoAccionAuditoria = tipoAccionAuditoriaRepository.ObtenerPorCodigo(codigo);

				TipoAccionAuditoriaDTO tipoAccionAuditoriaDTO = Mapper.Map<TipoAccionAuditoriaDTO>(tipoAccionAuditoria);

				return tipoAccionAuditoriaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
