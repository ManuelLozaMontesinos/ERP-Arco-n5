using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoCancelacionCuotaBusiness : ITipoCancelacionCuotaBusiness
	{
		private readonly ITipoCancelacionCuotaRepository tipoCancelacionCuotaRepository;

		public TipoCancelacionCuotaBusiness(
			ITipoCancelacionCuotaRepository tipoCancelacionCuotaRepository
			)
		{
			this.tipoCancelacionCuotaRepository = tipoCancelacionCuotaRepository;
		}
		public TipoCancelacionCuotaDTO GetById(int id)
		{
			try
			{
				TipoCancelacionCuota tipoCancelacionCuota = tipoCancelacionCuotaRepository.GetById(id);

				TipoCancelacionCuotaDTO tipoCancelacionCuotaDTO = Mapper.Map<TipoCancelacionCuotaDTO>(tipoCancelacionCuota);

				return tipoCancelacionCuotaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoCancelacionCuotaDTO> Search(TipoCancelacionCuotaDTO dto)
		{
			try
			{
				List<TipoCancelacionCuota> tipoCancelacionCuotaList = tipoCancelacionCuotaRepository.Search(Mapper.Map<TipoCancelacionCuota>(dto));

				List<TipoCancelacionCuotaDTO> tipoCancelacionCuotaDTOList = Mapper.Map<List<TipoCancelacionCuotaDTO>>(tipoCancelacionCuotaList);

				return tipoCancelacionCuotaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoCancelacionCuotaDTO SingleSearch(TipoCancelacionCuotaDTO dto)
		{
			try
			{
				TipoCancelacionCuota tipoCancelacionCuota = tipoCancelacionCuotaRepository.SingleSearch(Mapper.Map<TipoCancelacionCuota>(dto));

				TipoCancelacionCuotaDTO tipoCancelacionCuotaDTO = Mapper.Map<TipoCancelacionCuotaDTO>(tipoCancelacionCuota);

				return tipoCancelacionCuotaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoCancelacionCuotaDTO dto)
		{
			try
			{
				TipoCancelacionCuota tipoCancelacionCuota = Mapper.Map<TipoCancelacionCuota>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoCancelacionCuotaRepository.Insert(tipoCancelacionCuota);

					dto.TipoCancelacionCuotaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoCancelacionCuotaDTO dto)
		{
			try
			{
				TipoCancelacionCuota tipoCancelacionCuota = Mapper.Map<TipoCancelacionCuota>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoCancelacionCuotaRepository.Update(tipoCancelacionCuota);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoCancelacionCuotaDTO dto)
		{
			try
			{
				TipoCancelacionCuota tipoCancelacionCuota = Mapper.Map<TipoCancelacionCuota>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoCancelacionCuotaRepository.Delete(tipoCancelacionCuota);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoCancelacionCuotaDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoCancelacionCuota tipoCancelacionCuota = tipoCancelacionCuotaRepository.ObtenerPorCodigo(codigo);

				TipoCancelacionCuotaDTO tipoCancelacionCuotaDTO = Mapper.Map<TipoCancelacionCuotaDTO>(tipoCancelacionCuota);

				return tipoCancelacionCuotaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
