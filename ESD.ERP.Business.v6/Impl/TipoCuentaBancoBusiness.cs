using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoCuentaBancoBusiness : ITipoCuentaBancoBusiness
	{
		private readonly ITipoCuentaBancoRepository tipoCuentaBancoRepository;

		public TipoCuentaBancoBusiness(
			ITipoCuentaBancoRepository tipoCuentaBancoRepository
			)
		{
			this.tipoCuentaBancoRepository = tipoCuentaBancoRepository;
		}
		public TipoCuentaBancoDTO GetById(int id)
		{
			try
			{
				TipoCuentaBanco tipoCuentaBanco = tipoCuentaBancoRepository.GetById(id);

				TipoCuentaBancoDTO tipoCuentaBancoDTO = Mapper.Map<TipoCuentaBancoDTO>(tipoCuentaBanco);

				return tipoCuentaBancoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoCuentaBancoDTO> Search(TipoCuentaBancoDTO dto)
		{
			try
			{
				List<TipoCuentaBanco> tipoCuentaBancoList = tipoCuentaBancoRepository.Search(Mapper.Map<TipoCuentaBanco>(dto));

				List<TipoCuentaBancoDTO> tipoCuentaBancoDTOList = Mapper.Map<List<TipoCuentaBancoDTO>>(tipoCuentaBancoList);

				return tipoCuentaBancoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoCuentaBancoDTO SingleSearch(TipoCuentaBancoDTO dto)
		{
			try
			{
				TipoCuentaBanco tipoCuentaBanco = tipoCuentaBancoRepository.SingleSearch(Mapper.Map<TipoCuentaBanco>(dto));

				TipoCuentaBancoDTO tipoCuentaBancoDTO = Mapper.Map<TipoCuentaBancoDTO>(tipoCuentaBanco);

				return tipoCuentaBancoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoCuentaBancoDTO dto)
		{
			try
			{
				TipoCuentaBanco tipoCuentaBanco = Mapper.Map<TipoCuentaBanco>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoCuentaBancoRepository.Insert(tipoCuentaBanco);

					dto.TipoCuentaBancoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoCuentaBancoDTO dto)
		{
			try
			{
				TipoCuentaBanco tipoCuentaBanco = Mapper.Map<TipoCuentaBanco>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoCuentaBancoRepository.Update(tipoCuentaBanco);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoCuentaBancoDTO dto)
		{
			try
			{
				TipoCuentaBanco tipoCuentaBanco = Mapper.Map<TipoCuentaBanco>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoCuentaBancoRepository.Delete(tipoCuentaBanco);

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
