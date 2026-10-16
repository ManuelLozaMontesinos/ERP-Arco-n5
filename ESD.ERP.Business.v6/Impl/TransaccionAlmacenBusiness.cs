using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TransaccionAlmacenBusiness : ITransaccionAlmacenBusiness
	{
		private readonly ITransaccionAlmacenRepository transaccionAlmacenRepository;
		private readonly ITablaBusiness tablaBusiness;

		public TransaccionAlmacenBusiness(
			ITransaccionAlmacenRepository transaccionAlmacenRepository,
			ITablaBusiness tablaBusiness
			)
		{
			this.transaccionAlmacenRepository = transaccionAlmacenRepository;
			this.tablaBusiness = tablaBusiness;
		}
		public TransaccionAlmacenDTO GetById(int id)
		{
			try
			{
				TransaccionAlmacen transaccionAlmacen = transaccionAlmacenRepository.GetById(id);

				TransaccionAlmacenDTO transaccionAlmacenDTO = Mapper.Map<TransaccionAlmacenDTO>(transaccionAlmacen);

				return transaccionAlmacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TransaccionAlmacenDTO> Search(TransaccionAlmacenDTO dto)
		{
			try
			{
				List<TransaccionAlmacen> transaccionAlmacenList = transaccionAlmacenRepository.Search(Mapper.Map<TransaccionAlmacen>(dto));

				List<TransaccionAlmacenDTO> transaccionAlmacenDTOList = Mapper.Map<List<TransaccionAlmacenDTO>>(transaccionAlmacenList);

				return transaccionAlmacenDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TransaccionAlmacenDTO SingleSearch(TransaccionAlmacenDTO dto)
		{
			try
			{
				TransaccionAlmacen transaccionAlmacen = transaccionAlmacenRepository.SingleSearch(Mapper.Map<TransaccionAlmacen>(dto));

				TransaccionAlmacenDTO transaccionAlmacenDTO = Mapper.Map<TransaccionAlmacenDTO>(transaccionAlmacen);

				return transaccionAlmacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TransaccionAlmacenDTO dto)
		{
			try
			{
				TransaccionAlmacen transaccionAlmacen = Mapper.Map<TransaccionAlmacen>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
					{
						Tabla = TablaConstante.TRANSACCION_ALMACEN,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = dto.EmpresaId }
						},
						UsuarioAccionId = dto.UsuarioCreacionId
					};

					transaccionAlmacen.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = transaccionAlmacenRepository.Insert(transaccionAlmacen);

					dto.TransaccionAlmacenId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TransaccionAlmacenDTO dto)
		{
			try
			{
				TransaccionAlmacen transaccionAlmacen = Mapper.Map<TransaccionAlmacen>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = transaccionAlmacenRepository.Update(transaccionAlmacen);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TransaccionAlmacenDTO dto)
		{
			try
			{
				TransaccionAlmacen transaccionAlmacen = Mapper.Map<TransaccionAlmacen>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					transaccionAlmacenRepository.Delete(transaccionAlmacen);

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
