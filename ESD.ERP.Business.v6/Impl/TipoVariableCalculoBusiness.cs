using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoVariableCalculoBusiness : ITipoVariableCalculoBusiness
	{
		private readonly ITipoVariableCalculoRepository tipoVariableCalculoRepository;

		public TipoVariableCalculoBusiness(
			ITipoVariableCalculoRepository tipoVariableCalculoRepository
			)
		{
			this.tipoVariableCalculoRepository = tipoVariableCalculoRepository;
		}
		public TipoVariableCalculoDTO GetById(int id)
		{
			try
			{
				TipoVariableCalculo tipoVariableCalculo = tipoVariableCalculoRepository.GetById(id);

				TipoVariableCalculoDTO tipoVariableCalculoDTO = Mapper.Map<TipoVariableCalculoDTO>(tipoVariableCalculo);

				return tipoVariableCalculoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoVariableCalculoDTO> Search(TipoVariableCalculoDTO dto)
		{
			try
			{
				List<TipoVariableCalculo> tipoVariableCalculoList = tipoVariableCalculoRepository.Search(Mapper.Map<TipoVariableCalculo>(dto));

				List<TipoVariableCalculoDTO> tipoVariableCalculoDTOList = Mapper.Map<List<TipoVariableCalculoDTO>>(tipoVariableCalculoList);

				return tipoVariableCalculoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoVariableCalculoDTO SingleSearch(TipoVariableCalculoDTO dto)
		{
			try
			{
				TipoVariableCalculo tipoVariableCalculo = tipoVariableCalculoRepository.SingleSearch(Mapper.Map<TipoVariableCalculo>(dto));

				TipoVariableCalculoDTO tipoVariableCalculoDTO = Mapper.Map<TipoVariableCalculoDTO>(tipoVariableCalculo);

				return tipoVariableCalculoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoVariableCalculoDTO dto)
		{
			try
			{
				TipoVariableCalculo tipoVariableCalculo = Mapper.Map<TipoVariableCalculo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoVariableCalculoRepository.Insert(tipoVariableCalculo);

					dto.TipoVariableCalculoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoVariableCalculoDTO dto)
		{
			try
			{
				TipoVariableCalculo tipoVariableCalculo = Mapper.Map<TipoVariableCalculo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoVariableCalculoRepository.Update(tipoVariableCalculo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoVariableCalculoDTO dto)
		{
			try
			{
				TipoVariableCalculo tipoVariableCalculo = Mapper.Map<TipoVariableCalculo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoVariableCalculoRepository.Delete(tipoVariableCalculo);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoVariableCalculoDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoVariableCalculo tipoVariableCalculo = tipoVariableCalculoRepository.ObtenerPorCodigo(codigo);

				TipoVariableCalculoDTO tipoVariableCalculoDTO = Mapper.Map<TipoVariableCalculoDTO>(tipoVariableCalculo);

				return tipoVariableCalculoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
