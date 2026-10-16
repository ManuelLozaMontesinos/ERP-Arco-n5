using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoCargoAbonoBusiness : ITipoCargoAbonoBusiness
	{
		private readonly ITipoCargoAbonoRepository tipoCargoAbonoRepository;

		public TipoCargoAbonoBusiness(
			ITipoCargoAbonoRepository tipoCargoAbonoRepository
			)
		{
			this.tipoCargoAbonoRepository = tipoCargoAbonoRepository;
		}
		public TipoCargoAbonoDTO GetById(int id)
		{
			try
			{
				TipoCargoAbono tipoCargoAbono = tipoCargoAbonoRepository.GetById(id);

				TipoCargoAbonoDTO tipoCargoAbonoDTO = Mapper.Map<TipoCargoAbonoDTO>(tipoCargoAbono);

				return tipoCargoAbonoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoCargoAbonoDTO> Search(TipoCargoAbonoDTO dto)
		{
			try
			{
				List<TipoCargoAbono> tipoCargoAbonoList = tipoCargoAbonoRepository.Search(Mapper.Map<TipoCargoAbono>(dto));

				List<TipoCargoAbonoDTO> tipoCargoAbonoDTOList = Mapper.Map<List<TipoCargoAbonoDTO>>(tipoCargoAbonoList);

				return tipoCargoAbonoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoCargoAbonoDTO SingleSearch(TipoCargoAbonoDTO dto)
		{
			try
			{
				TipoCargoAbono tipoCargoAbono = tipoCargoAbonoRepository.SingleSearch(Mapper.Map<TipoCargoAbono>(dto));

				TipoCargoAbonoDTO tipoCargoAbonoDTO = Mapper.Map<TipoCargoAbonoDTO>(tipoCargoAbono);

				return tipoCargoAbonoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoCargoAbonoDTO dto)
		{
			try
			{
				TipoCargoAbono tipoCargoAbono = Mapper.Map<TipoCargoAbono>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoCargoAbonoRepository.Insert(tipoCargoAbono);

					dto.TipoCargoAbonoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoCargoAbonoDTO dto)
		{
			try
			{
				TipoCargoAbono tipoCargoAbono = Mapper.Map<TipoCargoAbono>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoCargoAbonoRepository.Update(tipoCargoAbono);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoCargoAbonoDTO dto)
		{
			try
			{
				TipoCargoAbono tipoCargoAbono = Mapper.Map<TipoCargoAbono>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoCargoAbonoRepository.Delete(tipoCargoAbono);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public TipoCargoAbonoDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				TipoCargoAbono tipoCargoAbono = tipoCargoAbonoRepository.ObtenerPorCodigo(codigo);

				TipoCargoAbonoDTO tipoCargoAbonoDTO = Mapper.Map<TipoCargoAbonoDTO>(tipoCargoAbono);

				return tipoCargoAbonoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
