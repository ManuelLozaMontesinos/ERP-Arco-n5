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
	public class RegimenPensionarioComisionBusiness : IRegimenPensionarioComisionBusiness
	{
		private readonly IRegimenPensionarioComisionRepository regimenPensionarioComisionRepository;

		public RegimenPensionarioComisionBusiness(
			IRegimenPensionarioComisionRepository regimenPensionarioComisionRepository
			)
		{
			this.regimenPensionarioComisionRepository = regimenPensionarioComisionRepository;
		}
		public RegimenPensionarioComisionDTO GetById(int id)
		{
			try
			{
				RegimenPensionarioComision regimenPensionarioComision = regimenPensionarioComisionRepository.GetById(id);

				RegimenPensionarioComisionDTO regimenPensionarioComisionDTO = Mapper.Map<RegimenPensionarioComisionDTO>(regimenPensionarioComision);

				return regimenPensionarioComisionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RegimenPensionarioComisionDTO> Search(RegimenPensionarioComisionDTO dto)
		{
			try
			{
				List<RegimenPensionarioComision> regimenPensionarioComisionList = regimenPensionarioComisionRepository.Search(Mapper.Map<RegimenPensionarioComision>(dto));

				List<RegimenPensionarioComisionDTO> regimenPensionarioComisionDTOList = Mapper.Map<List<RegimenPensionarioComisionDTO>>(regimenPensionarioComisionList);

				return regimenPensionarioComisionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RegimenPensionarioComisionDTO SingleSearch(RegimenPensionarioComisionDTO dto)
		{
			try
			{
				RegimenPensionarioComision regimenPensionarioComision = regimenPensionarioComisionRepository.SingleSearch(Mapper.Map<RegimenPensionarioComision>(dto));

				RegimenPensionarioComisionDTO regimenPensionarioComisionDTO = Mapper.Map<RegimenPensionarioComisionDTO>(regimenPensionarioComision);

				return regimenPensionarioComisionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RegimenPensionarioComisionDTO dto)
		{
			try
			{
				RegimenPensionarioComision regimenPensionarioComision = Mapper.Map<RegimenPensionarioComision>(dto);
				regimenPensionarioComision.FechaBajaNull = SiNoConstante.S;
				regimenPensionarioComision = regimenPensionarioComisionRepository.SingleSearch(regimenPensionarioComision);
				if (regimenPensionarioComision != null)
					throw new Exception(SR.GetString("EX_regimen_pensionario_comision_ya_registrado", regimenPensionarioComision.TipoAporteComision.Nombre));

				regimenPensionarioComision = Mapper.Map<RegimenPensionarioComision>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = regimenPensionarioComisionRepository.Insert(regimenPensionarioComision);

					dto.RegimenPensionarioComisionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RegimenPensionarioComisionDTO dto)
		{
			try
			{
				RegimenPensionarioComision regimenPensionarioComision = Mapper.Map<RegimenPensionarioComision>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = regimenPensionarioComisionRepository.Update(regimenPensionarioComision);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RegimenPensionarioComisionDTO dto)
		{
			try
			{
				RegimenPensionarioComision regimenPensionarioComision = Mapper.Map<RegimenPensionarioComision>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					regimenPensionarioComisionRepository.Delete(regimenPensionarioComision);

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
