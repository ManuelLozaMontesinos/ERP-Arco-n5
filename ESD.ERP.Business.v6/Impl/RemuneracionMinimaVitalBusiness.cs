using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class RemuneracionMinimaVitalBusiness : IRemuneracionMinimaVitalBusiness
	{
		private readonly IRemuneracionMinimaVitalRepository remuneracionMinimaVitalRepository;

		public RemuneracionMinimaVitalBusiness(
			IRemuneracionMinimaVitalRepository remuneracionMinimaVitalRepository
			)
		{
			this.remuneracionMinimaVitalRepository = remuneracionMinimaVitalRepository;
		}
		public RemuneracionMinimaVitalDTO GetById(int id)
		{
			try
			{
				RemuneracionMinimaVital remuneracionMinimaVital = remuneracionMinimaVitalRepository.GetById(id);

				RemuneracionMinimaVitalDTO remuneracionMinimaVitalDTO = Mapper.Map<RemuneracionMinimaVitalDTO>(remuneracionMinimaVital);

				return remuneracionMinimaVitalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RemuneracionMinimaVitalDTO> Search(RemuneracionMinimaVitalDTO dto)
		{
			try
			{
				List<RemuneracionMinimaVital> remuneracionMinimaVitalList = remuneracionMinimaVitalRepository.Search(Mapper.Map<RemuneracionMinimaVital>(dto));

				List<RemuneracionMinimaVitalDTO> remuneracionMinimaVitalDTOList = Mapper.Map<List<RemuneracionMinimaVitalDTO>>(remuneracionMinimaVitalList);

				return remuneracionMinimaVitalDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RemuneracionMinimaVitalDTO SingleSearch(RemuneracionMinimaVitalDTO dto)
		{
			try
			{
				RemuneracionMinimaVital remuneracionMinimaVital = remuneracionMinimaVitalRepository.SingleSearch(Mapper.Map<RemuneracionMinimaVital>(dto));

				RemuneracionMinimaVitalDTO remuneracionMinimaVitalDTO = Mapper.Map<RemuneracionMinimaVitalDTO>(remuneracionMinimaVital);

				return remuneracionMinimaVitalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RemuneracionMinimaVitalDTO dto)
		{
			try
			{

				RemuneracionMinimaVital remuneracionMinimaVital = Mapper.Map<RemuneracionMinimaVital>(dto);
				remuneracionMinimaVital.FechaBajaNull = SiNoConstante.S;

				remuneracionMinimaVital = remuneracionMinimaVitalRepository.SingleSearch(remuneracionMinimaVital);
				if (remuneracionMinimaVital != null)
					throw new Exception(SR.GetString("EX_rmv_ya_registrada"));

				remuneracionMinimaVital = Mapper.Map<RemuneracionMinimaVital>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = remuneracionMinimaVitalRepository.Insert(remuneracionMinimaVital);

					dto.RemuneracionMinimaVitalId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RemuneracionMinimaVitalDTO dto)
		{
			try
			{
				RemuneracionMinimaVital remuneracionMinimaVital = Mapper.Map<RemuneracionMinimaVital>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = remuneracionMinimaVitalRepository.Update(remuneracionMinimaVital);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RemuneracionMinimaVitalDTO dto)
		{
			try
			{
				RemuneracionMinimaVital remuneracionMinimaVital = Mapper.Map<RemuneracionMinimaVital>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					remuneracionMinimaVitalRepository.Delete(remuneracionMinimaVital);

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
