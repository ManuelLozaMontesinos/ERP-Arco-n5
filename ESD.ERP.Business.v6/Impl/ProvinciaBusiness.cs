using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ProvinciaBusiness : IProvinciaBusiness
	{
		private readonly IProvinciaRepository provinciaRepository;

		public ProvinciaBusiness(
			IProvinciaRepository provinciaRepository
			)
		{
			this.provinciaRepository = provinciaRepository;
		}
		public ProvinciaDTO GetById(int id)
		{
			try
			{
				Provincia provincia = provinciaRepository.GetById(id);

				ProvinciaDTO provinciaDTO = Mapper.Map<ProvinciaDTO>(provincia);

				return provinciaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProvinciaDTO> Search(ProvinciaDTO dto)
		{
			try
			{
				List<Provincia> provinciaList = provinciaRepository.Search(Mapper.Map<Provincia>(dto));

				List<ProvinciaDTO> provinciaDTOList = Mapper.Map<List<ProvinciaDTO>>(provinciaList);

				return provinciaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProvinciaDTO SingleSearch(ProvinciaDTO dto)
		{
			try
			{
				Provincia provincia = provinciaRepository.SingleSearch(Mapper.Map<Provincia>(dto));

				ProvinciaDTO provinciaDTO = Mapper.Map<ProvinciaDTO>(provincia);

				return provinciaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProvinciaDTO dto)
		{
			try
			{
				Provincia provincia = Mapper.Map<Provincia>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = provinciaRepository.Insert(provincia);

					dto.ProvinciaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProvinciaDTO dto)
		{
			try
			{
				Provincia provincia = Mapper.Map<Provincia>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = provinciaRepository.Update(provincia);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProvinciaDTO dto)
		{
			try
			{
				Provincia provincia = Mapper.Map<Provincia>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					provinciaRepository.Delete(provincia);

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
