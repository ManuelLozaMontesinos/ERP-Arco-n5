using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MotivoTrasladoBusiness : IMotivoTrasladoBusiness
	{
		private readonly IMotivoTrasladoRepository motivoTrasladoRepository;

		public MotivoTrasladoBusiness(
			IMotivoTrasladoRepository motivoTrasladoRepository
			)
		{
			this.motivoTrasladoRepository = motivoTrasladoRepository;
		}
		public MotivoTrasladoDTO GetById(int id)
		{
			try
			{
				MotivoTraslado motivoTraslado = motivoTrasladoRepository.GetById(id);

				MotivoTrasladoDTO motivoTrasladoDTO = Mapper.Map<MotivoTrasladoDTO>(motivoTraslado);

				return motivoTrasladoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MotivoTrasladoDTO> Search(MotivoTrasladoDTO dto)
		{
			try
			{
				List<MotivoTraslado> motivoTrasladoList = motivoTrasladoRepository.Search(Mapper.Map<MotivoTraslado>(dto));

				List<MotivoTrasladoDTO> motivoTrasladoDTOList = Mapper.Map<List<MotivoTrasladoDTO>>(motivoTrasladoList);

				return motivoTrasladoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MotivoTrasladoDTO SingleSearch(MotivoTrasladoDTO dto)
		{
			try
			{
				MotivoTraslado motivoTraslado = motivoTrasladoRepository.SingleSearch(Mapper.Map<MotivoTraslado>(dto));

				MotivoTrasladoDTO motivoTrasladoDTO = Mapper.Map<MotivoTrasladoDTO>(motivoTraslado);

				return motivoTrasladoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MotivoTrasladoDTO dto)
		{
			try
			{
				MotivoTraslado motivoTraslado = Mapper.Map<MotivoTraslado>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = motivoTrasladoRepository.Insert(motivoTraslado);

					dto.MotivoTrasladoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MotivoTrasladoDTO dto)
		{
			try
			{
				MotivoTraslado motivoTraslado = Mapper.Map<MotivoTraslado>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = motivoTrasladoRepository.Update(motivoTraslado);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MotivoTrasladoDTO dto)
		{
			try
			{
				MotivoTraslado motivoTraslado = Mapper.Map<MotivoTraslado>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					motivoTrasladoRepository.Delete(motivoTraslado);

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
