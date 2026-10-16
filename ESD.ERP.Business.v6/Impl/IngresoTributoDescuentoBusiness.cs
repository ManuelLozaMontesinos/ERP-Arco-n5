using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class IngresoTributoDescuentoBusiness : IIngresoTributoDescuentoBusiness
	{
		private readonly IIngresoTributoDescuentoRepository ingresoTributoDescuentoRepository;

		public IngresoTributoDescuentoBusiness(
			IIngresoTributoDescuentoRepository ingresoTributoDescuentoRepository
			)
		{
			this.ingresoTributoDescuentoRepository = ingresoTributoDescuentoRepository;
		}
		public IngresoTributoDescuentoDTO GetById(int id)
		{
			try
			{
				IngresoTributoDescuento ingresoTributoDescuento = ingresoTributoDescuentoRepository.GetById(id);

				IngresoTributoDescuentoDTO ingresoTributoDescuentoDTO = Mapper.Map<IngresoTributoDescuentoDTO>(ingresoTributoDescuento);

				return ingresoTributoDescuentoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<IngresoTributoDescuentoDTO> Search(IngresoTributoDescuentoDTO dto)
		{
			try
			{
				List<IngresoTributoDescuento> ingresoTributoDescuentoList = ingresoTributoDescuentoRepository.Search(Mapper.Map<IngresoTributoDescuento>(dto));

				List<IngresoTributoDescuentoDTO> ingresoTributoDescuentoDTOList = Mapper.Map<List<IngresoTributoDescuentoDTO>>(ingresoTributoDescuentoList);

				return ingresoTributoDescuentoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public IngresoTributoDescuentoDTO SingleSearch(IngresoTributoDescuentoDTO dto)
		{
			try
			{
				IngresoTributoDescuento ingresoTributoDescuento = ingresoTributoDescuentoRepository.SingleSearch(Mapper.Map<IngresoTributoDescuento>(dto));

				IngresoTributoDescuentoDTO ingresoTributoDescuentoDTO = Mapper.Map<IngresoTributoDescuentoDTO>(ingresoTributoDescuento);

				return ingresoTributoDescuentoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(IngresoTributoDescuentoDTO dto)
		{
			try
			{
				IngresoTributoDescuento ingresoTributoDescuento = Mapper.Map<IngresoTributoDescuento>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = ingresoTributoDescuentoRepository.Insert(ingresoTributoDescuento);

					dto.IngresoTributoDescuentoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(IngresoTributoDescuentoDTO dto)
		{
			try
			{
				IngresoTributoDescuento ingresoTributoDescuento = Mapper.Map<IngresoTributoDescuento>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = ingresoTributoDescuentoRepository.Update(ingresoTributoDescuento);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(IngresoTributoDescuentoDTO dto)
		{
			try
			{
				IngresoTributoDescuento ingresoTributoDescuento = Mapper.Map<IngresoTributoDescuento>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					ingresoTributoDescuentoRepository.Delete(ingresoTributoDescuento);

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
