using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PersonalSolicitudAccesoBusiness : IPersonalSolicitudAccesoBusiness
	{
		private readonly IPersonalSolicitudAccesoRepository personalSolicitudAccesoRepository;
		private readonly IEstadoAprobacionRepository estadoAprobacionRepository;

		public PersonalSolicitudAccesoBusiness(
			IPersonalSolicitudAccesoRepository personalSolicitudAccesoRepository,
			IEstadoAprobacionRepository estadoAprobacionRepository
			)
		{
			this.personalSolicitudAccesoRepository = personalSolicitudAccesoRepository;
			this.estadoAprobacionRepository = estadoAprobacionRepository;
		}
		public PersonalSolicitudAccesoDTO GetById(int id)
		{
			try
			{
				PersonalSolicitudAcceso personalSolicitudAcceso = personalSolicitudAccesoRepository.GetById(id);

				PersonalSolicitudAccesoDTO personalSolicitudAccesoDTO = Mapper.Map<PersonalSolicitudAccesoDTO>(personalSolicitudAcceso);

				return personalSolicitudAccesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalSolicitudAccesoDTO> Search(PersonalSolicitudAccesoDTO dto)
		{
			try
			{
				List<PersonalSolicitudAcceso> personalSolicitudAccesoList = personalSolicitudAccesoRepository.Search(Mapper.Map<PersonalSolicitudAcceso>(dto));

				List<PersonalSolicitudAccesoDTO> personalSolicitudAccesoDTOList = Mapper.Map<List<PersonalSolicitudAccesoDTO>>(personalSolicitudAccesoList);

				return personalSolicitudAccesoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalSolicitudAccesoDTO SingleSearch(PersonalSolicitudAccesoDTO dto)
		{
			try
			{
				PersonalSolicitudAcceso personalSolicitudAcceso = personalSolicitudAccesoRepository.SingleSearch(Mapper.Map<PersonalSolicitudAcceso>(dto));

				PersonalSolicitudAccesoDTO personalSolicitudAccesoDTO = Mapper.Map<PersonalSolicitudAccesoDTO>(personalSolicitudAcceso);

				return personalSolicitudAccesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalSolicitudAccesoDTO dto)
		{
			try
			{
				PersonalSolicitudAcceso personalSolicitudAcceso = Mapper.Map<PersonalSolicitudAcceso>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = personalSolicitudAccesoRepository.Insert(personalSolicitudAcceso);

					dto.PersonalSolicitudAccesoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalSolicitudAccesoDTO dto)
		{
			try
			{
				PersonalSolicitudAcceso personalSolicitudAcceso = Mapper.Map<PersonalSolicitudAcceso>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = personalSolicitudAccesoRepository.Update(personalSolicitudAcceso);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalSolicitudAccesoDTO dto)
		{
			try
			{
				PersonalSolicitudAcceso personalSolicitudAcceso = Mapper.Map<PersonalSolicitudAcceso>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalSolicitudAccesoRepository.Delete(personalSolicitudAcceso);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Aprobar(PersonalSolicitudAccesoDTO dto)
		{
			try
			{
				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO); ;

				using (TransactionScope tx = new())
				{

					personalSolicitudAccesoRepository.ActualizarEstadoAprobacion(new PersonalSolicitudAcceso()
					{
						PersonalSolicitudAccesoId = dto.PersonalSolicitudAccesoId,
						PersonalAutorizaId = dto.PersonalAutorizaId,
						HoraAutorizada = dto.HoraAutorizada,
						EstadoAprobacionId = estadoAprobacion.EntityId
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Desaprobar(PersonalSolicitudAccesoDTO dto)
		{
			EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.DESAPROBADO); ;

			using (TransactionScope tx = new())
			{

				personalSolicitudAccesoRepository.ActualizarEstadoAprobacion(new PersonalSolicitudAcceso()
				{
					PersonalSolicitudAccesoId = dto.PersonalSolicitudAccesoId,
					PersonalAutorizaId = dto.PersonalAutorizaId,
					HoraAutorizada = string.Empty,
					EstadoAprobacionId = estadoAprobacion.EntityId
				});

				tx.Complete();
			}
		}

	}
}
