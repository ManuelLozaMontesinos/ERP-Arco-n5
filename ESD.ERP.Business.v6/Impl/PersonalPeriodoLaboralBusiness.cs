using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PersonalPeriodoLaboralBusiness : IPersonalPeriodoLaboralBusiness
	{
		private readonly IPersonalPeriodoLaboralRepository personalPeriodoLaboralRepository;
		private readonly IPersonalRepository personalRepository;

		public PersonalPeriodoLaboralBusiness(
			IPersonalPeriodoLaboralRepository personalPeriodoLaboralRepository,
			IPersonalRepository personalRepository
			)
		{
			this.personalPeriodoLaboralRepository = personalPeriodoLaboralRepository;
			this.personalRepository = personalRepository;
		}
		public PersonalPeriodoLaboralDTO GetById(int id)
		{
			try
			{
				PersonalPeriodoLaboral personalPeriodoLaboral = personalPeriodoLaboralRepository.GetById(id);

				PersonalPeriodoLaboralDTO personalPeriodoLaboralDTO = Mapper.Map<PersonalPeriodoLaboralDTO>(personalPeriodoLaboral);

				return personalPeriodoLaboralDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalPeriodoLaboralDTO> Search(PersonalPeriodoLaboralDTO dto)
		{
			try
			{
				List<PersonalPeriodoLaboral> personalPeriodoLaboralList = personalPeriodoLaboralRepository.Search(Mapper.Map<PersonalPeriodoLaboral>(dto));

				List<PersonalPeriodoLaboralDTO> personalPeriodoLaboralDTOList = Mapper.Map<List<PersonalPeriodoLaboralDTO>>(personalPeriodoLaboralList);

				return personalPeriodoLaboralDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalPeriodoLaboralDTO SingleSearch(PersonalPeriodoLaboralDTO dto)
		{
			try
			{
				PersonalPeriodoLaboral personalPeriodoLaboral = personalPeriodoLaboralRepository.SingleSearch(Mapper.Map<PersonalPeriodoLaboral>(dto));

				PersonalPeriodoLaboralDTO personalPeriodoLaboralDTO = Mapper.Map<PersonalPeriodoLaboralDTO>(personalPeriodoLaboral);

				return personalPeriodoLaboralDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalPeriodoLaboralDTO dto)
		{
			try
			{
				PersonalPeriodoLaboral personalPeriodoLaboral = Mapper.Map<PersonalPeriodoLaboral>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = personalPeriodoLaboralRepository.Insert(personalPeriodoLaboral);

					personalRepository.ActualizarFechaIngreso(new Personal()
					{
						PersonalId = personalPeriodoLaboral.PersonalId,
						FechaIngreso = personalPeriodoLaboral.FechaIngreso,
						FechaAlta = personalPeriodoLaboral.FechaAlta
					});

					personalRepository.ActualizarCese(new Personal()
					{
						PersonalId = personalPeriodoLaboral.PersonalId,
						MotivoFinPeriodoId = null,
						FechaCese = null,
						UsuarioCreacionId = personalPeriodoLaboral.UsuarioCreacionId
					});

					dto.PersonalPeriodoLaboralId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalPeriodoLaboralDTO dto)
		{
			try
			{
				PersonalPeriodoLaboral personalPeriodoLaboral = Mapper.Map<PersonalPeriodoLaboral>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = personalPeriodoLaboralRepository.Update(personalPeriodoLaboral);

					personalRepository.ActualizarFechaIngreso(new Personal()
					{
						PersonalId = personalPeriodoLaboral.PersonalId,
						FechaIngreso = personalPeriodoLaboral.FechaIngreso,
						FechaAlta = personalPeriodoLaboral.FechaAlta
					});

					personalRepository.ActualizarCese(new Personal()
					{
						PersonalId = personalPeriodoLaboral.PersonalId,
						MotivoFinPeriodoId = null,
						FechaCese = null,
						UsuarioCreacionId = personalPeriodoLaboral.UsuarioCreacionId
					});

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalPeriodoLaboralDTO dto)
		{
			try
			{
				PersonalPeriodoLaboral personalPeriodoLaboral = Mapper.Map<PersonalPeriodoLaboral>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalPeriodoLaboralRepository.Delete(personalPeriodoLaboral);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarCese(PersonalPeriodoLaboralDTO dto)
      {
			try
			{
				PersonalPeriodoLaboral personalPeriodoLaboral = Mapper.Map<PersonalPeriodoLaboral>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalPeriodoLaboralRepository.ActualizarCese(personalPeriodoLaboral);

					personalRepository.ActualizarCese(new Personal() { 
						PersonalId = personalPeriodoLaboral.PersonalId,
						MotivoFinPeriodoId = personalPeriodoLaboral.MotivoFinPeriodoId,
						FechaCese = personalPeriodoLaboral.FechaCese,
						UsuarioCreacionId = personalPeriodoLaboral.UsuarioCreacionId
					});

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
