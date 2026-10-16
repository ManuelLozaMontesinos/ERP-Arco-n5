using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class BancoBusiness : IBancoBusiness
	{
		private readonly IBancoRepository bancoRepository;

		public BancoBusiness(
			IBancoRepository bancoRepository
			)
		{
			this.bancoRepository = bancoRepository;
		}
		public BancoDTO GetById(int id)
		{
			try
			{
				Banco banco = bancoRepository.GetById(id);

				BancoDTO bancoDTO = Mapper.Map<BancoDTO>(banco);

				return bancoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<BancoDTO> Search(BancoDTO dto)
		{
			try
			{
				List<Banco> bancoList = bancoRepository.Search(Mapper.Map<Banco>(dto));

				List<BancoDTO> bancoDTOList = Mapper.Map<List<BancoDTO>>(bancoList);

				return bancoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public BancoDTO SingleSearch(BancoDTO dto)
		{
			try
			{
				Banco banco = bancoRepository.SingleSearch(Mapper.Map<Banco>(dto));

				BancoDTO bancoDTO = Mapper.Map<BancoDTO>(banco);

				return bancoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(BancoDTO dto)
		{
			try
			{
				Banco banco = Mapper.Map<Banco>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = bancoRepository.Insert(banco);

					dto.BancoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(BancoDTO dto)
		{
			try
			{
				Banco banco = Mapper.Map<Banco>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = bancoRepository.Update(banco);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(BancoDTO dto)
		{
			try
			{
				Banco banco = Mapper.Map<Banco>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					bancoRepository.Delete(banco);

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
