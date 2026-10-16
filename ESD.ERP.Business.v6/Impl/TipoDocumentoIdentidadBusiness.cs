using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoDocumentoIdentidadBusiness : ITipoDocumentoIdentidadBusiness
	{
		private readonly ITipoDocumentoIdentidadRepository tipoDocumentoIdentidadRepository;

		public TipoDocumentoIdentidadBusiness(
			ITipoDocumentoIdentidadRepository tipoDocumentoIdentidadRepository
			)
		{
			this.tipoDocumentoIdentidadRepository = tipoDocumentoIdentidadRepository;
		}
		public TipoDocumentoIdentidadDTO GetById(int id)
		{
			try
			{
				TipoDocumentoIdentidad tipoDocumentoIdentidad = tipoDocumentoIdentidadRepository.GetById(id);

				TipoDocumentoIdentidadDTO tipoDocumentoIdentidadDTO = Mapper.Map<TipoDocumentoIdentidadDTO>(tipoDocumentoIdentidad);

				return tipoDocumentoIdentidadDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoDocumentoIdentidadDTO> Search(TipoDocumentoIdentidadDTO dto)
		{
			try
			{
				List<TipoDocumentoIdentidad> tipoDocumentoIdentidadList = tipoDocumentoIdentidadRepository.Search(Mapper.Map<TipoDocumentoIdentidad>(dto));

				List<TipoDocumentoIdentidadDTO> tipoDocumentoIdentidadDTOList = Mapper.Map<List<TipoDocumentoIdentidadDTO>>(tipoDocumentoIdentidadList);

				return tipoDocumentoIdentidadDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoDocumentoIdentidadDTO SingleSearch(TipoDocumentoIdentidadDTO dto)
		{
			try
			{
				TipoDocumentoIdentidad tipoDocumentoIdentidad = tipoDocumentoIdentidadRepository.SingleSearch(Mapper.Map<TipoDocumentoIdentidad>(dto));

				TipoDocumentoIdentidadDTO tipoDocumentoIdentidadDTO = Mapper.Map<TipoDocumentoIdentidadDTO>(tipoDocumentoIdentidad);

				return tipoDocumentoIdentidadDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoDocumentoIdentidadDTO dto)
		{
			try
			{
				TipoDocumentoIdentidad tipoDocumentoIdentidad = Mapper.Map<TipoDocumentoIdentidad>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoDocumentoIdentidadRepository.Insert(tipoDocumentoIdentidad);

					dto.TipoDocumentoIdentidadId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoDocumentoIdentidadDTO dto)
		{
			try
			{
				TipoDocumentoIdentidad tipoDocumentoIdentidad = Mapper.Map<TipoDocumentoIdentidad>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoDocumentoIdentidadRepository.Update(tipoDocumentoIdentidad);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoDocumentoIdentidadDTO dto)
		{
			try
			{
				TipoDocumentoIdentidad tipoDocumentoIdentidad = Mapper.Map<TipoDocumentoIdentidad>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoDocumentoIdentidadRepository.Delete(tipoDocumentoIdentidad);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoDocumentoIdentidadDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoDocumentoIdentidad tipoDocumentoIdentidad = tipoDocumentoIdentidadRepository.ObtenerPorCodigo(codigo);

				TipoDocumentoIdentidadDTO tipoDocumentoIdentidadDTO = Mapper.Map<TipoDocumentoIdentidadDTO>(tipoDocumentoIdentidad);

				return tipoDocumentoIdentidadDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
