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
	public class UITBusiness : IUITBusiness
	{
		private readonly IUITRepository uitRepository;

		public UITBusiness(
			IUITRepository uitRepository
			)
		{
			this.uitRepository = uitRepository;
		}
		public UITDTO GetById(int id)
		{
			try
			{
				UIT uit = uitRepository.GetById(id);

				UITDTO uitDTO = Mapper.Map<UITDTO>(uit);

				return uitDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<UITDTO> Search(UITDTO dto)
		{
			try
			{
				List<UIT> uitList = uitRepository.Search(Mapper.Map<UIT>(dto));

				List<UITDTO> uitDTOList = Mapper.Map<List<UITDTO>>(uitList);

				return uitDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public UITDTO SingleSearch(UITDTO dto)
		{
			try
			{
				UIT uit = uitRepository.SingleSearch(Mapper.Map<UIT>(dto));

				UITDTO uitDTO = Mapper.Map<UITDTO>(uit);

				return uitDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(UITDTO dto)
		{
			try
			{
				UIT uit = Mapper.Map<UIT>(dto);
				uit.FechaBajaNull = SiNoConstante.S;

				uit = uitRepository.SingleSearch(uit);
				if (uit != null)
					throw new Exception(SR.GetString("EX_uit_ya_registrada"));

				uit = Mapper.Map<UIT>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = uitRepository.Insert(uit);

					dto.UITId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(UITDTO dto)
		{
			try
			{
				UIT uit = Mapper.Map<UIT>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = uitRepository.Update(uit);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(UITDTO dto)
		{
			try
			{
				UIT uit = Mapper.Map<UIT>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					uitRepository.Delete(uit);

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
