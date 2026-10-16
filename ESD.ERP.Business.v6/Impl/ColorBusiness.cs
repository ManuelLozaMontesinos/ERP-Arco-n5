using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ColorBusiness : IColorBusiness
	{
		private readonly IColorRepository colorRepository;

		public ColorBusiness(
			IColorRepository colorRepository
			)
		{
			this.colorRepository = colorRepository;
		}
		public ColorDTO GetById(int id)
		{
			try
			{
				Color color = colorRepository.GetById(id);

				ColorDTO colorDTO = Mapper.Map<ColorDTO>(color);

				return colorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ColorDTO> Search(ColorDTO dto)
		{
			try
			{
				List<Color> colorList = colorRepository.Search(Mapper.Map<Color>(dto));

				List<ColorDTO> colorDTOList = Mapper.Map<List<ColorDTO>>(colorList);

				return colorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ColorDTO SingleSearch(ColorDTO dto)
		{
			try
			{
				Color color = colorRepository.SingleSearch(Mapper.Map<Color>(dto));

				ColorDTO colorDTO = Mapper.Map<ColorDTO>(color);

				return colorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ColorDTO dto)
		{
			try
			{
				Color color = Mapper.Map<Color>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = colorRepository.Insert(color);

					dto.ColorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ColorDTO dto)
		{
			try
			{
				Color color = Mapper.Map<Color>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = colorRepository.Update(color);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ColorDTO dto)
		{
			try
			{
				Color color = Mapper.Map<Color>(dto);

				using (TransactionScope tx = new())
				{
					colorRepository.Delete(color);

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
