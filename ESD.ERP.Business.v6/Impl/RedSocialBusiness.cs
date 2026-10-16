using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class RedSocialBusiness : IRedSocialBusiness
	{
		private readonly IRedSocialRepository redSocialRepository;

		public RedSocialBusiness(
			IRedSocialRepository redSocialRepository
			)
		{
			this.redSocialRepository = redSocialRepository;
		}
		public RedSocialDTO GetById(int id)
		{
			try
			{
				RedSocial redSocial = redSocialRepository.GetById(id);

				RedSocialDTO redSocialDTO = Mapper.Map<RedSocialDTO>(redSocial);

				return redSocialDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RedSocialDTO> Search(RedSocialDTO dto)
		{
			try
			{
				List<RedSocial> redSocialList = redSocialRepository.Search(Mapper.Map<RedSocial>(dto));

				List<RedSocialDTO> redSocialDTOList = Mapper.Map<List<RedSocialDTO>>(redSocialList);

				return redSocialDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RedSocialDTO SingleSearch(RedSocialDTO dto)
		{
			try
			{
				RedSocial redSocial = redSocialRepository.SingleSearch(Mapper.Map<RedSocial>(dto));

				RedSocialDTO redSocialDTO = Mapper.Map<RedSocialDTO>(redSocial);

				return redSocialDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RedSocialDTO dto)
		{
			try
			{
				RedSocial redSocial = Mapper.Map<RedSocial>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = redSocialRepository.Insert(redSocial);

					dto.RedSocialId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RedSocialDTO dto)
		{
			try
			{
				RedSocial redSocial = Mapper.Map<RedSocial>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = redSocialRepository.Update(redSocial);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RedSocialDTO dto)
		{
			try
			{
				RedSocial redSocial = Mapper.Map<RedSocial>(dto);

				using (TransactionScope tx = new())
				{
					redSocialRepository.Delete(redSocial);

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
