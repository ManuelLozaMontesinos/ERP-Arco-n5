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
	public class PersonalMarcacionBusiness : IPersonalMarcacionBusiness
	{
		private readonly IPersonalMarcacionRepository personalMarcacionRepository;
		private readonly IPersonalMarcacionHoraRepository personalMarcacionHoraRepository;
		private readonly IRelojMarcacionRepository relojMarcacionRepository;

		public PersonalMarcacionBusiness(
			IPersonalMarcacionRepository personalMarcacionRepository,
			IPersonalMarcacionHoraRepository personalMarcacionHoraRepository,
			IRelojMarcacionRepository relojMarcacionRepository
			)
		{
			this.personalMarcacionRepository = personalMarcacionRepository;
			this.personalMarcacionHoraRepository = personalMarcacionHoraRepository;
			this.relojMarcacionRepository = relojMarcacionRepository;
		}
		public PersonalMarcacionDTO GetById(int id)
		{
			try
			{
				PersonalMarcacion personalMarcacion = personalMarcacionRepository.GetById(id);

				PersonalMarcacionDTO personalMarcacionDTO = Mapper.Map<PersonalMarcacionDTO>(personalMarcacion);

				return personalMarcacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalMarcacionDTO> Search(PersonalMarcacionDTO dto)
		{
			try
			{
				List<PersonalMarcacion> personalMarcacionList = personalMarcacionRepository.Search(Mapper.Map<PersonalMarcacion>(dto));

				List<PersonalMarcacionDTO> personalMarcacionDTOList = Mapper.Map<List<PersonalMarcacionDTO>>(personalMarcacionList);

				return personalMarcacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalMarcacionDTO SingleSearch(PersonalMarcacionDTO dto)
		{
			try
			{
				PersonalMarcacion personalMarcacion = personalMarcacionRepository.SingleSearch(Mapper.Map<PersonalMarcacion>(dto));

				PersonalMarcacionDTO personalMarcacionDTO = Mapper.Map<PersonalMarcacionDTO>(personalMarcacion);

				return personalMarcacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalMarcacionDTO dto)
		{
			try
			{
				PersonalMarcacion personalMarcacion = Mapper.Map<PersonalMarcacion>(dto);

				personalMarcacion = personalMarcacionRepository.SingleSearch(personalMarcacion);
				if (personalMarcacion != null)
					throw new Exception(SR.GetString("EX_personal_marcacion_ya_registrado"));

				personalMarcacion = Mapper.Map<PersonalMarcacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = personalMarcacionRepository.Insert(personalMarcacion);
					dto.PersonalMarcacionId = newId;

					if (dto.RelojMarcacionDTOList != null)
               {
						foreach (RelojMarcacionDTO relojMarcacionDTO in dto.RelojMarcacionDTOList)
							relojMarcacionRepository.Insert(Mapper.Map<RelojMarcacion>(relojMarcacionDTO));

						SincronizarMarcacion(dto);
					}

					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalMarcacionDTO dto)
		{
			try
			{
				PersonalMarcacion personalMarcacion = Mapper.Map<PersonalMarcacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = personalMarcacionRepository.Update(personalMarcacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalMarcacionDTO dto)
		{
			try
			{
				PersonalMarcacion personalMarcacion = Mapper.Map<PersonalMarcacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalMarcacionRepository.Delete(personalMarcacion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<dynamic> ListarMarcacionHoraPivot(PersonalMarcacionDTO dto)
      {
			try
			{
				List<dynamic> dynamicList = personalMarcacionRepository.ListarMarcacionHoraPivot(Mapper.Map<PersonalMarcacion>(dto));

				return dynamicList;
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void SincronizarMarcacion(PersonalMarcacionDTO dto)
      {
			try
			{
				PersonalMarcacion personalMarcacion = Mapper.Map<PersonalMarcacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalMarcacionRepository.SincronizarMarcacion(personalMarcacion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<PersonalDTO> ListarPersonalConHoras(PersonalMarcacionDTO dto)
      {
			try
			{
				List<Personal> personalList = personalMarcacionRepository.ListarPersonalConHoras(Mapper.Map<PersonalMarcacion>(dto));

				List<PersonalDTO> personalDTOList = Mapper.Map<List<PersonalDTO>>(personalList);

				return personalDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarHoraLote(PersonalMarcacionDTO dto)
      {
			try
			{
				PersonalMarcacion personalMarcacion = Mapper.Map<PersonalMarcacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalMarcacionRepository.Update(personalMarcacion);

					personalMarcacionHoraRepository.ActualizarLote(Mapper.Map<List<PersonalMarcacionHora>>(dto.PersonalMarcacionHoraDTOList));

					personalMarcacionRepository.SincronizarMarcacion(personalMarcacion);

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
