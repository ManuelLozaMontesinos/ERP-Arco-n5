using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class LeadSourceBusiness : ILeadSourceBusiness
	{
		private readonly ILeadSourceRepository leadSourceRepository;

		public LeadSourceBusiness(
			ILeadSourceRepository leadSourceRepository
			)
		{
			this.leadSourceRepository = leadSourceRepository;
		}
		public LeadSourceDTO GetById(int id)
		{
			try
			{
				LeadSource leadSource = leadSourceRepository.GetById(id);

				LeadSourceDTO leadSourceDTO = Mapper.Map<LeadSourceDTO>(leadSource);

				return leadSourceDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<LeadSourceDTO> Search(LeadSourceDTO dto)
		{
			try
			{
				List<LeadSource> leadSourceList = leadSourceRepository.Search(Mapper.Map<LeadSource>(dto));

				List<LeadSourceDTO> leadSourceDTOList = Mapper.Map<List<LeadSourceDTO>>(leadSourceList);

				return leadSourceDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public LeadSourceDTO SingleSearch(LeadSourceDTO dto)
		{
			try
			{
				LeadSource leadSource = leadSourceRepository.SingleSearch(Mapper.Map<LeadSource>(dto));

				LeadSourceDTO leadSourceDTO = Mapper.Map<LeadSourceDTO>(leadSource);

				return leadSourceDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(LeadSourceDTO dto)
		{
			try
			{
				LeadSource leadSource = Mapper.Map<LeadSource>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = leadSourceRepository.Insert(leadSource);

					dto.LeadSourceId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(LeadSourceDTO dto)
		{
			try
			{
				LeadSource leadSource = Mapper.Map<LeadSource>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = leadSourceRepository.Update(leadSource);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(LeadSourceDTO dto)
		{
			try
			{
				LeadSource leadSource = Mapper.Map<LeadSource>(dto);

				using (TransactionScope tx = new())
				{
					leadSourceRepository.Delete(leadSource);

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
