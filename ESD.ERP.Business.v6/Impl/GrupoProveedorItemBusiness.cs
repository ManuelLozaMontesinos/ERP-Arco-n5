using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class GrupoProveedorItemBusiness : IGrupoProveedorItemBusiness
	{
		private readonly IGrupoProveedorItemRepository grupoProveedorItemRepository;

		public GrupoProveedorItemBusiness(
			IGrupoProveedorItemRepository grupoProveedorItemRepository
			)
		{
			this.grupoProveedorItemRepository = grupoProveedorItemRepository;
		}
		public GrupoProveedorItemDTO GetById(int id)
		{
			try
			{
				GrupoProveedorItem grupoProveedorItem = grupoProveedorItemRepository.GetById(id);

				GrupoProveedorItemDTO grupoProveedorItemDTO = Mapper.Map<GrupoProveedorItemDTO>(grupoProveedorItem);

				return grupoProveedorItemDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<GrupoProveedorItemDTO> Search(GrupoProveedorItemDTO dto)
		{
			try
			{
				List<GrupoProveedorItem> grupoProveedorItemList = grupoProveedorItemRepository.Search(Mapper.Map<GrupoProveedorItem>(dto));

				List<GrupoProveedorItemDTO> grupoProveedorItemDTOList = Mapper.Map<List<GrupoProveedorItemDTO>>(grupoProveedorItemList);

				return grupoProveedorItemDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public GrupoProveedorItemDTO SingleSearch(GrupoProveedorItemDTO dto)
		{
			try
			{
				GrupoProveedorItem grupoProveedorItem = grupoProveedorItemRepository.SingleSearch(Mapper.Map<GrupoProveedorItem>(dto));

				GrupoProveedorItemDTO grupoProveedorItemDTO = Mapper.Map<GrupoProveedorItemDTO>(grupoProveedorItem);

				return grupoProveedorItemDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(GrupoProveedorItemDTO dto)
		{
			try
			{
				GrupoProveedorItem grupoProveedorItem = Mapper.Map<GrupoProveedorItem>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = grupoProveedorItemRepository.Insert(grupoProveedorItem);

					dto.GrupoProveedorItemId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(GrupoProveedorItemDTO dto)
		{
			try
			{
				GrupoProveedorItem grupoProveedorItem = Mapper.Map<GrupoProveedorItem>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = grupoProveedorItemRepository.Update(grupoProveedorItem);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(GrupoProveedorItemDTO dto)
		{
			try
			{
				GrupoProveedorItem grupoProveedorItem = Mapper.Map<GrupoProveedorItem>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					grupoProveedorItemRepository.Delete(grupoProveedorItem);

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
