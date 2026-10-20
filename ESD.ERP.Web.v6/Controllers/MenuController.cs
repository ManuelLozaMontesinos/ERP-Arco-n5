using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.Web.Model;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ESD.ERP.Web.Controllers
{
    public class MenuController : AuthorizedController
   {
      private readonly IMenuProxy menuProxy;
      private readonly IRolMenuProxy rolMenuProxy;

      public MenuController(
         IMenuProxy menuProxy,
         IRolMenuProxy rolMenuProxy
         )
      {
         this.menuProxy = menuProxy;
         this.rolMenuProxy = rolMenuProxy;
      }
      public IActionResult Index()
      {
         return View();
      }
      public JsonResult ObtenerMenuAsignado()
      {

         List<MenuDTO> menuDTOList = new();
         if (EsAdministrador())
            menuDTOList = menuProxy.Search(new MenuDTO() { OrderBy = "menu_id", Estado = EstadoConstante.ACTIVO });
         else {

            List<RolMenuDTO> rolMenuDTOList = rolMenuProxy.Search(new RolMenuDTO() { 
               RolCodigoList = ObtenerRoles(),
               Lectura = SiNoConstante.S
            });

            foreach (RolMenuDTO rolMenuDTO in rolMenuDTOList)
            {
               if (menuDTOList.Count > 0)
               {
                  if (menuDTOList.SingleOrDefault(m => m.MenuId == rolMenuDTO.MenuDTO.MenuId) == null)
                     menuDTOList.Add(rolMenuDTO.MenuDTO);
               }
               else
                  menuDTOList.Add(rolMenuDTO.MenuDTO);
            }
         }

         List<MenuTreeNode> menuNodeList = obtenerArbol(menuDTOList.Distinct().OrderBy(x => x.MenuId).ToList(), null);

         if (menuNodeList != null && !menuNodeList.Any(m => m.Text != null && m.Text.Trim().Equals("Calendario", System.StringComparison.OrdinalIgnoreCase)))
         {
            menuNodeList.Add(new MenuTreeNode
            {
               Text = "Calendario",
               Icon = "far fa-calendar-alt ",
               Selectable = true,
               Url = Url.Action("Index", "Calendario"),
               Nodes = null
            });
         }

         return Json(menuNodeList);
      }
      private List<MenuTreeNode> obtenerArbol(List<MenuDTO> menuDTOList, int? parent)
      {
         List<MenuTreeNode> menuDTOTempList = menuDTOList.Where(x => x.MenuPadreId == parent).Select(x => new MenuTreeNode
         {
            Text = x.Nombre,
            Icon = !string.IsNullOrEmpty(x.Icono) ? string.Concat(x.Icono,SeparadorConstante.ESPACIO) : string.Empty,
            Selectable = true,
            Url = obtenerURL(x),
            Nodes = obtenerArbol(menuDTOList, x.MenuId),
         }).ToList();

         if (menuDTOTempList.Count == 0)
            menuDTOTempList = null;

         return menuDTOTempList;
      }
      private string obtenerURL(MenuDTO menuDTO)
      {
         string parameters = string.Empty;
         string url = string.Empty;

         if (string.IsNullOrEmpty(menuDTO.Url))
            url = string.Empty;
         else
         {
            string actionName = menuDTO.Url.Split('/')[1];
            string controllerName = menuDTO.Url.Split('/')[0];
            if (actionName.IndexOf('?') > 0)
            {
               parameters = string.Concat("?", actionName.Split('?')[1]);
               actionName = actionName.Split('?')[0];
            }
            url = Url.Action(actionName, controllerName, new { Area = menuDTO.Area });
         }
         return string.Concat(url, parameters);
      }
   }
}