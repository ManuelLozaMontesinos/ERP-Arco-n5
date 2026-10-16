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
	public class RegimenPensionarioEsquemaBusiness : IRegimenPensionarioEsquemaBusiness
	{
		private readonly IRegimenPensionarioEsquemaRepository regimenPensionarioEsquemaRepository;

		public RegimenPensionarioEsquemaBusiness(
			IRegimenPensionarioEsquemaRepository regimenPensionarioEsquemaRepository
			)
		{
			this.regimenPensionarioEsquemaRepository = regimenPensionarioEsquemaRepository;
		}
		public RegimenPensionarioEsquemaDTO GetById(int id)
		{
			try
			{
				RegimenPensionarioEsquema regimenPensionarioEsquema = regimenPensionarioEsquemaRepository.GetById(id);

				RegimenPensionarioEsquemaDTO regimenPensionarioEsquemaDTO = Mapper.Map<RegimenPensionarioEsquemaDTO>(regimenPensionarioEsquema);

				return regimenPensionarioEsquemaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RegimenPensionarioEsquemaDTO> Search(RegimenPensionarioEsquemaDTO dto)
		{
			try
			{
				List<RegimenPensionarioEsquema> regimenPensionarioEsquemaList = regimenPensionarioEsquemaRepository.Search(Mapper.Map<RegimenPensionarioEsquema>(dto));

				List<RegimenPensionarioEsquemaDTO> regimenPensionarioEsquemaDTOList = Mapper.Map<List<RegimenPensionarioEsquemaDTO>>(regimenPensionarioEsquemaList);

				return regimenPensionarioEsquemaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RegimenPensionarioEsquemaDTO SingleSearch(RegimenPensionarioEsquemaDTO dto)
		{
			try
			{
				RegimenPensionarioEsquema regimenPensionarioEsquema = regimenPensionarioEsquemaRepository.SingleSearch(Mapper.Map<RegimenPensionarioEsquema>(dto));

				RegimenPensionarioEsquemaDTO regimenPensionarioEsquemaDTO = Mapper.Map<RegimenPensionarioEsquemaDTO>(regimenPensionarioEsquema);

				return regimenPensionarioEsquemaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RegimenPensionarioEsquemaDTO dto)
		{
			try
			{
				RegimenPensionarioEsquema regimenPensionarioEsquema = Mapper.Map<RegimenPensionarioEsquema>(dto);

				regimenPensionarioEsquema = regimenPensionarioEsquemaRepository.SingleSearch(regimenPensionarioEsquema);
				if (regimenPensionarioEsquema != null)
					throw new Exception(SR.GetString("EX_regimen_pensionario_esquema_ya_registrado", regimenPensionarioEsquema.TipoEsquemaPension.Nombre));

				regimenPensionarioEsquema = Mapper.Map<RegimenPensionarioEsquema>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = regimenPensionarioEsquemaRepository.Insert(regimenPensionarioEsquema);

					dto.RegimenPensionarioEsquemaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RegimenPensionarioEsquemaDTO dto)
		{
			try
			{
				RegimenPensionarioEsquema regimenPensionarioEsquema = Mapper.Map<RegimenPensionarioEsquema>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = regimenPensionarioEsquemaRepository.Update(regimenPensionarioEsquema);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RegimenPensionarioEsquemaDTO dto)
		{
			try
			{
				RegimenPensionarioEsquema regimenPensionarioEsquema = Mapper.Map<RegimenPensionarioEsquema>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					regimenPensionarioEsquemaRepository.Delete(regimenPensionarioEsquema);

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
