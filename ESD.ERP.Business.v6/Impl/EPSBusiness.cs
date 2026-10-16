using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EPSBusiness : IEPSBusiness
	{
		private readonly IEPSRepository epsRepository;

		public EPSBusiness(
			IEPSRepository epsRepository
			)
		{
			this.epsRepository = epsRepository;
		}
		public EPSDTO GetById(int id)
		{
			try
			{
				EPS eps = epsRepository.GetById(id);

				EPSDTO epsDTO = Mapper.Map<EPSDTO>(eps);

				return epsDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EPSDTO> Search(EPSDTO dto)
		{
			try
			{
				List<EPS> epsList = epsRepository.Search(Mapper.Map<EPS>(dto));

				List<EPSDTO> epsDTOList = Mapper.Map<List<EPSDTO>>(epsList);

				return epsDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EPSDTO SingleSearch(EPSDTO dto)
		{
			try
			{
				EPS eps = epsRepository.SingleSearch(Mapper.Map<EPS>(dto));

				EPSDTO epsDTO = Mapper.Map<EPSDTO>(eps);

				return epsDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EPSDTO dto)
		{
			try
			{
				EPS eps = Mapper.Map<EPS>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = epsRepository.Insert(eps);

					dto.EPSId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EPSDTO dto)
		{
			try
			{
				EPS eps = Mapper.Map<EPS>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = epsRepository.Update(eps);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EPSDTO dto)
		{
			try
			{
				EPS eps = Mapper.Map<EPS>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					epsRepository.Delete(eps);

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
