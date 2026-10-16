using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class GuiaRemisionProductoBusiness : IGuiaRemisionProductoBusiness
	{
		private readonly IGuiaRemisionProductoRepository guiaRemisionProductoRepository;

		public GuiaRemisionProductoBusiness(
			IGuiaRemisionProductoRepository guiaRemisionProductoRepository
			)
		{
			this.guiaRemisionProductoRepository = guiaRemisionProductoRepository;
		}
		public GuiaRemisionProductoDTO GetById(int id)
		{
			try
			{
				GuiaRemisionProducto guiaRemisionProducto = guiaRemisionProductoRepository.GetById(id);

				GuiaRemisionProductoDTO guiaRemisionProductoDTO = Mapper.Map<GuiaRemisionProductoDTO>(guiaRemisionProducto);

				return guiaRemisionProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<GuiaRemisionProductoDTO> Search(GuiaRemisionProductoDTO dto)
		{
			try
			{
				List<GuiaRemisionProducto> guiaRemisionProductoList = guiaRemisionProductoRepository.Search(Mapper.Map<GuiaRemisionProducto>(dto));

				List<GuiaRemisionProductoDTO> guiaRemisionProductoDTOList = Mapper.Map<List<GuiaRemisionProductoDTO>>(guiaRemisionProductoList);

				return guiaRemisionProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public GuiaRemisionProductoDTO SingleSearch(GuiaRemisionProductoDTO dto)
		{
			try
			{
				GuiaRemisionProducto guiaRemisionProducto = guiaRemisionProductoRepository.SingleSearch(Mapper.Map<GuiaRemisionProducto>(dto));

				GuiaRemisionProductoDTO guiaRemisionProductoDTO = Mapper.Map<GuiaRemisionProductoDTO>(guiaRemisionProducto);

				return guiaRemisionProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(GuiaRemisionProductoDTO dto)
		{
			try
			{
				GuiaRemisionProducto guiaRemisionProducto = Mapper.Map<GuiaRemisionProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = guiaRemisionProductoRepository.Insert(guiaRemisionProducto);

					dto.GuiaRemisionProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(GuiaRemisionProductoDTO dto)
		{
			try
			{
				GuiaRemisionProducto guiaRemisionProducto = Mapper.Map<GuiaRemisionProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = guiaRemisionProductoRepository.Update(guiaRemisionProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(GuiaRemisionProductoDTO dto)
		{
			try
			{
				GuiaRemisionProducto guiaRemisionProducto = Mapper.Map<GuiaRemisionProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					guiaRemisionProductoRepository.Delete(guiaRemisionProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void EliminarPorGuiaRemisionId(GuiaRemisionProductoDTO dto)
      {
			try
			{
				GuiaRemisionProducto guiaRemisionProducto = Mapper.Map<GuiaRemisionProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					guiaRemisionProductoRepository.EliminarPorGuiaRemisionId(guiaRemisionProducto);

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
