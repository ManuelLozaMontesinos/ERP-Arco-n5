using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PersonalDependienteBusiness : IPersonalDependienteBusiness
	{
		private readonly IPersonalDependienteRepository personalDependienteRepository;

		public PersonalDependienteBusiness(
			IPersonalDependienteRepository personalDependienteRepository
			)
		{
			this.personalDependienteRepository = personalDependienteRepository;
		}
		public PersonalDependienteDTO GetById(int id)
		{
			try
			{
				PersonalDependiente personalDependiente = personalDependienteRepository.GetById(id);

				PersonalDependienteDTO personalDependienteDTO = Mapper.Map<PersonalDependienteDTO>(personalDependiente);

				return personalDependienteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalDependienteDTO> Search(PersonalDependienteDTO dto)
		{
			try
			{
				List<PersonalDependiente> personalDependienteList = personalDependienteRepository.Search(Mapper.Map<PersonalDependiente>(dto));

				List<PersonalDependienteDTO> personalDependienteDTOList = Mapper.Map<List<PersonalDependienteDTO>>(personalDependienteList);

				return personalDependienteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalDependienteDTO SingleSearch(PersonalDependienteDTO dto)
		{
			try
			{
				PersonalDependiente personalDependiente = personalDependienteRepository.SingleSearch(Mapper.Map<PersonalDependiente>(dto));

				PersonalDependienteDTO personalDependienteDTO = Mapper.Map<PersonalDependienteDTO>(personalDependiente);

				return personalDependienteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalDependienteDTO dto)
		{
			try
			{
				PersonalDependiente personalDependiente = Mapper.Map<PersonalDependiente>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = personalDependienteRepository.Insert(personalDependiente);

					dto.PersonalDependienteId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalDependienteDTO dto)
		{
			try
			{
				PersonalDependiente personalDependiente = Mapper.Map<PersonalDependiente>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = personalDependienteRepository.Update(personalDependiente);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalDependienteDTO dto)
		{
			try
			{
				PersonalDependiente personalDependiente = Mapper.Map<PersonalDependiente>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalDependienteRepository.Delete(personalDependiente);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarBaja(PersonalDependienteDTO dto)
      {
			try
			{
				PersonalDependiente personalDependiente = Mapper.Map<PersonalDependiente>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalDependienteRepository.ActualizarBaja(personalDependiente);

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
