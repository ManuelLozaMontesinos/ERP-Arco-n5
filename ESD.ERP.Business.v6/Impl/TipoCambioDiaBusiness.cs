using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoCambioDiaBusiness : ITipoCambioDiaBusiness
	{
		private readonly ITipoCambioDiaRepository tipoCambioDiaRepository;

		public TipoCambioDiaBusiness(
			ITipoCambioDiaRepository tipoCambioDiaRepository
			)
		{
			this.tipoCambioDiaRepository = tipoCambioDiaRepository;
		}
		public TipoCambioDiaDTO GetById(int id)
		{
			try
			{
				TipoCambioDia tipoCambioDia = tipoCambioDiaRepository.GetById(id);

				TipoCambioDiaDTO tipoCambioDiaDTO = Mapper.Map<TipoCambioDiaDTO>(tipoCambioDia);

				return tipoCambioDiaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoCambioDiaDTO> Search(TipoCambioDiaDTO dto)
		{
			try
			{
				List<TipoCambioDia> tipoCambioDiaList = tipoCambioDiaRepository.Search(Mapper.Map<TipoCambioDia>(dto));

				List<TipoCambioDiaDTO> tipoCambioDiaDTOList = Mapper.Map<List<TipoCambioDiaDTO>>(tipoCambioDiaList);

				return tipoCambioDiaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoCambioDiaDTO SingleSearch(TipoCambioDiaDTO dto)
		{
			try
			{
				TipoCambioDia tipoCambioDia = tipoCambioDiaRepository.SingleSearch(Mapper.Map<TipoCambioDia>(dto));

				TipoCambioDiaDTO tipoCambioDiaDTO = Mapper.Map<TipoCambioDiaDTO>(tipoCambioDia);

				return tipoCambioDiaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoCambioDiaDTO dto)
		{
			try
			{
				TipoCambioDia tipoCambioDia = Mapper.Map<TipoCambioDia>(dto);

				tipoCambioDia = tipoCambioDiaRepository.SingleSearch(tipoCambioDia);
				if (tipoCambioDia != null)
					throw new Exception(SR.GetString("EX_tipo_cambio_dia_ya_registrado"));

				tipoCambioDia = Mapper.Map<TipoCambioDia>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoCambioDiaRepository.Insert(tipoCambioDia);

					dto.TipoCambioDiaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoCambioDiaDTO dto)
		{
			try
			{
				TipoCambioDia tipoCambioDia = Mapper.Map<TipoCambioDia>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoCambioDiaRepository.Update(tipoCambioDia);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoCambioDiaDTO dto)
		{
			try
			{
				TipoCambioDia tipoCambioDia = Mapper.Map<TipoCambioDia>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoCambioDiaRepository.Delete(tipoCambioDia);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
