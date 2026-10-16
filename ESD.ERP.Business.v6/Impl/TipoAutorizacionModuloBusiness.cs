using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoAutorizacionModuloBusiness : ITipoAutorizacionModuloBusiness
	{
		private readonly ITipoAutorizacionModuloRepository tipoAutorizacionModuloRepository;

		public TipoAutorizacionModuloBusiness(
			ITipoAutorizacionModuloRepository tipoAutorizacionModuloRepository
			)
		{
			this.tipoAutorizacionModuloRepository = tipoAutorizacionModuloRepository;
		}
		public TipoAutorizacionModuloDTO GetById(int id)
		{
			try
			{
				TipoAutorizacionModulo tipoAutorizacionModulo = tipoAutorizacionModuloRepository.GetById(id);

				TipoAutorizacionModuloDTO tipoAutorizacionModuloDTO = Mapper.Map<TipoAutorizacionModuloDTO>(tipoAutorizacionModulo);

				return tipoAutorizacionModuloDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoAutorizacionModuloDTO> Search(TipoAutorizacionModuloDTO dto)
		{
			try
			{
				List<TipoAutorizacionModulo> tipoAutorizacionModuloList = tipoAutorizacionModuloRepository.Search(Mapper.Map<TipoAutorizacionModulo>(dto));

				List<TipoAutorizacionModuloDTO> tipoAutorizacionModuloDTOList = Mapper.Map<List<TipoAutorizacionModuloDTO>>(tipoAutorizacionModuloList);

				return tipoAutorizacionModuloDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoAutorizacionModuloDTO SingleSearch(TipoAutorizacionModuloDTO dto)
		{
			try
			{
				TipoAutorizacionModulo tipoAutorizacionModulo = tipoAutorizacionModuloRepository.SingleSearch(Mapper.Map<TipoAutorizacionModulo>(dto));

				TipoAutorizacionModuloDTO tipoAutorizacionModuloDTO = Mapper.Map<TipoAutorizacionModuloDTO>(tipoAutorizacionModulo);

				return tipoAutorizacionModuloDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoAutorizacionModuloDTO dto)
		{
			try
			{
				TipoAutorizacionModulo tipoAutorizacionModulo = Mapper.Map<TipoAutorizacionModulo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoAutorizacionModuloRepository.Insert(tipoAutorizacionModulo);

					dto.TipoAutorizacionModuloId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoAutorizacionModuloDTO dto)
		{
			try
			{
				TipoAutorizacionModulo tipoAutorizacionModulo = Mapper.Map<TipoAutorizacionModulo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoAutorizacionModuloRepository.Update(tipoAutorizacionModulo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoAutorizacionModuloDTO dto)
		{
			try
			{
				TipoAutorizacionModulo tipoAutorizacionModulo = Mapper.Map<TipoAutorizacionModulo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoAutorizacionModuloRepository.Delete(tipoAutorizacionModulo);

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
