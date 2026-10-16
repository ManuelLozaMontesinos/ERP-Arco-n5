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
	public class PersonalPlanillaBusiness : IPersonalPlanillaBusiness
	{
		private readonly IPersonalPlanillaRepository personalPlanillaRepository;

		public PersonalPlanillaBusiness(
			IPersonalPlanillaRepository personalPlanillaRepository
			)
		{
			this.personalPlanillaRepository = personalPlanillaRepository;
		}
		public PersonalPlanillaDTO GetById(int id)
		{
			try
			{
				PersonalPlanilla personalPlanilla = personalPlanillaRepository.GetById(id);

				PersonalPlanillaDTO personalPlanillaDTO = Mapper.Map<PersonalPlanillaDTO>(personalPlanilla);

				return personalPlanillaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalPlanillaDTO> Search(PersonalPlanillaDTO dto)
		{
			try
			{
				List<PersonalPlanilla> personalPlanillaList = personalPlanillaRepository.Search(Mapper.Map<PersonalPlanilla>(dto));

				List<PersonalPlanillaDTO> personalPlanillaDTOList = Mapper.Map<List<PersonalPlanillaDTO>>(personalPlanillaList);

				return personalPlanillaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalPlanillaDTO SingleSearch(PersonalPlanillaDTO dto)
		{
			try
			{
				PersonalPlanilla personalPlanilla = personalPlanillaRepository.SingleSearch(Mapper.Map<PersonalPlanilla>(dto));

				PersonalPlanillaDTO personalPlanillaDTO = Mapper.Map<PersonalPlanillaDTO>(personalPlanilla);

				return personalPlanillaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalPlanillaDTO dto)
		{
			try
			{
				PersonalPlanilla personalPlanilla = Mapper.Map<PersonalPlanilla>(dto);

				int newId = 0;

				personalPlanilla = personalPlanillaRepository.SingleSearch(personalPlanilla);
				if (personalPlanilla != null)
					throw new Exception(SR.GetString("EX_planilla_ya_registrada", personalPlanilla.Planilla.Nombre));

				personalPlanilla = Mapper.Map<PersonalPlanilla>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					newId = personalPlanillaRepository.Insert(personalPlanilla);

					dto.PersonalPlanillaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalPlanillaDTO dto)
		{
			try
			{
				PersonalPlanilla personalPlanilla = Mapper.Map<PersonalPlanilla>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = personalPlanillaRepository.Update(personalPlanilla);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalPlanillaDTO dto)
		{
			try
			{
				PersonalPlanilla personalPlanilla = Mapper.Map<PersonalPlanilla>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalPlanillaRepository.Delete(personalPlanilla);

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
