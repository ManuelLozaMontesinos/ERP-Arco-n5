using System;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using System.Linq;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Repository.Impl;
using System.Collections;

namespace ESD.ERP.Business.Impl
{
	public class PersonalMaquinaBusiness : IPersonalMaquinaBusiness
    {
		private readonly IOrdenProduccionRepository ordenProduccionRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly IEstadoDespachoRepository estadoDespachoRepository;
		private readonly IOrdenProduccionSubProcesoRepository ordenProduccionSubProcesoRepository;
		private readonly IOrdenProduccionSubProcesoBusiness ordenProduccionSubProcesoBusiness;
		private readonly IPersonalMaquinaRepository personalMaquinaRepository;


        public PersonalMaquinaBusiness(
			IOrdenProduccionRepository ordenProduccionRepository,
			ITablaBusiness tablaBusiness,
			IEstadoDespachoRepository estadoDespachoRepository,
			IOrdenProduccionSubProcesoRepository ordenProduccionSubProcesoRepository,
            IOrdenProduccionSubProcesoBusiness ordenProduccionSubProcesoBusiness,
			IPersonalMaquinaRepository personalMaquinaRepository

            )
		{
			this.ordenProduccionRepository = ordenProduccionRepository;
			this.tablaBusiness = tablaBusiness;
			this.estadoDespachoRepository = estadoDespachoRepository;
			this.ordenProduccionSubProcesoRepository = ordenProduccionSubProcesoRepository;
			this.ordenProduccionSubProcesoBusiness = ordenProduccionSubProcesoBusiness;
			this.personalMaquinaRepository = personalMaquinaRepository;
        }
		public PersonalMaquinaDTO GetById(int id) 
		{
			try
			{
				
				PersonalMaquina personalMaquina = personalMaquinaRepository.GetById(id);

				PersonalMaquinaDTO personalMaquinaDTO = Mapper.Map<PersonalMaquinaDTO>(personalMaquina);

				return personalMaquinaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalMaquinaDTO> Search(PersonalMaquinaDTO dto)
		{
			try
			{
				List<PersonalMaquina> personalMaquinaList = personalMaquinaRepository.Search(Mapper.Map<PersonalMaquina>(dto));

				List<PersonalMaquinaDTO> personalMaquinaDTOList = Mapper.Map<List<PersonalMaquinaDTO>>(personalMaquinaList);

				return personalMaquinaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalMaquinaDTO SingleSearch(PersonalMaquinaDTO dto)
		{
			try
			{
				PersonalMaquina personalMaquina = personalMaquinaRepository.SingleSearch(Mapper.Map<PersonalMaquina>(dto));

				PersonalMaquinaDTO personalMaquinaDTO = Mapper.Map<PersonalMaquinaDTO>(personalMaquina);

				return personalMaquinaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalMaquinaDTO dto)
		{
			try
			{
				PersonalMaquina personalMaquina = Mapper.Map<PersonalMaquina>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalMaquinaDTO dto)
		{
			try
			{
				PersonalMaquina personalMaquina = Mapper.Map<PersonalMaquina>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = personalMaquinaRepository.Update(personalMaquina);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalMaquinaDTO dto)
		{
			try
			{
				PersonalMaquina personalMaquina = Mapper.Map<PersonalMaquina>(dto);

				using (TransactionScope tx = new())
				{
					personalMaquinaRepository.Delete(personalMaquina);

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
