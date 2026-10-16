using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class OcupacionBusiness : IOcupacionBusiness
	{
		private readonly IOcupacionRepository ocupacionRepository;

		public OcupacionBusiness(
			IOcupacionRepository ocupacionRepository
			)
		{
			this.ocupacionRepository = ocupacionRepository;
		}
		public OcupacionDTO GetById(int id)
		{
			try
			{
				Ocupacion ocupacion = ocupacionRepository.GetById(id);

				OcupacionDTO ocupacionDTO = Mapper.Map<OcupacionDTO>(ocupacion);

				return ocupacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OcupacionDTO> Search(OcupacionDTO dto)
		{
			try
			{
				List<Ocupacion> ocupacionList = ocupacionRepository.Search(Mapper.Map<Ocupacion>(dto));

				List<OcupacionDTO> ocupacionDTOList = Mapper.Map<List<OcupacionDTO>>(ocupacionList);

				return ocupacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OcupacionDTO SingleSearch(OcupacionDTO dto)
		{
			try
			{
				Ocupacion ocupacion = ocupacionRepository.SingleSearch(Mapper.Map<Ocupacion>(dto));

				OcupacionDTO ocupacionDTO = Mapper.Map<OcupacionDTO>(ocupacion);

				return ocupacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OcupacionDTO dto)
		{
			try
			{
				Ocupacion ocupacion = Mapper.Map<Ocupacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = ocupacionRepository.Insert(ocupacion);

					dto.OcupacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OcupacionDTO dto)
		{
			try
			{
				Ocupacion ocupacion = Mapper.Map<Ocupacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = ocupacionRepository.Update(ocupacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(OcupacionDTO dto)
		{
			try
			{
				Ocupacion ocupacion = Mapper.Map<Ocupacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					ocupacionRepository.Delete(ocupacion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OcupacionDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				Ocupacion ocupacion = ocupacionRepository.ObtenerPorCodigo(codigo);

				OcupacionDTO ocupacionDTO = Mapper.Map<OcupacionDTO>(ocupacion);

				return ocupacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
