using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ComboProductoBusiness : IComboProductoBusiness
	{
		private readonly IComboProductoRepository comboProductoRepository;

		public ComboProductoBusiness(
			IComboProductoRepository comboProductoRepository
			)
		{
			this.comboProductoRepository = comboProductoRepository;
		}
		public ComboProductoDTO GetById(int id)
		{
			try
			{
				ComboProducto comboProducto = comboProductoRepository.GetById(id);

				ComboProductoDTO comboProductoDTO = Mapper.Map<ComboProductoDTO>(comboProducto);

				return comboProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComboProductoDTO> Search(ComboProductoDTO dto)
		{
			try
			{
				List<ComboProducto> comboProductoList = comboProductoRepository.Search(Mapper.Map<ComboProducto>(dto));

				List<ComboProductoDTO> comboProductoDTOList = Mapper.Map<List<ComboProductoDTO>>(comboProductoList);

				return comboProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ComboProductoDTO SingleSearch(ComboProductoDTO dto)
		{
			try
			{
				ComboProducto comboProducto = comboProductoRepository.SingleSearch(Mapper.Map<ComboProducto>(dto));

				ComboProductoDTO comboProductoDTO = Mapper.Map<ComboProductoDTO>(comboProducto);

				return comboProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ComboProductoDTO dto)
		{
			try
			{
				ComboProducto comboProducto = Mapper.Map<ComboProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = comboProductoRepository.Insert(comboProducto);

					dto.ComboProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ComboProductoDTO dto)
		{
			try
			{
				ComboProducto comboProducto = Mapper.Map<ComboProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = comboProductoRepository.Update(comboProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ComboProductoDTO dto)
		{
			try
			{
				ComboProducto comboProducto = Mapper.Map<ComboProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					comboProductoRepository.Delete(comboProducto);

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
