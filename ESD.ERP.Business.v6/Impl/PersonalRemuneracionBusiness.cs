using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PersonalRemuneracionBusiness : IPersonalRemuneracionBusiness
	{
		private readonly IPersonalRemuneracionRepository personalRemuneracionRepository;

		public PersonalRemuneracionBusiness(
			IPersonalRemuneracionRepository personalRemuneracionRepository
			)
		{
			this.personalRemuneracionRepository = personalRemuneracionRepository;
		}
		public PersonalRemuneracionDTO GetById(int id)
		{
			try
			{
				PersonalRemuneracion personalRemuneracion = personalRemuneracionRepository.GetById(id);

				PersonalRemuneracionDTO personalRemuneracionDTO = Mapper.Map<PersonalRemuneracionDTO>(personalRemuneracion);

				return personalRemuneracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalRemuneracionDTO> Search(PersonalRemuneracionDTO dto)
		{
			try
			{
				List<PersonalRemuneracion> personalRemuneracionList = personalRemuneracionRepository.Search(Mapper.Map<PersonalRemuneracion>(dto));

				List<PersonalRemuneracionDTO> personalRemuneracionDTOList = Mapper.Map<List<PersonalRemuneracionDTO>>(personalRemuneracionList);

				return personalRemuneracionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalRemuneracionDTO SingleSearch(PersonalRemuneracionDTO dto)
		{
			try
			{
				PersonalRemuneracion personalRemuneracion = personalRemuneracionRepository.SingleSearch(Mapper.Map<PersonalRemuneracion>(dto));

				PersonalRemuneracionDTO personalRemuneracionDTO = Mapper.Map<PersonalRemuneracionDTO>(personalRemuneracion);

				return personalRemuneracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalRemuneracionDTO dto)
		{
			try
			{
				PersonalRemuneracion personalRemuneracion = Mapper.Map<PersonalRemuneracion>(dto);

				personalRemuneracion.FechaBajaNull = SiNoConstante.S;
				personalRemuneracion = personalRemuneracionRepository.SingleSearch(personalRemuneracion);
				if (personalRemuneracion != null)
					throw new Exception(SR.GetString("EX_tipo_remuneracion_ya_registrado", personalRemuneracion.TipoRemuneracion.Nombre));

				personalRemuneracion = Mapper.Map<PersonalRemuneracion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = personalRemuneracionRepository.Insert(personalRemuneracion);

					dto.PersonalRemuneracionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalRemuneracionDTO dto)
		{
			try
			{
				PersonalRemuneracion personalRemuneracion = Mapper.Map<PersonalRemuneracion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = personalRemuneracionRepository.Update(personalRemuneracion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalRemuneracionDTO dto)
		{
			try
			{
				PersonalRemuneracion personalRemuneracion = Mapper.Map<PersonalRemuneracion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalRemuneracionRepository.Delete(personalRemuneracion);

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
