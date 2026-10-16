using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class SituacionEspecialTrabajadorBusiness : ISituacionEspecialTrabajadorBusiness
	{
		private readonly ISituacionEspecialTrabajadorRepository situacionEspecialTrabajadorRepository;

		public SituacionEspecialTrabajadorBusiness(
			ISituacionEspecialTrabajadorRepository situacionEspecialTrabajadorRepository
			)
		{
			this.situacionEspecialTrabajadorRepository = situacionEspecialTrabajadorRepository;
		}
		public SituacionEspecialTrabajadorDTO GetById(int id)
		{
			try
			{
				SituacionEspecialTrabajador situacionEspecialTrabajador = situacionEspecialTrabajadorRepository.GetById(id);

				SituacionEspecialTrabajadorDTO situacionEspecialTrabajadorDTO = Mapper.Map<SituacionEspecialTrabajadorDTO>(situacionEspecialTrabajador);

				return situacionEspecialTrabajadorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<SituacionEspecialTrabajadorDTO> Search(SituacionEspecialTrabajadorDTO dto)
		{
			try
			{
				List<SituacionEspecialTrabajador> situacionEspecialTrabajadorList = situacionEspecialTrabajadorRepository.Search(Mapper.Map<SituacionEspecialTrabajador>(dto));

				List<SituacionEspecialTrabajadorDTO> situacionEspecialTrabajadorDTOList = Mapper.Map<List<SituacionEspecialTrabajadorDTO>>(situacionEspecialTrabajadorList);

				return situacionEspecialTrabajadorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SituacionEspecialTrabajadorDTO SingleSearch(SituacionEspecialTrabajadorDTO dto)
		{
			try
			{
				SituacionEspecialTrabajador situacionEspecialTrabajador = situacionEspecialTrabajadorRepository.SingleSearch(Mapper.Map<SituacionEspecialTrabajador>(dto));

				SituacionEspecialTrabajadorDTO situacionEspecialTrabajadorDTO = Mapper.Map<SituacionEspecialTrabajadorDTO>(situacionEspecialTrabajador);

				return situacionEspecialTrabajadorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(SituacionEspecialTrabajadorDTO dto)
		{
			try
			{
				SituacionEspecialTrabajador situacionEspecialTrabajador = Mapper.Map<SituacionEspecialTrabajador>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = situacionEspecialTrabajadorRepository.Insert(situacionEspecialTrabajador);

					dto.SituacionEspecialTrabajadorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(SituacionEspecialTrabajadorDTO dto)
		{
			try
			{
				SituacionEspecialTrabajador situacionEspecialTrabajador = Mapper.Map<SituacionEspecialTrabajador>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = situacionEspecialTrabajadorRepository.Update(situacionEspecialTrabajador);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(SituacionEspecialTrabajadorDTO dto)
		{
			try
			{
				SituacionEspecialTrabajador situacionEspecialTrabajador = Mapper.Map<SituacionEspecialTrabajador>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					situacionEspecialTrabajadorRepository.Delete(situacionEspecialTrabajador);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SituacionEspecialTrabajadorDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				SituacionEspecialTrabajador situacionEspecialTrabajador = situacionEspecialTrabajadorRepository.ObtenerPorCodigo(codigo);

				SituacionEspecialTrabajadorDTO situacionEspecialTrabajadorDTO = Mapper.Map<SituacionEspecialTrabajadorDTO>(situacionEspecialTrabajador);

				return situacionEspecialTrabajadorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
