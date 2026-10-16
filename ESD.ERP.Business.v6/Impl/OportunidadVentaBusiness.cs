using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class OportunidadVentaBusiness : IOportunidadVentaBusiness
	{
		private readonly IOportunidadVentaRepository oportunidadVentaRepository;

		public OportunidadVentaBusiness(
			IOportunidadVentaRepository oportunidadVentaRepository
			)
		{
			this.oportunidadVentaRepository = oportunidadVentaRepository;
		}
		public OportunidadVentaDTO GetById(int id)
		{
			try
			{
				OportunidadVenta oportunidadVenta = oportunidadVentaRepository.GetById(id);

				OportunidadVentaDTO oportunidadVentaDTO = Mapper.Map<OportunidadVentaDTO>(oportunidadVenta);

				return oportunidadVentaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OportunidadVentaDTO> Search(OportunidadVentaDTO dto)
		{
			try
			{
				List<OportunidadVenta> oportunidadVentaList = oportunidadVentaRepository.Search(Mapper.Map<OportunidadVenta>(dto));

				List<OportunidadVentaDTO> oportunidadVentaDTOList = Mapper.Map<List<OportunidadVentaDTO>>(oportunidadVentaList);

				return oportunidadVentaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OportunidadVentaDTO SingleSearch(OportunidadVentaDTO dto)
		{
			try
			{
				OportunidadVenta oportunidadVenta = oportunidadVentaRepository.SingleSearch(Mapper.Map<OportunidadVenta>(dto));

				OportunidadVentaDTO oportunidadVentaDTO = Mapper.Map<OportunidadVentaDTO>(oportunidadVenta);

				return oportunidadVentaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OportunidadVentaDTO dto)
		{
			try
			{
				OportunidadVenta oportunidadVenta = Mapper.Map<OportunidadVenta>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = oportunidadVentaRepository.Insert(oportunidadVenta);

					dto.OportunidadVentaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OportunidadVentaDTO dto)
		{
			try
			{
				OportunidadVenta oportunidadVenta = Mapper.Map<OportunidadVenta>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = oportunidadVentaRepository.Update(oportunidadVenta);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(OportunidadVentaDTO dto)
		{
			try
			{
				OportunidadVenta oportunidadVenta = Mapper.Map<OportunidadVenta>(dto);

				using (TransactionScope tx = new())
				{
					oportunidadVentaRepository.Delete(oportunidadVenta);

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
