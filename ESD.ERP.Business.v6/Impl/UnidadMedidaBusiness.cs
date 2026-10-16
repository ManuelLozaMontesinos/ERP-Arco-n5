using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class UnidadMedidaBusiness : IUnidadMedidaBusiness
	{
		private readonly IUnidadMedidaRepository unidadMedidaRepository;

		public UnidadMedidaBusiness(
			IUnidadMedidaRepository unidadMedidaRepository
			)
		{
			this.unidadMedidaRepository = unidadMedidaRepository;
		}
		public UnidadMedidaDTO GetById(int id)
		{
			try
			{
				UnidadMedida unidadMedida = unidadMedidaRepository.GetById(id);

				UnidadMedidaDTO unidadMedidaDTO = Mapper.Map<UnidadMedidaDTO>(unidadMedida);

				return unidadMedidaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<UnidadMedidaDTO> Search(UnidadMedidaDTO dto)
		{
			try
			{
				List<UnidadMedida> unidadMedidaList = unidadMedidaRepository.Search(Mapper.Map<UnidadMedida>(dto));

				List<UnidadMedidaDTO> unidadMedidaDTOList = Mapper.Map<List<UnidadMedidaDTO>>(unidadMedidaList);

				return unidadMedidaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public UnidadMedidaDTO SingleSearch(UnidadMedidaDTO dto)
		{
			try
			{
				UnidadMedida unidadMedida = unidadMedidaRepository.SingleSearch(Mapper.Map<UnidadMedida>(dto));

				UnidadMedidaDTO unidadMedidaDTO = Mapper.Map<UnidadMedidaDTO>(unidadMedida);

				return unidadMedidaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(UnidadMedidaDTO dto)
		{
			try
			{
				UnidadMedida unidadMedida = Mapper.Map<UnidadMedida>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = unidadMedidaRepository.Insert(unidadMedida);

					dto.UnidadMedidaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(UnidadMedidaDTO dto)
		{
			try
			{
				UnidadMedida unidadMedida = Mapper.Map<UnidadMedida>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = unidadMedidaRepository.Update(unidadMedida);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(UnidadMedidaDTO dto)
		{
			try
			{
				UnidadMedida unidadMedida = Mapper.Map<UnidadMedida>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					unidadMedidaRepository.Delete(unidadMedida);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public UnidadMedidaDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				UnidadMedida unidadMedida = unidadMedidaRepository.ObtenerPorCodigo(codigo);

				UnidadMedidaDTO unidadMedidaDTO = Mapper.Map<UnidadMedidaDTO>(unidadMedida);

				return unidadMedidaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
