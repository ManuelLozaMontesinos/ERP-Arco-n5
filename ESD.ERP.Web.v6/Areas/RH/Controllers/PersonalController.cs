using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.RH.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.RH.Controllers
{
   [Area("RH")]
   [Route("[controller]")]
   public class PersonalController : AuthorizedController
   {
      private readonly IPersonalProxy personalProxy;
      private readonly ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy;
      private readonly ISexoProxy sexoProxy;
      private readonly ITipoViaProxy tipoViaProxy;
      private readonly ITipoZonaProxy tipoZonaProxy;
      private readonly IDepartamentoProxy departamentoProxy;
      private readonly IProvinciaProxy provinciaProxy;
      private readonly IDistritoProxy distritoProxy;
      private readonly IPaisProxy paisProxy;
      private readonly ITipoTrabajadorProxy tipoTrabajadorProxy;
      private readonly ISituacionEducativaProxy situacionEducativaProxy;
      private readonly IRegimenSaludProxy regimenSaludProxy;
      private readonly IEPSProxy epsProxy;
      private readonly IRegimenPensionarioProxy regimenPensionarioProxy;
      private readonly IRegimenPensionarioEsquemaProxy regimenPensionarioEsquemaProxy;
      private readonly ISctrPensionProxy sctrPensionProxy;
      private readonly ISctrSaludProxy sctrSaludProxy;
      private readonly ITipoContratoProxy tipoContratoProxy;
      private readonly IPeriodicidadRemunerativaProxy periodicidadRemunerativaProxy;
      private readonly IEstadoCivilProxy estadoCivilProxy;
      private readonly ITipoPagoRemuneracionProxy tipoPagoRemuneracionProxy;
      private readonly ISituacionEspecialTrabajadorProxy situacionEspecialTrabajadorProxy;
      private readonly ICargoPersonalProxy cargoPersonalProxy;
      private readonly IConvenioDobleTributacionProxy convenioDobleTributacionProxy;
      private readonly IOrigenRegistroProxy origenRegistroProxy;
      private readonly ITipoNaturalezaJuridicaProxy tipoNaturalezaJuridicaProxy;
      private readonly IRegimenLaboralProxy regimenLaboralProxy;
      private readonly IProveedorProxy proveedorProxy;
      private readonly ICategoriaOcupacionalProxy categoriaOcupacionalProxy;
      private readonly ITipoModalidadFormativaProxy tipoModalidadFormativaProxy;
      private readonly IBancoProxy bancoProxy;
      private readonly ICentroCostoProxy centroCostoProxy;
      private readonly IParametroSistemaProxy parametroSistemaProxy;
      private readonly IPersonalRemuneracionProxy personalRemuneracionProxy;
      private readonly ITipoRemuneracionProxy tipoRemuneracionProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IPersonalPeriodoLaboralProxy personalPeriodoLaboralProxy;
      private readonly IPersonalDependienteProxy personalDependienteProxy;
      private readonly IVinculoFamiliarProxy vinculoFamiliarProxy;
      private readonly ITipoDocumentoVinculoFamiliarProxy tipoDocumentoVinculoFamiliarProxy;
      private readonly IMotivoBajaDependienteProxy motivoBajaDependienteProxy;
      private readonly ISituacionTrabajadorProxy situacionTrabajadorProxy;
      private readonly IOcupacionProxy ocupacionProxy;
      private readonly IPlanillaProxy planillaProxy;
      private readonly IPersonalPlanillaProxy personalPlanillaProxy;
      private readonly IMotivoFinPeriodoProxy motivoFinPeriodoProxy;
      private readonly ITipoProveedorProxy tipoProveedorProxy;

      public PersonalController(
         IPersonalProxy personalProxy,
         ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy,
         ISexoProxy sexoProxy,
         ITipoViaProxy tipoViaProxy,
         ITipoZonaProxy tipoZonaProxy,
         IDepartamentoProxy departamentoProxy,
         IProvinciaProxy provinciaProxy,
         IDistritoProxy distritoProxy,
         IPaisProxy paisProxy,
         ITipoTrabajadorProxy tipoTrabajadorProxy,
         ISituacionEducativaProxy situacionEducativaProxy,
         IRegimenSaludProxy regimenSaludProxy,
         IEPSProxy epsProxy,
         IRegimenPensionarioProxy regimenPensionarioProxy,
         IRegimenPensionarioEsquemaProxy regimenPensionarioEsquemaProxy,
         ISctrPensionProxy sctrPensionProxy,
         ISctrSaludProxy sctrSaludProxy,
         ITipoContratoProxy tipoContratoProxy,
         IPeriodicidadRemunerativaProxy periodicidadRemunerativaProxy,
         IEstadoCivilProxy estadoCivilProxy,
         ITipoPagoRemuneracionProxy tipoPagoRemuneracionProxy,
         ISituacionEspecialTrabajadorProxy situacionEspecialTrabajadorProxy,
         ICargoPersonalProxy cargoPersonalProxy,
         IConvenioDobleTributacionProxy convenioDobleTributacionProxy,
         IOrigenRegistroProxy origenRegistroProxy,
         ITipoNaturalezaJuridicaProxy tipoNaturalezaJuridicaProxy,
         IRegimenLaboralProxy regimenLaboralProxy,
         IProveedorProxy proveedorProxy,
         ICategoriaOcupacionalProxy categoriaOcupacionalProxy,
         ITipoModalidadFormativaProxy tipoModalidadFormativaProxy,
         IBancoProxy bancoProxy,
         ICentroCostoProxy centroCostoProxy,
         IParametroSistemaProxy parametroSistemaProxy,
         IPersonalRemuneracionProxy personalRemuneracionProxy,
         IMonedaProxy monedaProxy,
         ITipoRemuneracionProxy tipoRemuneracionProxy,
         IPersonalPeriodoLaboralProxy personalPeriodoLaboralProxy,
         IPersonalDependienteProxy personalDependienteProxy,
         IVinculoFamiliarProxy vinculoFamiliarProxy,
         ITipoDocumentoVinculoFamiliarProxy tipoDocumentoVinculoFamiliarProxy,
         IMotivoBajaDependienteProxy motivoBajaDependienteProxy,
         ISituacionTrabajadorProxy situacionTrabajadorProxy,
         IOcupacionProxy ocupacionProxy,
         IPlanillaProxy planillaProxy,
         IPersonalPlanillaProxy personalPlanillaProxy,
         IMotivoFinPeriodoProxy motivoFinPeriodoProxy,
         ITipoProveedorProxy tipoProveedorProxy
         )
      {
         this.personalProxy = personalProxy;
         this.tipoDocumentoIdentidadProxy = tipoDocumentoIdentidadProxy;
         this.sexoProxy = sexoProxy;
         this.tipoViaProxy = tipoViaProxy;
         this.tipoZonaProxy = tipoZonaProxy;
         this.departamentoProxy = departamentoProxy;
         this.provinciaProxy = provinciaProxy;
         this.distritoProxy = distritoProxy;
         this.paisProxy = paisProxy;
         this.tipoTrabajadorProxy = tipoTrabajadorProxy;
         this.situacionEducativaProxy = situacionEducativaProxy;
         this.regimenSaludProxy = regimenSaludProxy;
         this.epsProxy = epsProxy;
         this.regimenPensionarioProxy = regimenPensionarioProxy;
         this.regimenPensionarioEsquemaProxy = regimenPensionarioEsquemaProxy;
         this.sctrPensionProxy = sctrPensionProxy;
         this.sctrSaludProxy = sctrSaludProxy;
         this.tipoContratoProxy = tipoContratoProxy;
         this.periodicidadRemunerativaProxy = periodicidadRemunerativaProxy;
         this.estadoCivilProxy = estadoCivilProxy;
         this.tipoPagoRemuneracionProxy = tipoPagoRemuneracionProxy;
         this.situacionEspecialTrabajadorProxy = situacionEspecialTrabajadorProxy;
         this.cargoPersonalProxy = cargoPersonalProxy;
         this.convenioDobleTributacionProxy = convenioDobleTributacionProxy;
         this.origenRegistroProxy = origenRegistroProxy;
         this.tipoNaturalezaJuridicaProxy = tipoNaturalezaJuridicaProxy;
         this.regimenLaboralProxy = regimenLaboralProxy;
         this.proveedorProxy = proveedorProxy;
         this.categoriaOcupacionalProxy = categoriaOcupacionalProxy;
         this.tipoModalidadFormativaProxy = tipoModalidadFormativaProxy;
         this.bancoProxy = bancoProxy;
         this.centroCostoProxy = centroCostoProxy;
         this.parametroSistemaProxy = parametroSistemaProxy;
         this.personalRemuneracionProxy = personalRemuneracionProxy;
         this.monedaProxy = monedaProxy;
         this.tipoRemuneracionProxy = tipoRemuneracionProxy;
         this.personalPeriodoLaboralProxy = personalPeriodoLaboralProxy;
         this.personalDependienteProxy = personalDependienteProxy;
         this.vinculoFamiliarProxy = vinculoFamiliarProxy;
         this.tipoDocumentoVinculoFamiliarProxy = tipoDocumentoVinculoFamiliarProxy;
         this.motivoBajaDependienteProxy = motivoBajaDependienteProxy;
         this.situacionTrabajadorProxy = situacionTrabajadorProxy;
         this.ocupacionProxy = ocupacionProxy;
         this.planillaProxy = planillaProxy;
         this.personalPlanillaProxy = personalPlanillaProxy;
         this.motivoFinPeriodoProxy = motivoFinPeriodoProxy;
         this.tipoProveedorProxy = tipoProveedorProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         PersonalViewModel personalViewModel = new();

         return PartialView(personalViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PersonalViewModel model = JsonSerializer.Deserialize<PersonalViewModel>(jsonModel);

         model.PersonalDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PersonalDTO.OrderBy = "p.personal DESC";
         //model.PersonalDTO.PersonalDependienteId = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         try
         {
            List<PersonalDTO> personalDTOList = personalProxy.Search(model.PersonalDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = personalDTOList.Count,
               rows = from f in personalDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PersonalId,
                            f.Codigo,
                            TipoDocumentoIdentidad = f.ProveedorDTO.TipoDocumentoIdentidadDTO.Abreviatura,
                            f.ProveedorId,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.Direccion,
                            FechaIngreso = f.FechaIngreso.ToShortDateString(),
                            FechaAlta = f.FechaAlta == null ? string.Empty : f.FechaAlta.Value.ToShortDateString(),
                            FechaCese = f.FechaCese == null ? string.Empty : f.FechaCese.Value.ToShortDateString(),
                            CentroCosto = f.CentroCostoDTO == null ? string.Empty : f.CentroCostoDTO.Nombre,
                            f.Estado
                         }
                      }
            };

            Logger.InfoFinish();
            return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }
      }
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody]PersonalViewModel model)
      {
         Logger.InfoStart();

         PersonalViewModel personalViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(personalViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, personalViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody]PersonalViewModel model)
      {

         Logger.InfoStart();

         PersonalViewModel personalViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(personalViewModel, model.PersonalDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, personalViewModel);
      }
      private void cargarDataParaEditar(PersonalViewModel model, int id)
      {

         PersonalDTO personalDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {

            personalDTO = new PersonalDTO()
            {
               ProveedorDTO = new ProveedorDTO(),
               PaisEmisorDTO = new PaisEmisorDTO(),
               FechaNacimiento = DateTime.Today.AddYears(-18),
               FechaIngreso = DateTime.Today
            };

            PaisDTO paisDTO = paisProxy.ObtenerPorCodigo(new PaisDTO() { Codigo = PaisConstante.DEFAULT });
            personalDTO.PaisDTO = paisDTO;
            personalDTO.PaisId = paisDTO.Id;

            TipoDocumentoIdentidadDTO tipoDocumentoIdentidadDTO = tipoDocumentoIdentidadProxy.
               ObtenerPorCodigo(new TipoDocumentoIdentidadDTO() { Codigo = TipoDocumentoIdentidadConstante.DEFAULT });
            model.FormatoValidacion = tipoDocumentoIdentidadDTO.FormatoValidacion;
            model.TipoDocumentoIdentidad = tipoDocumentoIdentidadDTO.Codigo;

            personalDTO.ProveedorDTO.TipoDocumentoIdentidadId = tipoDocumentoIdentidadDTO.Id;

            DistritoDTO distritoDTO = new()
            {
               Codigo = DistritoConstante.DEFAULT,
               Estado = EstadoConstante.ACTIVO,
               ProvinciaDTO = new ProvinciaDTO()
               {
                  Codigo = ProvinciaConstante.DEFAULT,
                  DepartamentoDTO = new DepartamentoDTO() { Codigo = DepartamentoConstante.DEFAULT }
               }
            };

            personalDTO.ProveedorDTO.DistritoDTO = distritoProxy.SingleSearch(distritoDTO);
            personalDTO.ProveedorDTO.DistritoId = personalDTO.ProveedorDTO.DistritoDTO.Id;
            personalDTO.DistritoDTO = distritoProxy.SingleSearch(distritoDTO);
            personalDTO.DistritoId = personalDTO.ProveedorDTO.DistritoDTO.Id;

            TipoViaDTO tipoViaDTO = tipoViaProxy.ObtenerPorCodigo(new TipoViaDTO() { Codigo = TipoViaConstante.DEFAULT });
            personalDTO.ProveedorDTO.TipoViaId = tipoViaDTO.Id;
            personalDTO.TipoViaId = tipoViaDTO.Id;

            TipoZonaDTO tipoZonaDTO = tipoZonaProxy.ObtenerPorCodigo(new TipoZonaDTO() { Codigo = TipoZonaConstante.DEFAULT });
            personalDTO.ProveedorDTO.TipoZonaId = tipoZonaDTO.Id;
            personalDTO.TipoZonaId = tipoZonaDTO.Id;

            TipoTrabajadorDTO tipoTrabajadorDTO = tipoTrabajadorProxy.ObtenerPorCodigo(new TipoTrabajadorDTO()
               { Codigo = TipoTrabajadorConstante.DEFAULT });
            personalDTO.TipoTrabajadorId = tipoTrabajadorDTO.Id;

            RegimenLaboralDTO regimenLaboralDTO = regimenLaboralProxy.ObtenerPorCodigo(new RegimenLaboralDTO()
               { Codigo = RegimenLaboralConstante.DEFAULT });
            personalDTO.RegimenLaboralId = regimenLaboralDTO.Id;

            CargoPersonalDTO cargoPersonalDTO = cargoPersonalProxy
               .ObtenerPorCodigo(new CargoPersonalDTO() { Codigo = CargoPersonalConstante.DEFAULT });
            personalDTO.CargoPersonalId = cargoPersonalDTO.Id;

            OcupacionDTO ocupacionDTO = ocupacionProxy.ObtenerPorCodigo(new OcupacionDTO() { Codigo = OcupacionConstante.DEFAULT });
            personalDTO.OcupacionDTO = ocupacionDTO;
            personalDTO.OcupacionId = ocupacionDTO.Id;

            RegimenSaludDTO regimenSaludDTO = regimenSaludProxy.ObtenerPorCodigo(new RegimenSaludDTO(){ Codigo = RegimenSaludConstante.DEFAULT });
            personalDTO.RegimenSaludDTO = regimenSaludDTO;
            personalDTO.RegimenSaludId = regimenSaludDTO.Id;

            RegimenPensionarioDTO regimenPensionarioDTO = regimenPensionarioProxy.ObtenerPorCodigo(new RegimenPensionarioDTO()
               { Codigo = RegimenPensionarioConstante.DEFAULT });
            personalDTO.RegimenPensionarioDTO = regimenPensionarioDTO;
            personalDTO.RegimenPensionarioId = regimenPensionarioDTO.Id;

            TipoContratoDTO tipoContratoDTO = tipoContratoProxy.ObtenerPorCodigo(new TipoContratoDTO()
               { Codigo = TipoContratoConstante.DEFAULT });
            personalDTO.TipoContratoId = tipoContratoDTO.Id;

            SituacionEspecialTrabajadorDTO situacionEspecialTrabajadorDTO = situacionEspecialTrabajadorProxy
               .ObtenerPorCodigo(new SituacionEspecialTrabajadorDTO() { Codigo = SituacionEspecialTrabajadorConstante.DEFAULT });
            personalDTO.SituacionEspecialTrabajadorId = situacionEspecialTrabajadorDTO.Id;

            PeriodicidadRemunerativaDTO periodicidadRemunerativaDTO = periodicidadRemunerativaProxy
               .ObtenerPorCodigo(new PeriodicidadRemunerativaDTO() { Codigo = PeriodicidadRemunerativaConstante.DEFAULT });
            personalDTO.PeriodicidadRemunerativaId = periodicidadRemunerativaDTO.Id;

            EstadoCivilDTO estadoCivilDTO = estadoCivilProxy.ObtenerPorCodigo(new EstadoCivilDTO()
               { Codigo = EstadoCivilConstante.DEFAULT });
            personalDTO.EstadoCivilId = estadoCivilDTO.Id;

            TipoPagoRemuneracionDTO tipoPagoRemuneracionDTO = tipoPagoRemuneracionProxy.ObtenerPorCodigo(new TipoPagoRemuneracionDTO()
               { Codigo = TipoPagoRemuneracionConstante.DEFAULT });
            personalDTO.TipoPagoRemuneracionId = tipoPagoRemuneracionDTO.Id;
            personalDTO.TipoPagoRemuneracionDTO = tipoPagoRemuneracionDTO;

            ConvenioDobleTributacionDTO convenioDobleTributacionDTO = convenioDobleTributacionProxy
               .ObtenerPorCodigo(new ConvenioDobleTributacionDTO() { Codigo = ConvenioDobleTributacionConstante.DEFAULT });
            personalDTO.ConvenioDobleTributacionId = convenioDobleTributacionDTO.Id;

         }
         else
         {
            personalDTO = personalProxy.GetById(new PersonalDTO() { PersonalId = id });

            model.FormatoValidacion = personalDTO.ProveedorDTO.TipoDocumentoIdentidadDTO.FormatoValidacion;
            model.TipoDocumentoIdentidad = personalDTO.ProveedorDTO.TipoDocumentoIdentidadDTO.Codigo;
            model.CategoriaOcupacional = personalDTO.CategoriaOcupacionalDTO?.Codigo;

            model.EsSoloLectura = personalDTO.FechaCese.HasValue ? SiNoConstante.S : SiNoConstante.N;
         }

         model.RegimenSalud = personalDTO.RegimenSaludDTO.Codigo;
         model.RegimenPensionario = personalDTO.RegimenPensionarioDTO.Codigo;
         model.TipoPagoRemuneracion = personalDTO.TipoPagoRemuneracionDTO.Codigo;

         personalDTO.EsEventual = personalDTO.EsEventual == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         personalDTO.EsCentroAsistencial = personalDTO.EsCentroAsistencial == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         personalDTO.EsDiscapacitado = personalDTO.EsDiscapacitado == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         personalDTO.EsRegimenAtipico = personalDTO.EsRegimenAtipico == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         personalDTO.EsJornadaTrabajoMaxima = personalDTO.EsJornadaTrabajoMaxima == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         personalDTO.EsJornadaTrabajoNocturna = personalDTO.EsJornadaTrabajoNocturna == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         personalDTO.EsMiembroSindicalizado = personalDTO.EsMiembroSindicalizado == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         personalDTO.AportaSctr = personalDTO.AportaSctr == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         personalDTO.EsExonerado5ta = personalDTO.EsExonerado5ta == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         personalDTO.AsignacionFamiliar = personalDTO.AsignacionFamiliar == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;

         model.PersonalDTO = personalDTO;

         cargarListas(model);
         cargarUbigeo(model, model.PersonalDTO.ProveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
            model.PersonalDTO.ProveedorDTO.DistritoDTO.ProvinciaId);
         cargarUbigeo2(model, model.PersonalDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
            model.PersonalDTO.DistritoDTO.ProvinciaId);

         WebHelper.SelectedListItemSetValue(model.TipoDocumentoIdentidadList, personalDTO.ProveedorDTO.TipoDocumentoIdentidadId);
         WebHelper.SelectedListItemSetValue(model.SexoList, personalDTO.SexoId);
         WebHelper.SelectedListItemSetValue(model.TipoViaList, personalDTO.ProveedorDTO.TipoViaId);
         WebHelper.SelectedListItemSetValue(model.TipoZonaList, personalDTO.ProveedorDTO.TipoZonaId);
         WebHelper.SelectedListItemSetValue(model.DistritoList, personalDTO.ProveedorDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList, personalDTO.ProveedorDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList, personalDTO.ProveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);
         WebHelper.SelectedListItemSetValue(model.TipoViaList2, personalDTO.TipoViaId);
         WebHelper.SelectedListItemSetValue(model.TipoZonaList2, personalDTO.TipoZonaId);
         WebHelper.SelectedListItemSetValue(model.DistritoList2, personalDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList2, personalDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList2, personalDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);
         WebHelper.SelectedListItemSetValue(model.TipoTrabajadorList, personalDTO.TipoTrabajadorId);
         WebHelper.SelectedListItemSetValue(model.CategoriaOcupacionalList, personalDTO.CategoriaOcupacionalId);
         WebHelper.SelectedListItemSetValue(model.RegimenLaboralList, personalDTO.RegimenLaboralId);
         WebHelper.SelectedListItemSetValue(model.SituacionEducativaList, personalDTO.SituacionEducativaId);
         WebHelper.SelectedListItemSetValue(model.RegimenSaludList, personalDTO.RegimenSaludId);
         WebHelper.SelectedListItemSetValue(model.EPSList, personalDTO.EPSId);
         WebHelper.SelectedListItemSetValue(model.RegimenPensionarioList, personalDTO.RegimenPensionarioId);
         WebHelper.SelectedListItemSetValue(model.RegimenPensionarioEsquemaList, personalDTO.RegimenPensionarioEsquemaId);
         WebHelper.SelectedListItemSetValue(model.SctrPensionList, personalDTO.SctrPensionId);
         WebHelper.SelectedListItemSetValue(model.SctrSaludList, personalDTO.SctrSaludId);
         WebHelper.SelectedListItemSetValue(model.TipoContratoList, personalDTO.TipoContratoId);
         WebHelper.SelectedListItemSetValue(model.SituacionEspecialTrabajadorList, personalDTO.SituacionEspecialTrabajadorId);
         WebHelper.SelectedListItemSetValue(model.PeriodicidadRemunerativaList, personalDTO.PeriodicidadRemunerativaId);
         WebHelper.SelectedListItemSetValue(model.EstadoCivilList, personalDTO.EstadoCivilId);
         WebHelper.SelectedListItemSetValue(model.TipoPagoRemuneracionList, personalDTO.TipoPagoRemuneracionId);
         WebHelper.SelectedListItemSetValue(model.BancoList, personalDTO.BancoId);
         WebHelper.SelectedListItemSetValue(model.BancoCTSList, personalDTO.BancoCTSId);
         WebHelper.SelectedListItemSetValue(model.CentroCostoList, personalDTO.CentroCostoId);
         WebHelper.SelectedListItemSetValue(model.ConvenioDobleTributacionList, personalDTO.ConvenioDobleTributacionId);

         cargarParametrosConfiguracion(model);

         model.MenuItemList = new List<MenuItem>() {
            new MenuItem() { Key="proveedor", Content="Desde Proveedores"},
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { MenuItemType = MenuItemType.Exit }
         };

         model.MenuItemList2 = new List<MenuItem>() {
            new MenuItem() { MenuItemType = MenuItemType.New },
            new MenuItem() { MenuItemType = MenuItemType.Edit },
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { MenuItemType = MenuItemType.Cancel },
            new MenuItem() { Key="baja", Content="Cese", ButtonStyle="btn-danger"}
         };

         model.MenuItemList3 = new List<MenuItem>() {
            new MenuItem() { MenuItemType = MenuItemType.New },
            new MenuItem() { MenuItemType = MenuItemType.Edit },
            new MenuItem() { Key="baja", Content="Dar de Baja", ButtonStyle="btn-danger"}
         };
      }
      private void cargarListas(PersonalViewModel model)
      {
         cargarListasCompartidas(model);

         List<TipoTrabajadorDTO> tipoTrabajadorDTOList = tipoTrabajadorProxy.Search(new TipoTrabajadorDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoTrabajadorList = WebHelper.ToSelectListItem(tipoTrabajadorDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<CategoriaOcupacionalDTO> categoriaOcupacionalDTOList = categoriaOcupacionalProxy.Search(
            new CategoriaOcupacionalDTO() { Estado = EstadoConstante.ACTIVO });
         model.CategoriaOcupacionalList = WebHelper.ToSelectListItem(categoriaOcupacionalDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         List<RegimenLaboralDTO> regimenLaboralDTOList = regimenLaboralProxy.Search(
            new RegimenLaboralDTO() { Estado = EstadoConstante.ACTIVO });
         model.RegimenLaboralList = WebHelper.ToSelectListItem(regimenLaboralDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<SituacionEducativaDTO> situacionEducativaDTOList = situacionEducativaProxy.Search(
            new SituacionEducativaDTO() { Estado = EstadoConstante.ACTIVO });
         model.SituacionEducativaList = WebHelper.ToSelectListItem(situacionEducativaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         List<RegimenSaludDTO> regimenSaludDTOList = regimenSaludProxy.Search(
            new RegimenSaludDTO() { Estado = EstadoConstante.ACTIVO });
         model.RegimenSaludList = WebHelper.ToSelectListItem(regimenSaludDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<EPSDTO> epsDTOList = epsProxy.Search( new EPSDTO() { Estado = EstadoConstante.ACTIVO });
         model.EPSList = WebHelper.ToSelectListItem(epsDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         List<RegimenPensionarioDTO> regimenPensionarioDTOList = regimenPensionarioProxy.Search(
            new RegimenPensionarioDTO() { Estado = EstadoConstante.ACTIVO });
         model.RegimenPensionarioList = WebHelper.ToSelectListItem(regimenPensionarioDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<RegimenPensionarioEsquemaDTO> regimenPensionarioEsquemaDTOList = obtenerListaRegimenPensionarioEsquema(model.PersonalDTO.RegimenPensionarioId);
         model.RegimenPensionarioEsquemaList = WebHelper.ToSelectListItem(regimenPensionarioEsquemaDTOList, x => x.Id.ToString(), 
            x => x.TipoEsquemaPensionDTO.Nombre, SelectListFirstOptionType.Blank);

         List<SctrPensionDTO> sctrPensionDTOList = sctrPensionProxy.Search(new SctrPensionDTO() { Estado = EstadoConstante.ACTIVO });
         model.SctrPensionList = WebHelper.ToSelectListItem(sctrPensionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         List<SctrSaludDTO> sctrSaludDTOList = sctrSaludProxy.Search(new SctrSaludDTO() { Estado = EstadoConstante.ACTIVO });
         model.SctrSaludList = WebHelper.ToSelectListItem(sctrSaludDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         List<TipoModalidadFormativaDTO> tipoModalidadFormativaDTOList = tipoModalidadFormativaProxy.Search(
            new TipoModalidadFormativaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoModalidadFormativaList = WebHelper.ToSelectListItem(tipoModalidadFormativaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         List<TipoContratoDTO> tipoContratoDTOList = tipoContratoProxy.Search(
            new TipoContratoDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoContratoList = WebHelper.ToSelectListItem(tipoContratoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<SituacionEspecialTrabajadorDTO> situacionEspecialTrabajadorDTOList = situacionEspecialTrabajadorProxy.Search(
            new SituacionEspecialTrabajadorDTO() { Estado = EstadoConstante.ACTIVO });
         model.SituacionEspecialTrabajadorList = WebHelper.ToSelectListItem(situacionEspecialTrabajadorDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<PeriodicidadRemunerativaDTO> periodicidadRemunerativaDTOList = periodicidadRemunerativaProxy.Search(
            new PeriodicidadRemunerativaDTO() { Estado = EstadoConstante.ACTIVO });
         model.PeriodicidadRemunerativaList = WebHelper.ToSelectListItem(periodicidadRemunerativaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<EstadoCivilDTO> estadoCivilDTOList = estadoCivilProxy.Search(
            new EstadoCivilDTO() { Estado = EstadoConstante.ACTIVO });
         model.EstadoCivilList = WebHelper.ToSelectListItem(estadoCivilDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoPagoRemuneracionDTO> tipoPagoRemuneracionDTOList = tipoPagoRemuneracionProxy.Search(
            new TipoPagoRemuneracionDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoPagoRemuneracionList = WebHelper.ToSelectListItem(tipoPagoRemuneracionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<BancoDTO> bancoDTOList = bancoProxy.Search(
            new BancoDTO() { Estado = EstadoConstante.ACTIVO });
         model.BancoList = WebHelper.ToSelectListItem(bancoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         model.BancoCTSList = WebHelper.ToSelectListItem(bancoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         List<CentroCostoDTO> centroCostoDTOList = centroCostoProxy.Search(
            new CentroCostoDTO() { Estado = EstadoConstante.ACTIVO });
         model.CentroCostoList = WebHelper.ToSelectListItem(centroCostoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         List<ConvenioDobleTributacionDTO> convenioDobleTributacionDTOList = convenioDobleTributacionProxy.Search(
            new ConvenioDobleTributacionDTO() { Estado = EstadoConstante.ACTIVO });
         model.ConvenioDobleTributacionList = WebHelper.ToSelectListItem(convenioDobleTributacionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaString = string.Join(SeparadorConstante.PUNTO_COMA, monedaDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

         List<TipoRemuneracionDTO> tipoRemuneracionDTOList = tipoRemuneracionProxy.Search(new TipoRemuneracionDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoRemuneracionString = string.Join(SeparadorConstante.PUNTO_COMA, tipoRemuneracionDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

         List<PlanillaDTO> planillaDTOList = planillaProxy.Search(new PlanillaDTO() { EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
            Estado = EstadoConstante.ACTIVO });
         model.PlanillaString = string.Join(SeparadorConstante.PUNTO_COMA, planillaDTOList.Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

      }
      private void cargarListasCompartidas(PersonalViewModel model) {
         List<TipoDocumentoIdentidadDTO> tipoDocumentoIdentidadDTOList = tipoDocumentoIdentidadProxy.
            Search(new TipoDocumentoIdentidadDTO()
            {
               Estado = EstadoConstante.ACTIVO,
               NotTipoDocumentoIdentidadList = new List<string>() { TipoDocumentoIdentidadConstante.RUC }
            });
         model.TipoDocumentoIdentidadList = WebHelper.ToSelectListItem(tipoDocumentoIdentidadDTOList, x => x.Id.ToString(), x => x.Abreviatura,
            SelectListFirstOptionType.Select);

         List<SexoDTO> sexoDTOList = sexoProxy.Search(new SexoDTO() { Estado = EstadoConstante.ACTIVO });
         model.SexoList = WebHelper.ToSelectListItem(sexoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoViaDTO> tipoViaDTOList = tipoViaProxy.Search(new TipoViaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoViaList = WebHelper.ToSelectListItem(tipoViaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
         model.TipoViaList2 = WebHelper.ToSelectListItem(tipoViaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoZonaDTO> tipoZonaDTOList = tipoZonaProxy.Search(new TipoZonaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoZonaList = WebHelper.ToSelectListItem(tipoZonaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
         model.TipoZonaList2 = WebHelper.ToSelectListItem(tipoZonaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

      }
      private void cargarUbigeo(PersonalViewModel model, int departamentoId, int provinciaId)
      {

         List<DepartamentoDTO> departamentoDTOList = departamentoProxy.Search(new DepartamentoDTO() { Estado = EstadoConstante.ACTIVO });
         model.DepartamentoList = WebHelper.ToSelectListItem(departamentoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<ProvinciaDTO> provinciaDTOList = obtenerListaProvincia(departamentoId);
         model.ProvinciaList = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<DistritoDTO> distritoDTOList = obtenerListaDistrito(provinciaId);
         model.DistritoList = WebHelper.ToSelectListItem(distritoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarUbigeo2(PersonalViewModel model, int departamentoId, int provinciaId)
      {

         List<DepartamentoDTO> departamentoDTOList = departamentoProxy.Search(new DepartamentoDTO() { Estado = EstadoConstante.ACTIVO });
         model.DepartamentoList2 = WebHelper.ToSelectListItem(departamentoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<ProvinciaDTO> provinciaDTOList = obtenerListaProvincia(departamentoId);
         model.ProvinciaList2 = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<DistritoDTO> distritoDTOList = obtenerListaDistrito(provinciaId);
         model.DistritoList2 = WebHelper.ToSelectListItem(distritoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarParametrosConfiguracion(PersonalViewModel model) {

         List<ParametroSistemaDTO> parametroSistemaDTOList = parametroSistemaProxy.Search(new ParametroSistemaDTO() { 
            OrderBy = "orden_grupo asc",
            GrupoParametroSistemaDTO = new GrupoParametroSistemaDTO() { Codigo = GrupoParametroSistemaConstante.CAPITAL_HUMANO } 
         });

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.Where
            (x => x.Codigo == ParametroSistemaConstante.PERSONAL_PAIS_EMISOR_X_TIPO_DOC).FirstOrDefault();
         model.RestriccionPaisEmisorXTipoDocumento = parametroSistemaDTO == null ? string.Empty : parametroSistemaDTO.ValorCadena;

         parametroSistemaDTO = parametroSistemaDTOList.Where
            (x => x.Codigo == ParametroSistemaConstante.PERSONAL_SITUACION_EDUCATIVA_EXCLU_X_TIPO_TRABAJADOR).FirstOrDefault();
         model.RestriccionSituacionEducativaXTipoTrabajador = parametroSistemaDTO == null ? string.Empty : parametroSistemaDTO.ValorCadena;

         parametroSistemaDTO = parametroSistemaDTOList.Where
            (x => x.Codigo == ParametroSistemaConstante.PERSONAL_CATEG_OCUPACIONAL_EXCLU_X_TIPO_TRABAJADOR).FirstOrDefault();
         model.RestriccionSituacionEducativaXTipoTrabajador = parametroSistemaDTO == null ? string.Empty : parametroSistemaDTO.ValorCadena;

      }
      private List<ProvinciaDTO> obtenerListaProvincia(int departamentoId)
      {
         List<ProvinciaDTO> provinciaDTOList = provinciaProxy.Search(new ProvinciaDTO()
         {
            DepartamentoId = departamentoId,
            Estado = EstadoConstante.ACTIVO 
         });

         return provinciaDTOList;
      }
      private List<DistritoDTO> obtenerListaDistrito(int provinciaId)
      {
         List<DistritoDTO> distritoDTOList = distritoProxy.Search(new DistritoDTO()
         {
            ProvinciaId = provinciaId,
            Estado = EstadoConstante.ACTIVO
         });

         return distritoDTOList;
      }
      private List<RegimenPensionarioEsquemaDTO> obtenerListaRegimenPensionarioEsquema(int regimenPensionarioId)
      {
         List<RegimenPensionarioEsquemaDTO> regimenPensionarioEsquemaDTOList = regimenPensionarioEsquemaProxy.Search(
            new RegimenPensionarioEsquemaDTO() { RegimenPensionarioId = regimenPensionarioId, Estado = EstadoConstante.ACTIVO });

         return regimenPensionarioEsquemaDTOList;
      }
      [HttpGet("ListarProvincia")]
      public JsonResult ListarProvincia(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {

            List<ProvinciaDTO> provinciaDTOList = obtenerListaProvincia(id);
            resultResponse.Data = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse);
      }
      [HttpGet("ListarDistrito")]
      public JsonResult ListarDistrito(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            List<DistritoDTO> provinciaDTOList = obtenerListaDistrito(id);
            resultResponse.Data = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse);
      }
      [HttpGet("ListarRegimenPensionarioEsquema")]
      public JsonResult ListarRegimenPensionarioEsquema(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            List<RegimenPensionarioEsquemaDTO> regimenPensionarioEsquemaDTOList = obtenerListaRegimenPensionarioEsquema(id);

            RegimenPensionarioDTO regimenPensionarioDTO = regimenPensionarioProxy.GetById(new RegimenPensionarioDTO() { RegimenPensionarioId = id});

            resultResponse.Data = new
            {
               regimenPensionarioDTO.Codigo,
               List = WebHelper.ToSelectListItem(regimenPensionarioEsquemaDTOList, x => x.Id.ToString(),
                  x => x.TipoEsquemaPensionDTO.Nombre, SelectListFirstOptionType.Blank)
            };
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse);
      }
      [HttpGet("ObtenerCategoriaOcupacional")]
      public JsonResult ObtenerCategoriaOcupacional(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            CategoriaOcupacionalDTO categoriaOcupacionalDTO = categoriaOcupacionalProxy.
               GetById(new CategoriaOcupacionalDTO() { CategoriaOcupacionalId = id });

            resultResponse.Data = new
            {
               categoriaOcupacionalDTO.Codigo
            };
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpGet("ObtenerTipoPagoRemuneracion")]
      public JsonResult ObtenerTipoPagoRemuneracion(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            TipoPagoRemuneracionDTO tipoPagoRemuneracionDTO = tipoPagoRemuneracionProxy.
               GetById(new TipoPagoRemuneracionDTO() { TipoPagoRemuneracionId = id });

            resultResponse.Data = new
            {
               tipoPagoRemuneracionDTO.Codigo
            };
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpGet("ObtenerRegimenSalud")]
      public JsonResult ObtenerRegimenSalud(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            RegimenSaludDTO regimenSaludDTO = regimenSaludProxy.GetById(new RegimenSaludDTO() { RegimenSaludId = id });

            resultResponse.Data = new
            {
               regimenSaludDTO.Codigo
            };
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody]PersonalViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PersonalDTO personalDTO = cargarPersonalDTO(model);

            int newId = model.PersonalDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = personalProxy.Insert(personalDTO);
                  messageResourceKey = "RH_personal_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  personalProxy.Update(personalDTO);
                  messageResourceKey = "RH_personal_actualizado";
                  break;
            }

            personalDTO = personalProxy.GetById(new PersonalDTO() { PersonalId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, personalDTO.ProveedorDTO.RazonSocial);
            resultResponse.Data = new
            {
               personalDTO.Id,
               personalDTO.ProveedorDTO.ProveedorId,
               personalDTO.Codigo
            };
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      private PersonalDTO cargarPersonalDTO(PersonalViewModel model)
      {

         PersonalDTO personalDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               personalDTO = model.PersonalDTO;

               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

               personalDTO.EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId };
               personalDTO.EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId;

               if(personalDTO.ProveedorId == 0)
               {
                  personalDTO.ProveedorDTO.EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId;

                  TipoNaturalezaJuridicaDTO tipoNaturalezaJuridicaDTO = tipoNaturalezaJuridicaProxy
                     .ObtenerPorCodigo(new TipoNaturalezaJuridicaDTO() { Codigo = TipoNaturalezaJuridicaConstante.NATURAL });
                  personalDTO.ProveedorDTO.TipoNaturalezaJuridicaId = tipoNaturalezaJuridicaDTO.Id;

                  TipoProveedorDTO tipoProveedorDTO = tipoProveedorProxy.ObtenerPorCodigo(new TipoProveedorDTO() { Codigo = TipoProveedorConstante.CLIENTE });
                  personalDTO.ProveedorDTO.TipoProveedorId = tipoProveedorDTO.Id;

                  OrigenRegistroDTO origenRegistroDTO = origenRegistroProxy
                     .ObtenerPorCodigo(new OrigenRegistroDTO() { Codigo = OrigenRegistroConstante.USUARIO });
                  personalDTO.ProveedorDTO.OrigenRegistroId = origenRegistroDTO.Id;
               }
               else
               {
                  ProveedorDTO proveedorDTO = proveedorProxy.GetById(new ProveedorDTO() { ProveedorId = model.PersonalDTO.ProveedorId });

                  proveedorDTO.ApellidoPaterno = model.PersonalDTO.ProveedorDTO.ApellidoPaterno;
                  proveedorDTO.ApellidoMaterno = model.PersonalDTO.ProveedorDTO.ApellidoMaterno;
                  proveedorDTO.Nombres = model.PersonalDTO.ProveedorDTO.Nombres;
                  proveedorDTO.TipoDocumentoIdentidadId = model.PersonalDTO.ProveedorDTO.TipoDocumentoIdentidadId;
                  proveedorDTO.NumeroDocumentoIdentidad = model.PersonalDTO.ProveedorDTO.NumeroDocumentoIdentidad;
                  proveedorDTO.Telefono = model.PersonalDTO.ProveedorDTO.Telefono;
                  proveedorDTO.Email = model.PersonalDTO.ProveedorDTO.Email;
                  proveedorDTO.TipoViaId = model.PersonalDTO.ProveedorDTO.TipoViaId;
                  proveedorDTO.NombreVia = model.PersonalDTO.ProveedorDTO.NombreVia;
                  proveedorDTO.NumeroVia = model.PersonalDTO.ProveedorDTO.NumeroVia;
                  proveedorDTO.Interior = model.PersonalDTO.ProveedorDTO.Interior;
                  proveedorDTO.Departamento = model.PersonalDTO.ProveedorDTO.Departamento;
                  proveedorDTO.Lote = model.PersonalDTO.ProveedorDTO.Lote;
                  proveedorDTO.Manzana = model.PersonalDTO.ProveedorDTO.Manzana;
                  proveedorDTO.Kilometro = model.PersonalDTO.ProveedorDTO.Kilometro;
                  proveedorDTO.Block = model.PersonalDTO.ProveedorDTO.Block;
                  proveedorDTO.Etapa = model.PersonalDTO.ProveedorDTO.Etapa;
                  proveedorDTO.TipoZonaId = model.PersonalDTO.ProveedorDTO.TipoZonaId;
                  proveedorDTO.NombreZona = model.PersonalDTO.ProveedorDTO.NombreZona;
                  proveedorDTO.Referencia = model.PersonalDTO.ProveedorDTO.Referencia;
                  proveedorDTO.DistritoId = model.PersonalDTO.ProveedorDTO.DistritoId;

                  model.PersonalDTO.ProveedorDTO = proveedorDTO;
               }

               SituacionTrabajadorDTO situacionTrabajadorDTO = situacionTrabajadorProxy
                  .ObtenerPorCodigo(new SituacionTrabajadorDTO() { Codigo = SituacionTrabajadorConstante.ACTIVO });
               personalDTO.SituacionTrabajadorId = situacionTrabajadorDTO.Id;
            }
            else {
               personalDTO = personalProxy.GetById(new PersonalDTO() { PersonalId = model.PersonalDTO.Id });
               ProveedorDTO proveedorDTO = proveedorProxy.GetById(new ProveedorDTO() { ProveedorId = model.PersonalDTO.ProveedorId });

               model.PersonalDTO.ProveedorId = proveedorDTO.Id;

               proveedorDTO.ApellidoPaterno = model.PersonalDTO.ProveedorDTO.ApellidoPaterno;
               proveedorDTO.ApellidoMaterno = model.PersonalDTO.ProveedorDTO.ApellidoMaterno;
               proveedorDTO.Nombres = model.PersonalDTO.ProveedorDTO.Nombres;
               proveedorDTO.TipoDocumentoIdentidadId = model.PersonalDTO.ProveedorDTO.TipoDocumentoIdentidadId;
               proveedorDTO.NumeroDocumentoIdentidad = model.PersonalDTO.ProveedorDTO.NumeroDocumentoIdentidad;
               proveedorDTO.Telefono = model.PersonalDTO.ProveedorDTO.Telefono;
               proveedorDTO.Email = model.PersonalDTO.ProveedorDTO.Email;
               proveedorDTO.TipoViaId = model.PersonalDTO.ProveedorDTO.TipoViaId;
               proveedorDTO.NombreVia = model.PersonalDTO.ProveedorDTO.NombreVia;
               proveedorDTO.NumeroVia = model.PersonalDTO.ProveedorDTO.NumeroVia;
               proveedorDTO.Interior = model.PersonalDTO.ProveedorDTO.Interior;
               proveedorDTO.Departamento = model.PersonalDTO.ProveedorDTO.Departamento;
               proveedorDTO.Lote = model.PersonalDTO.ProveedorDTO.Lote;
               proveedorDTO.Manzana = model.PersonalDTO.ProveedorDTO.Manzana;
               proveedorDTO.Kilometro = model.PersonalDTO.ProveedorDTO.Kilometro;
               proveedorDTO.Block = model.PersonalDTO.ProveedorDTO.Block;
               proveedorDTO.Etapa = model.PersonalDTO.ProveedorDTO.Etapa;
               proveedorDTO.TipoZonaId = model.PersonalDTO.ProveedorDTO.TipoZonaId;
               proveedorDTO.NombreZona = model.PersonalDTO.ProveedorDTO.NombreZona;
               proveedorDTO.Referencia = model.PersonalDTO.ProveedorDTO.Referencia;
               proveedorDTO.DistritoId = model.PersonalDTO.ProveedorDTO.DistritoId;

               model.PersonalDTO.ProveedorDTO = proveedorDTO;
               personalDTO = model.PersonalDTO;
            }

            personalDTO.ProveedorDTO.RazonSocial = string.Concat(model.PersonalDTO.ProveedorDTO.ApellidoPaterno, SeparadorConstante.ESPACIO,
               (!string.IsNullOrEmpty(model.PersonalDTO.ProveedorDTO.ApellidoMaterno) ? string.Concat(model.PersonalDTO.ProveedorDTO.ApellidoMaterno,
               SeparadorConstante.ESPACIO) : string.Empty),
               model.PersonalDTO.ProveedorDTO.Nombres);

            personalDTO.ProveedorDTO.Abreviatura = string.Empty;
            personalDTO.ProveedorDTO.Web = string.Empty;

            if (string.IsNullOrEmpty(personalDTO.ProveedorDTO.NumeroRuc))
               personalDTO.ProveedorDTO.NumeroRuc = string.Empty;

            if (string.IsNullOrEmpty(personalDTO.ProveedorDTO.NumeroCCI))
               personalDTO.ProveedorDTO.NumeroCCI = string.Empty;

            if (string.IsNullOrEmpty(personalDTO.ProveedorDTO.CodigoExterno))
               personalDTO.ProveedorDTO.CodigoExterno = string.Empty;
            if (string.IsNullOrEmpty(personalDTO.ProveedorDTO.EsLocal))
               personalDTO.ProveedorDTO.EsLocal = SiNoConstante.S;
            if (string.IsNullOrEmpty(personalDTO.ProveedorDTO.Contacto))
               personalDTO.ProveedorDTO.Contacto = string.Empty;
            if (string.IsNullOrEmpty(personalDTO.ProveedorDTO.Email))
               personalDTO.ProveedorDTO.Email = string.Empty;
            if (string.IsNullOrEmpty(personalDTO.ProveedorDTO.EsProspecto))
               personalDTO.ProveedorDTO.EsProspecto = SiNoConstante.S;
            if (string.IsNullOrEmpty(personalDTO.ProveedorDTO.NumeroDocumentoIdentidad2))
               personalDTO.ProveedorDTO.NumeroDocumentoIdentidad2 = string.Empty;
            if (string.IsNullOrEmpty(personalDTO.ProveedorDTO.Cargo))
               personalDTO.ProveedorDTO.Cargo = string.Empty;

            if (string.IsNullOrEmpty(personalDTO.NumeroFotocheck))
               personalDTO.NumeroFotocheck = string.Empty;

            if (string.IsNullOrEmpty(personalDTO.Domiciliado))
               personalDTO.Domiciliado = SiNoConstante.S;

            if (string.IsNullOrEmpty(personalDTO.NumeroCuspp))
               personalDTO.NumeroCuspp = string.Empty;

            if (string.IsNullOrEmpty(personalDTO.EsDiscapacitado))
               personalDTO.EsDiscapacitado = SiNoConstante.N;

            if (string.IsNullOrEmpty(personalDTO.NumeroCuentaAbono))
               personalDTO.NumeroCuentaAbono = string.Empty;

            if (string.IsNullOrEmpty(personalDTO.NumeroCuentaCTS))
               personalDTO.NumeroCuentaCTS = string.Empty;

            if (string.IsNullOrEmpty(personalDTO.EsRegimenAtipico))
               personalDTO.EsRegimenAtipico = SiNoConstante.N;

            if (string.IsNullOrEmpty(personalDTO.EsJornadaTrabajoMaxima))
               personalDTO.EsJornadaTrabajoMaxima = SiNoConstante.N;

            if (string.IsNullOrEmpty(personalDTO.EsJornadaTrabajoNocturna))
               personalDTO.EsJornadaTrabajoNocturna = SiNoConstante.N;

            if (string.IsNullOrEmpty(personalDTO.EsMiembroSindicalizado))
               personalDTO.EsMiembroSindicalizado = SiNoConstante.N;

            if (string.IsNullOrEmpty(personalDTO.EsExonerado5ta))
               personalDTO.EsExonerado5ta = SiNoConstante.N;

            if (string.IsNullOrEmpty(personalDTO.AsignacionFamiliar))
               personalDTO.AsignacionFamiliar = SiNoConstante.N;

            personalDTO.Estado = EstadoConstante.ACTIVO;
            personalDTO.ProveedorDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(personalDTO.ProveedorDTO);
            AuditDTO(personalDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return personalDTO;
      }
      [HttpPost("ListarPersonalRemuneracion")]
      public ActionResult ListarPersonalRemuneracion(string jsonModel)
      {

         Logger.InfoStart();

         PersonalViewModel model = JsonSerializer.Deserialize<PersonalViewModel>(jsonModel);

         model.PersonalRemuneracionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PersonalRemuneracionDTO.OrderBy = "pr.fecha_alta desc";

         try
         {
            List<PersonalRemuneracionDTO> personalRemuneracionDTOList =
               personalRemuneracionProxy.Search(model.PersonalRemuneracionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = personalRemuneracionDTOList.Count,
               rows = from f in personalRemuneracionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PersonalRemuneracionId,
                            f.PersonalId,
                            f.MonedaId,
                            f.TipoRemuneracionId,
                            f.Monto,
                            FechaAlta = f.FechaAlta.ToShortDateString(),
                            FechaBaja = f.FechaBaja == null ? string.Empty : f.FechaBaja.Value.ToShortDateString(),
                            f.Estado,
                            DTOState = DTOState.Unchanged
                         }
                      }
            };

            Logger.InfoFinish();
            return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }
      }
      [HttpPost("GrabarPersonalRemuneracion")]
      public ActionResult GrabarPersonalRemuneracion([FromBody] PersonalViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PersonalRemuneracionDTO personalRemuneracionDTO = cargarPersonalRemuneracionDTO(model);

            int newId = model.PersonalRemuneracionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (personalRemuneracionDTO.DTOState)
            {
               case DTOState.Added:
                  newId = personalRemuneracionProxy.Insert(personalRemuneracionDTO);
                  messageResourceKey = "RH_personal_remuneracion_agregada";
                  break;

               case DTOState.Modified:
                  personalRemuneracionProxy.Update(personalRemuneracionDTO);
                  messageResourceKey = "RH_personal_remuneracion_actualizada";
                  break;
            }

            personalRemuneracionDTO = personalRemuneracionProxy.
               GetById(new PersonalRemuneracionDTO() { PersonalRemuneracionId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      private PersonalRemuneracionDTO cargarPersonalRemuneracionDTO(PersonalViewModel model)
      {

         PersonalRemuneracionDTO personalRemuneracionDTO;
         try
         {

            personalRemuneracionDTO = model.PersonalRemuneracionDTO;

            personalRemuneracionDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(personalRemuneracionDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return personalRemuneracionDTO;
      }
      [HttpPost("ListarPeriodoLaboral")]
      public ActionResult ListarPeriodoLaboral(string jsonModel)
      {

         Logger.InfoStart();

         PersonalViewModel model = JsonSerializer.Deserialize<PersonalViewModel>(jsonModel);

         model.PersonalPeriodoLaboralDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PersonalPeriodoLaboralDTO.OrderBy = "ppl.fecha_ing desc";

         try
         {
            List<PersonalPeriodoLaboralDTO> personalPeriodoLaboralDTOList =
               personalPeriodoLaboralProxy.Search(model.PersonalPeriodoLaboralDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = personalPeriodoLaboralDTOList.Count,
               rows = from f in personalPeriodoLaboralDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PersonalPeriodoLaboralId,
                            f.PersonalId,
                            FechaIngreso = f.FechaIngreso.ToShortDateString(),
                            FechaAlta = f.FechaAlta == null ? string.Empty : f.FechaAlta.Value.ToShortDateString(),
                            FechaCese = f.FechaCese == null ? string.Empty : f.FechaCese.Value.ToShortDateString(),
                            MotivoFinPeriodo = f.MotivoFinPeriodoDTO == null ? string.Empty : f.MotivoFinPeriodoDTO.Nombre,
                            f.Estado,
                            DTOState = DTOState.Unchanged
                         }
                      }
            };

            Logger.InfoFinish();
            return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }
      }
      [HttpPost("GrabarPersonalPeriodoLaboral")]
      public ActionResult GrabarPersonalPeriodoLaboral([FromBody] PersonalViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PersonalPeriodoLaboralDTO personalPeriodoLaboralDTO = cargarPersonalPeriodoLaboralDTO(model);

            int newId = model.PersonalPeriodoLaboralDTO.Id;
            string messageResourceKey = string.Empty;
            switch (personalPeriodoLaboralDTO.DTOState)
            {
               case DTOState.Added:
                  newId = personalPeriodoLaboralProxy.Insert(personalPeriodoLaboralDTO);
                  messageResourceKey = "RH_personal_periodo_laboral_agregado";
                  break;

               case DTOState.Modified:
                  personalPeriodoLaboralProxy.Update(personalPeriodoLaboralDTO);
                  messageResourceKey = "RH_personal_periodo_laboral_actualizado";
                  break;
            }

            personalPeriodoLaboralDTO = personalPeriodoLaboralProxy.
               GetById(new PersonalPeriodoLaboralDTO() { PersonalPeriodoLaboralId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      private PersonalPeriodoLaboralDTO cargarPersonalPeriodoLaboralDTO(PersonalViewModel model)
      {

         PersonalPeriodoLaboralDTO personalPeriodoLaboralDTO;
         try
         {

            personalPeriodoLaboralDTO = model.PersonalPeriodoLaboralDTO;

            personalPeriodoLaboralDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(personalPeriodoLaboralDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return personalPeriodoLaboralDTO;
      }
      [HttpPost("ListarPersonalDependiente")]
      public ActionResult ListarPersonalDependiente(string jsonModel)
      {

         Logger.InfoStart();

         PersonalViewModel model = JsonSerializer.Deserialize<PersonalViewModel>(jsonModel);

         model.PersonalDependienteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PersonalDependienteDTO.OrderBy = "pd.nombres";

         try
         {
            List<PersonalDependienteDTO> personalDependienteDTOList =
               personalDependienteProxy.Search(model.PersonalDependienteDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = personalDependienteDTOList.Count,
               rows = from f in personalDependienteDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PersonalDependienteId,
                            f.PersonalId,
                            TipoDocumentoIdentidad = f.TipoDocumentoIdentidadDTO.Abreviatura,
                            f.NumeroDocumentoIdentidad,
                            f.Nombres,
                            f.ApellidoPaterno,
                            f.ApellidoMaterno,
                            FechaAlta = f.FechaAlta == null ? string.Empty : f.FechaAlta.Value.ToShortDateString(),
                            FechaBaja = f.FechaBaja == null ? string.Empty : f.FechaBaja.Value.ToShortDateString(),
                            MotivoBajaDependiente = f.MotivoBajaDependienteDTO == null ? string.Empty : f.MotivoBajaDependienteDTO.Nombre,
                            f.Estado,
                            DTOState = DTOState.Unchanged
                         }
                      }
            };

            Logger.InfoFinish();
            return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }
      }
      [HttpPost("CrearPersonalDependiente")]
      public ActionResult CrearPersonalDependiente([FromBody] PersonalViewModel model)
      {
         PersonalViewModel personalViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarPersonalDependienteParaEditar(personalViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView("DependienteEditPopup", personalViewModel);
      }
      [HttpPost("EditarPersonalDependiente")]
      public ActionResult EditarPersonalDependiente([FromBody] PersonalViewModel model)
      {
         PersonalViewModel personalViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarPersonalDependienteParaEditar(personalViewModel, model.PersonalDependienteDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView("DependienteEditPopup", personalViewModel);
      }
      private void cargarPersonalDependienteParaEditar(PersonalViewModel model, int id)
      {

         PersonalDependienteDTO personalDependienteDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            personalDependienteDTO = new PersonalDependienteDTO
            {
               PaisEmisorDTO = new PaisEmisorDTO(),
               FechaNacimiento = DateTime.Today,
               Estado = EstadoConstante.ACTIVO
            };

            DistritoDTO distritoDTO = new()
            {
               Codigo = DistritoConstante.DEFAULT,
               Estado = EstadoConstante.ACTIVO,
               ProvinciaDTO = new ProvinciaDTO()
               {
                  Codigo = ProvinciaConstante.DEFAULT,
                  DepartamentoDTO = new DepartamentoDTO() { Codigo = DepartamentoConstante.DEFAULT }
               }
            };

            personalDependienteDTO.DistritoDTO = distritoProxy.SingleSearch(distritoDTO);
            personalDependienteDTO.DistritoId = personalDependienteDTO.DistritoDTO.Id;

            TipoViaDTO tipoViaDTO = tipoViaProxy.ObtenerPorCodigo(new TipoViaDTO() { Codigo = TipoViaConstante.DEFAULT });
            personalDependienteDTO.TipoViaId = tipoViaDTO.Id;

            TipoZonaDTO tipoZonaDTO = tipoZonaProxy.ObtenerPorCodigo(new TipoZonaDTO() { Codigo = TipoZonaConstante.DEFAULT });
            personalDependienteDTO.TipoZonaId = tipoZonaDTO.Id;

         }
         else
         {
            personalDependienteDTO = personalDependienteProxy.GetById(new PersonalDependienteDTO() { PersonalDependienteId = id });

            model.FormatoValidacion = personalDependienteDTO.TipoDocumentoIdentidadDTO.FormatoValidacion;
            model.TipoDocumentoIdentidad = personalDependienteDTO.TipoDocumentoIdentidadDTO.Codigo;
         }

         model.PersonalDependienteDTO = personalDependienteDTO;

         cargarListasPersonalDependiente(model);
         cargarUbigeo(model, model.PersonalDependienteDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
            model.PersonalDependienteDTO.DistritoDTO.ProvinciaId);

         WebHelper.SelectedListItemSetValue(model.TipoDocumentoIdentidadList, personalDependienteDTO.TipoDocumentoIdentidadId);
         WebHelper.SelectedListItemSetValue(model.SexoList, personalDependienteDTO.SexoId);
         WebHelper.SelectedListItemSetValue(model.VinculoFamiliarList, personalDependienteDTO.VinculoFamiliarId);
         WebHelper.SelectedListItemSetValue(model.TipoDocumentoVinculoFamiliarList, personalDependienteDTO.TipoDocumentoVinculoFamiliarId);
         WebHelper.SelectedListItemSetValue(model.MotivoBajaDependienteList, personalDependienteDTO.MotivoBajaDependienteId);
         WebHelper.SelectedListItemSetValue(model.TipoViaList, personalDependienteDTO.TipoViaId);
         WebHelper.SelectedListItemSetValue(model.TipoZonaList, personalDependienteDTO.TipoZonaId);
         WebHelper.SelectedListItemSetValue(model.DistritoList, personalDependienteDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList, personalDependienteDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList, personalDependienteDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);

         cargarParametrosConfiguracion(model);
      }
      private void cargarListasPersonalDependiente(PersonalViewModel model)
      {
         cargarListasCompartidas(model);

         List<VinculoFamiliarDTO> vinculoFamiliarDTOList = vinculoFamiliarProxy.Search(new VinculoFamiliarDTO() { Estado = EstadoConstante.ACTIVO });
         model.VinculoFamiliarList = WebHelper.ToSelectListItem(vinculoFamiliarDTOList, x => x.Id.ToString(), x => x.Abreviatura,
            SelectListFirstOptionType.Select);

         List<TipoDocumentoVinculoFamiliarDTO> tipoDocumentoVinculoFamiliarDTOList = tipoDocumentoVinculoFamiliarProxy
            .Search(new TipoDocumentoVinculoFamiliarDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoDocumentoVinculoFamiliarList = WebHelper.ToSelectListItem(tipoDocumentoVinculoFamiliarDTOList, x => x.Id.ToString(), x => x.Abreviatura,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("GrabarPersonalDependiente")]
      public ActionResult GrabarPersonalDependiente([FromBody] PersonalViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PersonalDependienteDTO personalDependienteDTO = cargarPersonalDependienteDTO(model);

            int newId = model.PersonalDependienteDTO.Id;
            string messageResourceKey = string.Empty;
            switch (personalDependienteDTO.DTOState)
            {
               case DTOState.Added:
                  newId = personalDependienteProxy.Insert(personalDependienteDTO);
                  messageResourceKey = "RH_personal_dependiente_agregado";
                  break;

               case DTOState.Modified:
                  personalDependienteProxy.Update(personalDependienteDTO);
                  messageResourceKey = "RH_personal_dependiente_actualizado";
                  break;
            }

            resultResponse.Message = SR.GetString(messageResourceKey, personalDependienteDTO.Nombres);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      private PersonalDependienteDTO cargarPersonalDependienteDTO(PersonalViewModel model)
      {

         PersonalDependienteDTO personalDependienteDTO;
         try
         {

            personalDependienteDTO = model.PersonalDependienteDTO;

            if (string.IsNullOrEmpty(model.PersonalDependienteDTO.Block))
               model.PersonalDependienteDTO.Block = string.Empty;

            personalDependienteDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(personalDependienteDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return personalDependienteDTO;
      }
      [HttpPost("BajaPersonalPeriodoLaboral")]
      public ActionResult BajaPersonalPeriodoLaboral([FromBody] PersonalViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.PersonalPeriodoLaboralDTO.DTOState = DTOState.Modified;
            AuditDTO(model.PersonalPeriodoLaboralDTO);

            personalPeriodoLaboralProxy.ActualizarCese(model.PersonalPeriodoLaboralDTO);

            resultResponse.Message = SR.GetString("RH_personal_periodo_laboral_cese");
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpPost("EliminarSede")]
      public ActionResult EliminarSede([FromBody] PersonalViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.PersonalDependienteDTO.DTOState = DTOState.Modified;
            AuditDTO(model.PersonalDependienteDTO);

            personalDependienteProxy.Delete(model.PersonalDependienteDTO);

            resultResponse.Message = SR.GetString("RH_personal_dependiente_anulado");
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpPost("BajaEditPopup")]
      public ActionResult BajaEditPopup([FromBody] PersonalViewModel model)
      {
         PersonalViewModel personalViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {

            personalViewModel.PersonalPeriodoLaboralDTO = personalPeriodoLaboralProxy.GetById(model.PersonalPeriodoLaboralDTO);

            List<MotivoFinPeriodoDTO> motivoFinPeriodoDTOList = motivoFinPeriodoProxy.Search(new MotivoFinPeriodoDTO() { 
               Estado = EstadoConstante.ACTIVO });
            personalViewModel.MotivoFinPeriodoList = WebHelper.ToSelectListItem(motivoFinPeriodoDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(personalViewModel);
      }
      [HttpPost("BajaDependienteEditPopup")]
      public ActionResult BajaDependienteEditPopup([FromBody] PersonalViewModel model)
      {
         PersonalViewModel personalViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {

            personalViewModel.PersonalDependienteDTO = personalDependienteProxy.GetById(model.PersonalDependienteDTO);

            List<MotivoBajaDependienteDTO> motivoBajaDependienteDTOList = motivoBajaDependienteProxy
               .Search(new MotivoBajaDependienteDTO() { Estado = EstadoConstante.ACTIVO });
            personalViewModel.MotivoBajaDependienteList = WebHelper.ToSelectListItem(motivoBajaDependienteDTOList, x => x.Id.ToString(), x => x.Abreviatura,
               SelectListFirstOptionType.Select);

         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(personalViewModel);
      }
      [HttpPost("BajaPersonalDependiente")]
      public ActionResult BajaPersonalDependiente([FromBody] PersonalViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.PersonalDependienteDTO.DTOState = DTOState.Modified;
            AuditDTO(model.PersonalDependienteDTO);

            personalDependienteProxy.ActualizarBaja(model.PersonalDependienteDTO);

            resultResponse.Message = SR.GetString("RH_personal_dependiente_baja");
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpPost("ListarPersonalPlanilla")]
      public ActionResult ListarPersonalPlanilla(string jsonModel)
      {

         Logger.InfoStart();

         PersonalViewModel model = JsonSerializer.Deserialize<PersonalViewModel>(jsonModel);

         model.PersonalPlanillaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();

         try
         {
            List<PersonalPlanillaDTO> personalPlanillaDTOList = personalPlanillaProxy.Search(model.PersonalPlanillaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = personalPlanillaDTOList.Count,
               rows = from f in personalPlanillaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PersonalPlanillaId,
                            f.PersonalId,
                            f.PlanillaId,
                            f.Estado,
                            DTOState = DTOState.Unchanged
                         }
                      }
            };

            Logger.InfoFinish();
            return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }
      }
      [HttpPost("GrabarPersonalPlanilla")]
      public ActionResult GrabarPersonalPlanilla([FromBody] PersonalViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PersonalPlanillaDTO personalPlanillaDTO = model.PersonalPlanillaDTO;
            personalPlanillaDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(personalPlanillaDTO);

            switch (personalPlanillaDTO.DTOState)
            {
               case DTOState.Added:
                  personalPlanillaProxy.Insert(personalPlanillaDTO);
                  break;

               case DTOState.Modified:
                  personalPlanillaProxy.Update(personalPlanillaDTO);
                  break;
            }

            resultResponse.Message = SR.GetString("RH_personal_planilla_agregada");
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpPost("EliminarPersonalPlanilla")]
      public ActionResult EliminarPersonalPlanilla([FromBody] PersonalViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.PersonalPlanillaDTO.DTOState = DTOState.Deleted;

            personalPlanillaProxy.Delete(model.PersonalPlanillaDTO);

            resultResponse.Message = SR.GetString("RH_personal_planilla_anulada");

         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [Route("ListPopup")]
      public IActionResult ListPopup([FromBody] PersonalViewModel model)
      {
         if (model == null) {
            model = new PersonalViewModel
            {
               PersonalDTO = new PersonalDTO()
            };
         }

         model.ModalEditId = GetModalEditPanelId();
         return PartialView(model);
      }
   }
}