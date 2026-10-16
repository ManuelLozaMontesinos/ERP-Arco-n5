using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoPagoRemuneracionBusiness : ITipoPagoRemuneracionBusiness
	{
		private readonly ITipoPagoRemuneracionRepository tipoPagoRemuneracionRepository;

		public TipoPagoRemuneracionBusiness(
			ITipoPagoRemuneracionRepository tipoPagoRemuneracionRepository
			)
		{
			this.tipoPagoRemuneracionRepository = tipoPagoRemuneracionRepository;
		}
		public TipoPagoRemuneracionDTO GetById(int id)
		{
			try
			{
				TipoPagoRemuneracion tipoPagoRemuneracion = tipoPagoRemuneracionRepository.GetById(id);

				TipoPagoRemuneracionDTO tipoPagoRemuneracionDTO = Mapper.Map<TipoPagoRemuneracionDTO>(tipoPagoRemuneracion);

				return tipoPagoRemuneracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoPagoRemuneracionDTO> Search(TipoPagoRemuneracionDTO dto)
		{
			try
			{
				List<TipoPagoRemuneracion> tipoPagoRemuneracionList = tipoPagoRemuneracionRepository.Search(Mapper.Map<TipoPagoRemuneracion>(dto));

				List<TipoPagoRemuneracionDTO> tipoPagoRemuneracionDTOList = Mapper.Map<List<TipoPagoRemuneracionDTO>>(tipoPagoRemuneracionList);

				return tipoPagoRemuneracionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoPagoRemuneracionDTO SingleSearch(TipoPagoRemuneracionDTO dto)
		{
			try
			{
				TipoPagoRemuneracion tipoPagoRemuneracion = tipoPagoRemuneracionRepository.SingleSearch(Mapper.Map<TipoPagoRemuneracion>(dto));

				TipoPagoRemuneracionDTO tipoPagoRemuneracionDTO = Mapper.Map<TipoPagoRemuneracionDTO>(tipoPagoRemuneracion);

				return tipoPagoRemuneracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoPagoRemuneracionDTO dto)
		{
			try
			{
				TipoPagoRemuneracion tipoPagoRemuneracion = Mapper.Map<TipoPagoRemuneracion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoPagoRemuneracionRepository.Insert(tipoPagoRemuneracion);

					dto.TipoPagoRemuneracionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoPagoRemuneracionDTO dto)
		{
			try
			{
				TipoPagoRemuneracion tipoPagoRemuneracion = Mapper.Map<TipoPagoRemuneracion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoPagoRemuneracionRepository.Update(tipoPagoRemuneracion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoPagoRemuneracionDTO dto)
		{
			try
			{
				TipoPagoRemuneracion tipoPagoRemuneracion = Mapper.Map<TipoPagoRemuneracion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoPagoRemuneracionRepository.Delete(tipoPagoRemuneracion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoPagoRemuneracionDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoPagoRemuneracion tipoPagoRemuneracion = tipoPagoRemuneracionRepository.ObtenerPorCodigo(codigo);

				TipoPagoRemuneracionDTO tipoPagoRemuneracionDTO = Mapper.Map<TipoPagoRemuneracionDTO>(tipoPagoRemuneracion);

				return tipoPagoRemuneracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
