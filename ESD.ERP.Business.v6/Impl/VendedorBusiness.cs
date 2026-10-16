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
	public class VendedorBusiness : IVendedorBusiness
	{
		private readonly IVendedorRepository vendedorRepository;
		private readonly ITablaBusiness tablaBusiness;

		public VendedorBusiness(
			IVendedorRepository vendedorRepository,
			ITablaBusiness tablaBusiness
			)
		{
			this.vendedorRepository = vendedorRepository;
			this.tablaBusiness = tablaBusiness;
		}
		public VendedorDTO GetById(int id)
		{
			try
			{
				Vendedor vendedor = vendedorRepository.GetById(id);

				VendedorDTO vendedorDTO = Mapper.Map<VendedorDTO>(vendedor);

				return vendedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<VendedorDTO> Search(VendedorDTO dto)
		{
			try
			{
				List<Vendedor> vendedorList = vendedorRepository.Search(Mapper.Map<Vendedor>(dto));

				List<VendedorDTO> vendedorDTOList = Mapper.Map<List<VendedorDTO>>(vendedorList);

				return vendedorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public VendedorDTO SingleSearch(VendedorDTO dto)
		{
			try
			{
				Vendedor vendedor = vendedorRepository.SingleSearch(Mapper.Map<Vendedor>(dto));

				VendedorDTO vendedorDTO = Mapper.Map<VendedorDTO>(vendedor);

				return vendedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(VendedorDTO dto)
		{
			try
			{
				Vendedor vendedor = new Vendedor
				{
					Proveedor = new Proveedor() { EmpresaId = dto.ProveedorDTO.EmpresaId},
					ProveedorId = dto.ProveedorId
				};
				vendedor = vendedorRepository.SingleSearch(vendedor);
				if (vendedor != null)
					throw new Exception(SR.GetString("EX_vendedor_ya_registrado", vendedor.Proveedor.RazonSocial));

				vendedor = Mapper.Map<Vendedor>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
					{
						Tabla = TablaConstante.VENDEDOR,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = dto.ProveedorDTO.EmpresaId }
						},
						UsuarioAccionId = dto.UsuarioCreacionId
					};

					vendedor.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = vendedorRepository.Insert(vendedor);

					dto.VendedorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(VendedorDTO dto)
		{
			try
			{
				Vendedor vendedor = Mapper.Map<Vendedor>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = vendedorRepository.Update(vendedor);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(VendedorDTO dto)
		{
			try
			{
				Vendedor vendedor = Mapper.Map<Vendedor>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					vendedorRepository.Delete(vendedor);

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
