using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class SexoBusiness : ISexoBusiness
	{
		private readonly ISexoRepository sexoRepository;

		public SexoBusiness(
			ISexoRepository sexoRepository
			)
		{
			this.sexoRepository = sexoRepository;
		}
		public SexoDTO GetById(int id)
		{
			try
			{
				Sexo sexo = sexoRepository.GetById(id);

				SexoDTO sexoDTO = Mapper.Map<SexoDTO>(sexo);

				return sexoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<SexoDTO> Search(SexoDTO dto)
		{
			try
			{
				List<Sexo> sexoList = sexoRepository.Search(Mapper.Map<Sexo>(dto));

				List<SexoDTO> sexoDTOList = Mapper.Map<List<SexoDTO>>(sexoList);

				return sexoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SexoDTO SingleSearch(SexoDTO dto)
		{
			try
			{
				Sexo sexo = sexoRepository.SingleSearch(Mapper.Map<Sexo>(dto));

				SexoDTO sexoDTO = Mapper.Map<SexoDTO>(sexo);

				return sexoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(SexoDTO dto)
		{
			try
			{
				Sexo sexo = Mapper.Map<Sexo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = sexoRepository.Insert(sexo);

					dto.SexoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(SexoDTO dto)
		{
			try
			{
				Sexo sexo = Mapper.Map<Sexo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = sexoRepository.Update(sexo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(SexoDTO dto)
		{
			try
			{
				Sexo sexo = Mapper.Map<Sexo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					sexoRepository.Delete(sexo);

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
