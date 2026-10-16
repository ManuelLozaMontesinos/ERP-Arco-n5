using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PersonalContratoBusiness : IPersonalContratoBusiness
	{
		private readonly IPersonalContratoRepository personalContratoRepository;

		public PersonalContratoBusiness(
			IPersonalContratoRepository personalContratoRepository
			)
		{
			this.personalContratoRepository = personalContratoRepository;
		}
		public PersonalContratoDTO GetById(int id)
		{
			try
			{
				PersonalContrato personalContrato = personalContratoRepository.GetById(id);

				PersonalContratoDTO personalContratoDTO = Mapper.Map<PersonalContratoDTO>(personalContrato);

				return personalContratoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalContratoDTO> Search(PersonalContratoDTO dto)
		{
			try
			{
				List<PersonalContrato> personalContratoList = personalContratoRepository.Search(Mapper.Map<PersonalContrato>(dto));

				List<PersonalContratoDTO> personalContratoDTOList = Mapper.Map<List<PersonalContratoDTO>>(personalContratoList);

				return personalContratoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalContratoDTO SingleSearch(PersonalContratoDTO dto)
		{
			try
			{
				PersonalContrato personalContrato = personalContratoRepository.SingleSearch(Mapper.Map<PersonalContrato>(dto));

				PersonalContratoDTO personalContratoDTO = Mapper.Map<PersonalContratoDTO>(personalContrato);

				return personalContratoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalContratoDTO dto)
		{
			try
			{
				PersonalContrato personalContrato = Mapper.Map<PersonalContrato>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = personalContratoRepository.Insert(personalContrato);

					dto.PersonalContratoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalContratoDTO dto)
		{
			try
			{
				PersonalContrato personalContrato = Mapper.Map<PersonalContrato>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = personalContratoRepository.Update(personalContrato);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalContratoDTO dto)
		{
			try
			{
				PersonalContrato personalContrato = Mapper.Map<PersonalContrato>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalContratoRepository.Delete(personalContrato);

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
