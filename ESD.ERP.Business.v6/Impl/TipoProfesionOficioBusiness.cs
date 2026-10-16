using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoProfesionOficioBusiness : ITipoProfesionOficioBusiness
	{
		private readonly ITipoProfesionOficioRepository TipoProfesionOficioRepository;

		public TipoProfesionOficioBusiness(
			ITipoProfesionOficioRepository TipoProfesionOficioRepository
			)
		{
			this.TipoProfesionOficioRepository = TipoProfesionOficioRepository;
		}
		public TipoProfesionOficioDTO GetById(int id)
		{
			try
			{
				TipoProfesionOficio TipoProfesionOficio = TipoProfesionOficioRepository.GetById(id);

				TipoProfesionOficioDTO TipoProfesionOficioDTO = Mapper.Map<TipoProfesionOficioDTO>(TipoProfesionOficio);

				return TipoProfesionOficioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoProfesionOficioDTO> Search(TipoProfesionOficioDTO dto)
		{
			try
			{
				List<TipoProfesionOficio> TipoProfesionOficioList = TipoProfesionOficioRepository.Search(Mapper.Map<TipoProfesionOficio>(dto));

				List<TipoProfesionOficioDTO> TipoProfesionOficioDTOList = Mapper.Map<List<TipoProfesionOficioDTO>>(TipoProfesionOficioList);

				return TipoProfesionOficioDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoProfesionOficioDTO SingleSearch(TipoProfesionOficioDTO dto)
		{
			try
			{
				TipoProfesionOficio TipoProfesionOficio = TipoProfesionOficioRepository.SingleSearch(Mapper.Map<TipoProfesionOficio>(dto));

				TipoProfesionOficioDTO TipoProfesionOficioDTO = Mapper.Map<TipoProfesionOficioDTO>(TipoProfesionOficio);

				return TipoProfesionOficioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoProfesionOficioDTO dto)
		{
			try
			{
				TipoProfesionOficio TipoProfesionOficio = Mapper.Map<TipoProfesionOficio>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = TipoProfesionOficioRepository.Insert(TipoProfesionOficio);

					dto.TipoProfesionOficioId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoProfesionOficioDTO dto)
		{
			try
			{
				TipoProfesionOficio TipoProfesionOficio = Mapper.Map<TipoProfesionOficio>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = TipoProfesionOficioRepository.Update(TipoProfesionOficio);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoProfesionOficioDTO dto)
		{
			try
			{
				TipoProfesionOficio TipoProfesionOficio = Mapper.Map<TipoProfesionOficio>(dto);

				using (TransactionScope tx = new())
				{
					TipoProfesionOficioRepository.Delete(TipoProfesionOficio);

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
