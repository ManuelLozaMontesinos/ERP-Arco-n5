using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ConvenioDobleTributacionBusiness : IConvenioDobleTributacionBusiness
	{
		private readonly IConvenioDobleTributacionRepository convenioDobleTributacionRepository;

		public ConvenioDobleTributacionBusiness(
			IConvenioDobleTributacionRepository convenioDobleTributacionRepository
			)
		{
			this.convenioDobleTributacionRepository = convenioDobleTributacionRepository;
		}
		public ConvenioDobleTributacionDTO GetById(int id)
		{
			try
			{
				ConvenioDobleTributacion convenioDobleTributacion = convenioDobleTributacionRepository.GetById(id);

				ConvenioDobleTributacionDTO convenioDobleTributacionDTO = Mapper.Map<ConvenioDobleTributacionDTO>(convenioDobleTributacion);

				return convenioDobleTributacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ConvenioDobleTributacionDTO> Search(ConvenioDobleTributacionDTO dto)
		{
			try
			{
				List<ConvenioDobleTributacion> convenioDobleTributacionList = convenioDobleTributacionRepository.Search(Mapper.Map<ConvenioDobleTributacion>(dto));

				List<ConvenioDobleTributacionDTO> convenioDobleTributacionDTOList = Mapper.Map<List<ConvenioDobleTributacionDTO>>(convenioDobleTributacionList);

				return convenioDobleTributacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ConvenioDobleTributacionDTO SingleSearch(ConvenioDobleTributacionDTO dto)
		{
			try
			{
				ConvenioDobleTributacion convenioDobleTributacion = convenioDobleTributacionRepository.SingleSearch(Mapper.Map<ConvenioDobleTributacion>(dto));

				ConvenioDobleTributacionDTO convenioDobleTributacionDTO = Mapper.Map<ConvenioDobleTributacionDTO>(convenioDobleTributacion);

				return convenioDobleTributacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ConvenioDobleTributacionDTO dto)
		{
			try
			{
				ConvenioDobleTributacion convenioDobleTributacion = Mapper.Map<ConvenioDobleTributacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = convenioDobleTributacionRepository.Insert(convenioDobleTributacion);

					dto.ConvenioDobleTributacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ConvenioDobleTributacionDTO dto)
		{
			try
			{
				ConvenioDobleTributacion convenioDobleTributacion = Mapper.Map<ConvenioDobleTributacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = convenioDobleTributacionRepository.Update(convenioDobleTributacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ConvenioDobleTributacionDTO dto)
		{
			try
			{
				ConvenioDobleTributacion convenioDobleTributacion = Mapper.Map<ConvenioDobleTributacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					convenioDobleTributacionRepository.Delete(convenioDobleTributacion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ConvenioDobleTributacionDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				ConvenioDobleTributacion convenioDobleTributacion = convenioDobleTributacionRepository.ObtenerPorCodigo(codigo);

				ConvenioDobleTributacionDTO convenioDobleTributacionDTO = Mapper.Map<ConvenioDobleTributacionDTO>(convenioDobleTributacion);

				return convenioDobleTributacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
