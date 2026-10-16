using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PersonalVacacionesBusiness : IPersonalVacacionBusiness
	{
		private readonly IPersonalVacacionRepository personalVacacionRepository;

		public PersonalVacacionesBusiness(
			IPersonalVacacionRepository personalVacacionRepository
			)
		{
			this.personalVacacionRepository = personalVacacionRepository;
		}
		public PersonalVacacionDTO GetById(int id)
		{
			try
			{
				PersonalVacacion personalVacacion = personalVacacionRepository.GetById(id);

				PersonalVacacionDTO personalVacacionDTO = Mapper.Map<PersonalVacacionDTO>(personalVacacion);

				return personalVacacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalVacacionDTO> Search(PersonalVacacionDTO dto)
		{
			try
			{
				List<PersonalVacacion> personalVacacionList = personalVacacionRepository.Search(Mapper.Map<PersonalVacacion>(dto));

				List<PersonalVacacionDTO> personalVacacionDTOList = Mapper.Map<List<PersonalVacacionDTO>>(personalVacacionList);

				return personalVacacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalVacacionDTO SingleSearch(PersonalVacacionDTO dto)
		{
			try
			{
				PersonalVacacion personalVacacion = personalVacacionRepository.SingleSearch(Mapper.Map<PersonalVacacion>(dto));

				PersonalVacacionDTO personalVacacionDTO = Mapper.Map<PersonalVacacionDTO>(personalVacacion);

				return personalVacacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalVacacionDTO dto)
		{
			try
			{
				PersonalVacacion personalVacacion = Mapper.Map<PersonalVacacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = personalVacacionRepository.Insert(personalVacacion);

					dto.PersonalVacacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalVacacionDTO dto)
		{
			try
			{
				PersonalVacacion personalVacacion = Mapper.Map<PersonalVacacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = personalVacacionRepository.Update(personalVacacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalVacacionDTO dto)
		{
			try
			{
				PersonalVacacion personalVacacion = Mapper.Map<PersonalVacacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalVacacionRepository.Delete(personalVacacion);

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
