using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PersonalMarcacionHoraBusiness : IPersonalMarcacionHoraBusiness
	{
		private readonly IPersonalMarcacionHoraRepository personalMarcacionHoraRepository;

		public PersonalMarcacionHoraBusiness(
			IPersonalMarcacionHoraRepository personalMarcacionHoraRepository
			)
		{
			this.personalMarcacionHoraRepository = personalMarcacionHoraRepository;
		}
		public PersonalMarcacionHoraDTO GetById(int id)
		{
			try
			{
				PersonalMarcacionHora personalMarcacionHora = personalMarcacionHoraRepository.GetById(id);

				PersonalMarcacionHoraDTO personalMarcacionHoraDTO = Mapper.Map<PersonalMarcacionHoraDTO>(personalMarcacionHora);

				return personalMarcacionHoraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalMarcacionHoraDTO> Search(PersonalMarcacionHoraDTO dto)
		{
			try
			{
				List<PersonalMarcacionHora> personalMarcacionHoraList = personalMarcacionHoraRepository.Search(Mapper.Map<PersonalMarcacionHora>(dto));

				List<PersonalMarcacionHoraDTO> personalMarcacionHoraDTOList = Mapper.Map<List<PersonalMarcacionHoraDTO>>(personalMarcacionHoraList);

				return personalMarcacionHoraDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalMarcacionHoraDTO SingleSearch(PersonalMarcacionHoraDTO dto)
		{
			try
			{
				PersonalMarcacionHora personalMarcacionHora = personalMarcacionHoraRepository.SingleSearch(Mapper.Map<PersonalMarcacionHora>(dto));

				PersonalMarcacionHoraDTO personalMarcacionHoraDTO = Mapper.Map<PersonalMarcacionHoraDTO>(personalMarcacionHora);

				return personalMarcacionHoraDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalMarcacionHoraDTO dto)
		{
			try
			{
				PersonalMarcacionHora personalMarcacionHora = Mapper.Map<PersonalMarcacionHora>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = personalMarcacionHoraRepository.Insert(personalMarcacionHora);

					dto.PersonalMarcacionHoraId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalMarcacionHoraDTO dto)
		{
			try
			{
				PersonalMarcacionHora personalMarcacionHora = Mapper.Map<PersonalMarcacionHora>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = personalMarcacionHoraRepository.Update(personalMarcacionHora);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalMarcacionHoraDTO dto)
		{
			try
			{
				PersonalMarcacionHora personalMarcacionHora = Mapper.Map<PersonalMarcacionHora>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalMarcacionHoraRepository.Delete(personalMarcacionHora);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarLote(List<PersonalMarcacionHoraDTO> list)
      {
			try
			{
				List<PersonalMarcacionHora> personalMarcacionHoraList = Mapper.Map<List<PersonalMarcacionHora>>(list);

				using (TransactionScope tx = new TransactionScope())
				{
					personalMarcacionHoraRepository.ActualizarLote(personalMarcacionHoraList);

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
