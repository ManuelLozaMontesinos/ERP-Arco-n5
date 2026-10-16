using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class VendedorSerieBusiness : IVendedorSerieBusiness
	{
		private readonly IVendedorSerieRepository vendedorSerieRepository;

		public VendedorSerieBusiness(
			IVendedorSerieRepository vendedorSerieRepository
			)
		{
			this.vendedorSerieRepository = vendedorSerieRepository;
		}
		public VendedorSerieDTO GetById(int id)
		{
			try
			{
				VendedorSerie vendedorSerie = vendedorSerieRepository.GetById(id);

				VendedorSerieDTO vendedorSerieDTO = Mapper.Map<VendedorSerieDTO>(vendedorSerie);

				return vendedorSerieDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<VendedorSerieDTO> Search(VendedorSerieDTO dto)
		{
			try
			{
				List<VendedorSerie> vendedorSerieList = vendedorSerieRepository.Search(Mapper.Map<VendedorSerie>(dto));

				List<VendedorSerieDTO> vendedorSerieDTOList = Mapper.Map<List<VendedorSerieDTO>>(vendedorSerieList);

				return vendedorSerieDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public VendedorSerieDTO SingleSearch(VendedorSerieDTO dto)
		{
			try
			{
				VendedorSerie vendedorSerie = vendedorSerieRepository.SingleSearch(Mapper.Map<VendedorSerie>(dto));

				VendedorSerieDTO vendedorSerieDTO = Mapper.Map<VendedorSerieDTO>(vendedorSerie);

				return vendedorSerieDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(VendedorSerieDTO dto)
		{
			try
			{
				VendedorSerie vendedorSerie = Mapper.Map<VendedorSerie>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = vendedorSerieRepository.Insert(vendedorSerie);

					dto.VendedorSerieId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(VendedorSerieDTO dto)
		{
			try
			{
				VendedorSerie vendedorSerie = Mapper.Map<VendedorSerie>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = vendedorSerieRepository.Update(vendedorSerie);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(VendedorSerieDTO dto)
		{
			try
			{
				VendedorSerie vendedorSerie = Mapper.Map<VendedorSerie>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					vendedorSerieRepository.Delete(vendedorSerie);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarListaDocumentoComercialSerieId(VendedorSerieDTO dto)
      {
			try
			{
				VendedorSerie vendedorSerie = Mapper.Map<VendedorSerie>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					vendedorSerieRepository.ActualizarListaDocumentoComercialSerieId(vendedorSerie);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoDocumentoComercialDTO> ListarTipoDocumentoComercialPorVendedor(VendedorSerieDTO dto)
		{
			try
			{
				List<VendedorSerie> vendedorSerieList = vendedorSerieRepository.Search(Mapper.Map<VendedorSerie>(dto));

				List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = (from p in vendedorSerieList
								group p by p.DocumentoComercialSerie.TipoDocumentoComercial into g
								select new TipoDocumentoComercialDTO
								{
									TipoDocumentoComercialId = g.Key.TipoDocumentoComercialId,
									Nombre = g.Key.Nombre,
									Abreviatura = g.Key.Abreviatura
								}).ToList();

				return tipoDocumentoComercialDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<AlmacenDTO> ListarAlmacenPorVendedor(VendedorSerieDTO dto)
		{
			try
			{
				List<VendedorSerie> vendedorSerieList = vendedorSerieRepository.Search(Mapper.Map<VendedorSerie>(dto));

				List<AlmacenDTO> almacenDTOList = vendedorSerieList.GroupBy(
					x => new { x.DocumentoComercialSerie.AlmacenId, x.DocumentoComercialSerie.Almacen.Nombre })
					.Select(x => new AlmacenDTO
					{
						AlmacenId = x.Key.AlmacenId.Value,
						Nombre = x.Key.Nombre
					}).ToList();

				return almacenDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
