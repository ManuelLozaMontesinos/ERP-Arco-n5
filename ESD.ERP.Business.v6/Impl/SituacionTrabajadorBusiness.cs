using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class SituacionTrabajadorBusiness : ISituacionTrabajadorBusiness
	{
		private readonly ISituacionTrabajadorRepository situacionTrabajadorRepository;

		public SituacionTrabajadorBusiness(
			ISituacionTrabajadorRepository situacionTrabajadorRepository
			)
		{
			this.situacionTrabajadorRepository = situacionTrabajadorRepository;
		}
		public SituacionTrabajadorDTO GetById(int id)
		{
			try
			{
				SituacionTrabajador situacionTrabajador = situacionTrabajadorRepository.GetById(id);

				SituacionTrabajadorDTO situacionTrabajadorDTO = Mapper.Map<SituacionTrabajadorDTO>(situacionTrabajador);

				return situacionTrabajadorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<SituacionTrabajadorDTO> Search(SituacionTrabajadorDTO dto)
		{
			try
			{
				List<SituacionTrabajador> situacionTrabajadorList = situacionTrabajadorRepository.Search(Mapper.Map<SituacionTrabajador>(dto));

				List<SituacionTrabajadorDTO> situacionTrabajadorDTOList = Mapper.Map<List<SituacionTrabajadorDTO>>(situacionTrabajadorList);

				return situacionTrabajadorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SituacionTrabajadorDTO SingleSearch(SituacionTrabajadorDTO dto)
		{
			try
			{
				SituacionTrabajador situacionTrabajador = situacionTrabajadorRepository.SingleSearch(Mapper.Map<SituacionTrabajador>(dto));

				SituacionTrabajadorDTO situacionTrabajadorDTO = Mapper.Map<SituacionTrabajadorDTO>(situacionTrabajador);

				return situacionTrabajadorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(SituacionTrabajadorDTO dto)
		{
			try
			{
				SituacionTrabajador situacionTrabajador = Mapper.Map<SituacionTrabajador>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = situacionTrabajadorRepository.Insert(situacionTrabajador);

					dto.SituacionTrabajadorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(SituacionTrabajadorDTO dto)
		{
			try
			{
				SituacionTrabajador situacionTrabajador = Mapper.Map<SituacionTrabajador>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = situacionTrabajadorRepository.Update(situacionTrabajador);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(SituacionTrabajadorDTO dto)
		{
			try
			{
				SituacionTrabajador situacionTrabajador = Mapper.Map<SituacionTrabajador>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					situacionTrabajadorRepository.Delete(situacionTrabajador);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SituacionTrabajadorDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				SituacionTrabajador situacionTrabajador = situacionTrabajadorRepository.ObtenerPorCodigo(codigo);

				SituacionTrabajadorDTO situacionTrabajadorDTO = Mapper.Map<SituacionTrabajadorDTO>(situacionTrabajador);

				return situacionTrabajadorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
