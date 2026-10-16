using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoProveedorBusiness : ITipoProveedorBusiness
	{
		private readonly ITipoProveedorRepository tipoProveedorRepository;

		public TipoProveedorBusiness(
			ITipoProveedorRepository tipoProveedorRepository
			)
		{
			this.tipoProveedorRepository = tipoProveedorRepository;
		}
		public TipoProveedorDTO GetById(int id)
		{
			try
			{
				TipoProveedor tipoProveedor = tipoProveedorRepository.GetById(id);

				TipoProveedorDTO tipoProveedorDTO = Mapper.Map<TipoProveedorDTO>(tipoProveedor);

				return tipoProveedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoProveedorDTO> Search(TipoProveedorDTO dto)
		{
			try
			{
				List<TipoProveedor> tipoProveedorList = tipoProveedorRepository.Search(Mapper.Map<TipoProveedor>(dto));

				List<TipoProveedorDTO> tipoProveedorDTOList = Mapper.Map<List<TipoProveedorDTO>>(tipoProveedorList);

				return tipoProveedorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoProveedorDTO SingleSearch(TipoProveedorDTO dto)
		{
			try
			{
				TipoProveedor tipoProveedor = tipoProveedorRepository.SingleSearch(Mapper.Map<TipoProveedor>(dto));

				TipoProveedorDTO tipoProveedorDTO = Mapper.Map<TipoProveedorDTO>(tipoProveedor);

				return tipoProveedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoProveedorDTO dto)
		{
			try
			{
				TipoProveedor tipoProveedor = Mapper.Map<TipoProveedor>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = tipoProveedorRepository.Insert(tipoProveedor);

					dto.TipoProveedorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoProveedorDTO dto)
		{
			try
			{
				TipoProveedor tipoProveedor = Mapper.Map<TipoProveedor>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = tipoProveedorRepository.Update(tipoProveedor);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoProveedorDTO dto)
		{
			try
			{
				TipoProveedor tipoProveedor = Mapper.Map<TipoProveedor>(dto);

				using (TransactionScope tx = new())
				{
					tipoProveedorRepository.Delete(tipoProveedor);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public TipoProveedorDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				TipoProveedor tipoProveedor = tipoProveedorRepository.ObtenerPorCodigo(codigo);

				TipoProveedorDTO tipoProveedorDTO = Mapper.Map<TipoProveedorDTO>(tipoProveedor);

				return tipoProveedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
