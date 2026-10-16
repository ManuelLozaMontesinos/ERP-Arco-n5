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
	public class ProveedorRedSocialBusiness : IProveedorRedSocialBusiness
	{
		private readonly IProveedorRedSocialRepository proveedorRedSocialRepository;

		public ProveedorRedSocialBusiness(
			IProveedorRedSocialRepository proveedorRedSocialRepository
			)
		{
			this.proveedorRedSocialRepository = proveedorRedSocialRepository;
		}
		public ProveedorRedSocialDTO GetById(int id)
		{
			try
			{
				ProveedorRedSocial proveedorRedSocial = proveedorRedSocialRepository.GetById(id);

				ProveedorRedSocialDTO proveedorRedSocialDTO = Mapper.Map<ProveedorRedSocialDTO>(proveedorRedSocial);

				return proveedorRedSocialDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProveedorRedSocialDTO> Search(ProveedorRedSocialDTO dto)
		{
			try
			{
				List<ProveedorRedSocial> proveedorRedSocialList = proveedorRedSocialRepository.Search(Mapper.Map<ProveedorRedSocial>(dto));

				List<ProveedorRedSocialDTO> proveedorRedSocialDTOList = Mapper.Map<List<ProveedorRedSocialDTO>>(proveedorRedSocialList);

				return proveedorRedSocialDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProveedorRedSocialDTO SingleSearch(ProveedorRedSocialDTO dto)
		{
			try
			{
				ProveedorRedSocial proveedorRedSocial = proveedorRedSocialRepository.SingleSearch(Mapper.Map<ProveedorRedSocial>(dto));

				ProveedorRedSocialDTO proveedorRedSocialDTO = Mapper.Map<ProveedorRedSocialDTO>(proveedorRedSocial);

				return proveedorRedSocialDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProveedorRedSocialDTO dto)
		{
			try
			{
				ProveedorRedSocial proveedorRedSocial = Mapper.Map<ProveedorRedSocial>(dto);

				proveedorRedSocial = proveedorRedSocialRepository.SingleSearch(proveedorRedSocial);
				if (proveedorRedSocial != null)
					throw new Exception(SR.GetString("EX_red_social_ya_registrada"));

				proveedorRedSocial = Mapper.Map<ProveedorRedSocial>(dto);
				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = proveedorRedSocialRepository.Insert(proveedorRedSocial);

					dto.ProveedorRedSocialId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProveedorRedSocialDTO dto)
		{
			try
			{
				ProveedorRedSocial proveedorRedSocial = Mapper.Map<ProveedorRedSocial>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = proveedorRedSocialRepository.Update(proveedorRedSocial);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProveedorRedSocialDTO dto)
		{
			try
			{
				ProveedorRedSocial proveedorRedSocial = Mapper.Map<ProveedorRedSocial>(dto);

				using (TransactionScope tx = new())
				{
					proveedorRedSocialRepository.Delete(proveedorRedSocial);

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
