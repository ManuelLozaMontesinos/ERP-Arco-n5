using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class OrigenRegistroBusiness : IOrigenRegistroBusiness
	{
		private readonly IOrigenRegistroRepository origenRegistroRepository;

		public OrigenRegistroBusiness(
			IOrigenRegistroRepository origenRegistroRepository
			)
		{
			this.origenRegistroRepository = origenRegistroRepository;
		}
		public OrigenRegistroDTO GetById(int id)
			{
				try
				{
					OrigenRegistro origenRegistro = origenRegistroRepository.GetById(id);

					OrigenRegistroDTO origenRegistroDTO = Mapper.Map<OrigenRegistroDTO>(origenRegistro);

					return origenRegistroDTO;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public List<OrigenRegistroDTO> Search(OrigenRegistroDTO dto)
			{
				try
				{
					List<OrigenRegistro> origenRegistroList = origenRegistroRepository.Search(Mapper.Map<OrigenRegistro>(dto));

					List<OrigenRegistroDTO> origenRegistroDTOList = Mapper.Map<List<OrigenRegistroDTO>>(origenRegistroList);

					return origenRegistroDTOList;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public OrigenRegistroDTO SingleSearch(OrigenRegistroDTO dto)
			{
				try
				{
					OrigenRegistro origenRegistro = origenRegistroRepository.SingleSearch(Mapper.Map<OrigenRegistro>(dto));

					OrigenRegistroDTO origenRegistroDTO = Mapper.Map<OrigenRegistroDTO>(origenRegistro);

					return origenRegistroDTO;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public int Insert(OrigenRegistroDTO dto)
			{
				try
				{
					OrigenRegistro origenRegistro = Mapper.Map<OrigenRegistro>(dto);

					int newId = 0;
					using (TransactionScope tx = new TransactionScope())
					{
						newId = origenRegistroRepository.Insert(origenRegistro);

						dto.OrigenRegistroId = newId;
						tx.Complete();
					}

					return newId;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public int Update(OrigenRegistroDTO dto)
			{
				try
				{
					OrigenRegistro origenRegistro = Mapper.Map<OrigenRegistro>(dto);

					int id = 0;
					using (TransactionScope tx = new TransactionScope())
					{
						id = origenRegistroRepository.Update(origenRegistro);

						tx.Complete();
					}

					return id;
				}
				catch (Exception)
				{
					throw;
				}
			}
		public void Delete(OrigenRegistroDTO dto)
			{
				try
				{
					OrigenRegistro origenRegistro = Mapper.Map<OrigenRegistro>(dto);

					using (TransactionScope tx = new TransactionScope())
					{
						origenRegistroRepository.Delete(origenRegistro);

						tx.Complete();
					}
				}
				catch (Exception)
				{
					throw;
				}
			}
		public OrigenRegistroDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				OrigenRegistro origenRegistro = origenRegistroRepository.ObtenerPorCodigo(codigo);

				OrigenRegistroDTO origenRegistroDTO = Mapper.Map<OrigenRegistroDTO>(origenRegistro);

				return origenRegistroDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
