using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class AtributoConfiguracionBusiness : IAtributoConfiguracionBusiness
	{
		private readonly IAtributoConfiguracionRepository atributoConfiguracionRepository;

		public AtributoConfiguracionBusiness(
			IAtributoConfiguracionRepository atributoConfiguracionRepository
			)
		{
			this.atributoConfiguracionRepository = atributoConfiguracionRepository;
		}
		public AtributoConfiguracionDTO GetById(int id)
		{
			try
			{
				AtributoConfiguracion atributoConfiguracion = atributoConfiguracionRepository.GetById(id);

				AtributoConfiguracionDTO atributoConfiguracionDTO = Mapper.Map<AtributoConfiguracionDTO>(atributoConfiguracion);

				return atributoConfiguracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<AtributoConfiguracionDTO> Search(AtributoConfiguracionDTO dto)
		{
			try
			{
				List<AtributoConfiguracion> atributoConfiguracionList = atributoConfiguracionRepository.Search(Mapper.Map<AtributoConfiguracion>(dto));

				List<AtributoConfiguracionDTO> atributoConfiguracionDTOList = Mapper.Map<List<AtributoConfiguracionDTO>>(atributoConfiguracionList);

				return atributoConfiguracionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public AtributoConfiguracionDTO SingleSearch(AtributoConfiguracionDTO dto)
		{
			try
			{
				AtributoConfiguracion atributoConfiguracion = atributoConfiguracionRepository.SingleSearch(Mapper.Map<AtributoConfiguracion>(dto));

				AtributoConfiguracionDTO atributoConfiguracionDTO = Mapper.Map<AtributoConfiguracionDTO>(atributoConfiguracion);

				return atributoConfiguracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(AtributoConfiguracionDTO dto)
		{
			try
			{
				AtributoConfiguracion atributoConfiguracion = Mapper.Map<AtributoConfiguracion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = atributoConfiguracionRepository.Insert(atributoConfiguracion);

					dto.AtributoConfiguracionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(AtributoConfiguracionDTO dto)
		{
			try
			{
				AtributoConfiguracion atributoConfiguracion = Mapper.Map<AtributoConfiguracion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = atributoConfiguracionRepository.Update(atributoConfiguracion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(AtributoConfiguracionDTO dto)
		{
			try
			{
				AtributoConfiguracion atributoConfiguracion = Mapper.Map<AtributoConfiguracion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					atributoConfiguracionRepository.Delete(atributoConfiguracion);

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
