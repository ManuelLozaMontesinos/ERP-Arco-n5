using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ParametroSistemaBusiness : IParametroSistemaBusiness
	{
		private readonly IParametroSistemaRepository parametroSistemaRepository;

		public ParametroSistemaBusiness(
			IParametroSistemaRepository parametroSistemaRepository
			)
		{
			this.parametroSistemaRepository = parametroSistemaRepository;
		}
		public ParametroSistemaDTO GetById(int id)
			{
				try
				{
					ParametroSistema parametroSistema = parametroSistemaRepository.GetById(id);

					ParametroSistemaDTO parametroSistemaDTO = Mapper.Map<ParametroSistemaDTO>(parametroSistema);

					return parametroSistemaDTO;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public List<ParametroSistemaDTO> Search(ParametroSistemaDTO dto)
			{
				try
				{
					List<ParametroSistema> parametroSistemaList = parametroSistemaRepository.Search(Mapper.Map<ParametroSistema>(dto));

					List<ParametroSistemaDTO> parametroSistemaDTOList = Mapper.Map<List<ParametroSistemaDTO>>(parametroSistemaList);

					return parametroSistemaDTOList;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public ParametroSistemaDTO SingleSearch(ParametroSistemaDTO dto)
			{
				try
				{
					ParametroSistema parametroSistema = parametroSistemaRepository.SingleSearch(Mapper.Map<ParametroSistema>(dto));

					ParametroSistemaDTO parametroSistemaDTO = Mapper.Map<ParametroSistemaDTO>(parametroSistema);

					return parametroSistemaDTO;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public int Insert(ParametroSistemaDTO dto)
			{
				try
				{
					ParametroSistema parametroSistema = Mapper.Map<ParametroSistema>(dto);

					int newId = 0;
					using (TransactionScope tx = new TransactionScope())
					{
						newId = parametroSistemaRepository.Insert(parametroSistema);

						dto.ParametroSistemaId = newId;
						tx.Complete();
					}

					return newId;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public int Update(ParametroSistemaDTO dto)
			{
				try
				{
					ParametroSistema parametroSistema = Mapper.Map<ParametroSistema>(dto);

					int id = 0;
					using (TransactionScope tx = new TransactionScope())
					{
						id = parametroSistemaRepository.Update(parametroSistema);

						tx.Complete();
					}

					return id;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public void Delete(ParametroSistemaDTO dto)
			{
				try
				{
					ParametroSistema parametroSistema = Mapper.Map<ParametroSistema>(dto);

					using (TransactionScope tx = new TransactionScope())
					{
						parametroSistemaRepository.Delete(parametroSistema);

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
