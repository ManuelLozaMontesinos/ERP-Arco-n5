using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class GrupoProveedorBusiness : IGrupoProveedorBusiness
	{
		private readonly IGrupoProveedorRepository grupoProveedorRepository;

		public GrupoProveedorBusiness(
			IGrupoProveedorRepository grupoProveedorRepository
			)
		{
			this.grupoProveedorRepository = grupoProveedorRepository;
		}
		public GrupoProveedorDTO GetById(int id)
		{
			try
			{
				GrupoProveedor grupoProveedor = grupoProveedorRepository.GetById(id);

				GrupoProveedorDTO grupoProveedorDTO = Mapper.Map<GrupoProveedorDTO>(grupoProveedor);

				return grupoProveedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<GrupoProveedorDTO> Search(GrupoProveedorDTO dto)
		{
			try
			{
				List<GrupoProveedor> grupoProveedorList = grupoProveedorRepository.Search(Mapper.Map<GrupoProveedor>(dto));

				List<GrupoProveedorDTO> grupoProveedorDTOList = Mapper.Map<List<GrupoProveedorDTO>>(grupoProveedorList);

				return grupoProveedorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public GrupoProveedorDTO SingleSearch(GrupoProveedorDTO dto)
		{
			try
			{
				GrupoProveedor grupoProveedor = grupoProveedorRepository.SingleSearch(Mapper.Map<GrupoProveedor>(dto));

				GrupoProveedorDTO grupoProveedorDTO = Mapper.Map<GrupoProveedorDTO>(grupoProveedor);

				return grupoProveedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(GrupoProveedorDTO dto)
		{
			try
			{
				GrupoProveedor grupoProveedor = Mapper.Map<GrupoProveedor>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = grupoProveedorRepository.Insert(grupoProveedor);

					dto.GrupoProveedorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(GrupoProveedorDTO dto)
		{
			try
			{
				GrupoProveedor grupoProveedor = Mapper.Map<GrupoProveedor>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = grupoProveedorRepository.Update(grupoProveedor);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(GrupoProveedorDTO dto)
		{
			try
			{
				GrupoProveedor grupoProveedor = Mapper.Map<GrupoProveedor>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					grupoProveedorRepository.Delete(grupoProveedor);

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
