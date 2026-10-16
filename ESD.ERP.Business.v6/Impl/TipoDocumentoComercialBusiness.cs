using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoDocumentoComercialBusiness : ITipoDocumentoComercialBusiness
	{
		private readonly ITipoDocumentoComercialRepository tipoDocumentoComercialRepository;

		public TipoDocumentoComercialBusiness(
			ITipoDocumentoComercialRepository tipoDocumentoComercialRepository
			)
		{
			this.tipoDocumentoComercialRepository = tipoDocumentoComercialRepository;
		}
		public TipoDocumentoComercialDTO GetById(int id)
		{
			try
			{
				TipoDocumentoComercial tipoDocumentoComercial = tipoDocumentoComercialRepository.GetById(id);

				TipoDocumentoComercialDTO tipoDocumentoComercialDTO = Mapper.Map<TipoDocumentoComercialDTO>(tipoDocumentoComercial);

				return tipoDocumentoComercialDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoDocumentoComercialDTO> Search(TipoDocumentoComercialDTO dto)
		{
			try
			{
				List<TipoDocumentoComercial> tipoDocumentoComercialList = tipoDocumentoComercialRepository.Search(Mapper.Map<TipoDocumentoComercial>(dto));

				List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = Mapper.Map<List<TipoDocumentoComercialDTO>>(tipoDocumentoComercialList);

				return tipoDocumentoComercialDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoDocumentoComercialDTO SingleSearch(TipoDocumentoComercialDTO dto)
		{
			try
			{
				TipoDocumentoComercial tipoDocumentoComercial = tipoDocumentoComercialRepository.SingleSearch(Mapper.Map<TipoDocumentoComercial>(dto));

				TipoDocumentoComercialDTO tipoDocumentoComercialDTO = Mapper.Map<TipoDocumentoComercialDTO>(tipoDocumentoComercial);

				return tipoDocumentoComercialDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoDocumentoComercialDTO dto)
		{
			try
			{
				TipoDocumentoComercial tipoDocumentoComercial = Mapper.Map<TipoDocumentoComercial>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoDocumentoComercialRepository.Insert(tipoDocumentoComercial);

					dto.TipoDocumentoComercialId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoDocumentoComercialDTO dto)
		{
			try
			{
				TipoDocumentoComercial tipoDocumentoComercial = Mapper.Map<TipoDocumentoComercial>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoDocumentoComercialRepository.Update(tipoDocumentoComercial);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoDocumentoComercialDTO dto)
		{
			try
			{
				TipoDocumentoComercial tipoDocumentoComercial = Mapper.Map<TipoDocumentoComercial>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoDocumentoComercialRepository.Delete(tipoDocumentoComercial);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public TipoDocumentoComercialDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				TipoDocumentoComercial tipoDocumentoComercial = tipoDocumentoComercialRepository.ObtenerPorCodigo(codigo);

				TipoDocumentoComercialDTO tipoDocumentoComercialDTO = Mapper.Map<TipoDocumentoComercialDTO>(tipoDocumentoComercial);

				return tipoDocumentoComercialDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
