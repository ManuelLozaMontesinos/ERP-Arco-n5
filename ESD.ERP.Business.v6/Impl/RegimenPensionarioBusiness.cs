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
	public class RegimenPensionarioBusiness : IRegimenPensionarioBusiness
	{
		private readonly IRegimenPensionarioRepository regimenPensionarioRepository;

		public RegimenPensionarioBusiness(
			IRegimenPensionarioRepository regimenPensionarioRepository
			)
		{
			this.regimenPensionarioRepository = regimenPensionarioRepository;
		}
		public RegimenPensionarioDTO GetById(int id)
		{
			try
			{
				RegimenPensionario regimenPensionario = regimenPensionarioRepository.GetById(id);

				RegimenPensionarioDTO regimenPensionarioDTO = Mapper.Map<RegimenPensionarioDTO>(regimenPensionario);

				return regimenPensionarioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RegimenPensionarioDTO> Search(RegimenPensionarioDTO dto)
		{
			try
			{
				List<RegimenPensionario> regimenPensionarioList = regimenPensionarioRepository.Search(Mapper.Map<RegimenPensionario>(dto));

				List<RegimenPensionarioDTO> regimenPensionarioDTOList = Mapper.Map<List<RegimenPensionarioDTO>>(regimenPensionarioList);

				return regimenPensionarioDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RegimenPensionarioDTO SingleSearch(RegimenPensionarioDTO dto)
		{
			try
			{
				RegimenPensionario regimenPensionario = regimenPensionarioRepository.SingleSearch(Mapper.Map<RegimenPensionario>(dto));

				RegimenPensionarioDTO regimenPensionarioDTO = Mapper.Map<RegimenPensionarioDTO>(regimenPensionario);

				return regimenPensionarioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RegimenPensionarioDTO dto)
		{
			try
			{
				RegimenPensionario regimenPensionario = Mapper.Map<RegimenPensionario>(dto);

				regimenPensionario = regimenPensionarioRepository.SingleSearch(regimenPensionario);
				if (regimenPensionario != null)
					throw new Exception(SR.GetString("EX_regimen_pensionario_ya_registrado", regimenPensionario.Codigo));

				regimenPensionario = Mapper.Map<RegimenPensionario>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = regimenPensionarioRepository.Insert(regimenPensionario);

					dto.RegimenPensionarioId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RegimenPensionarioDTO dto)
		{
			try
			{
				RegimenPensionario regimenPensionario = Mapper.Map<RegimenPensionario>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = regimenPensionarioRepository.Update(regimenPensionario);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RegimenPensionarioDTO dto)
		{
			try
			{
				RegimenPensionario regimenPensionario = Mapper.Map<RegimenPensionario>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					regimenPensionarioRepository.Delete(regimenPensionario);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RegimenPensionarioDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				RegimenPensionario regimenPensionario = regimenPensionarioRepository.ObtenerPorCodigo(codigo);

				RegimenPensionarioDTO regimenPensionarioDTO = Mapper.Map<RegimenPensionarioDTO>(regimenPensionario);

				return regimenPensionarioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
