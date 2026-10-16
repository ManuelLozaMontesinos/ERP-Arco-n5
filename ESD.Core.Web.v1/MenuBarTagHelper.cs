using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESD.Core.Web
{
   public enum MenuItemStyle
   {
      CRUD = 0,
      CRUDAndPreview = 1,
      CRUDAndPreviewExport = 2,
      CRUDAndPreviewExportList = 3,
      CRUDAndExport = 4,
      CRUDAndExportList = 5,
      CRUDAndSaveCancel = 6,
      CRUDAndSaveCancelPreview = 7,
      SaveExit = 8,
      PreviewExport = 9,
      PreviewExportList = 10,
      Export = 11,
      ExportData = 12,
      ExportList = 13,
      NewDelete = 14,
      NewEdit = 15,
      NewEditDelete = 16,
      NewDeleteAndSaveCancel = 17,
      NewEditAndSaveCancel = 18,
      EditAndSaveCancel = 19,
      Edit = 20,
      Preview = 21,
      Exit =22,
      Custom = 100
   }
   public enum MenuItemType
   {
      New = 0,
      Edit = 1,
      Copy = 2,
      Delete = 3,
      Save = 4,
      Cancel = 5,
      Exit = 6,
      Preview = 7,
      ImportXLS = 8,
      ExportXLS = 9,
      ExportDataXLS = 10,
      Divider = 11,
      Custom = 100
   }
   public class MenuItemKey
   {
      public const string NEW = "new";
      public const string COPY = "copy";
      public const string EDIT = "edit";
      public const string DELETE = "delete";
      public const string SAVE = "save";
      public const string CANCEL = "cancel";
      public const string EXIT = "exit";
      public const string PREVIEW = "preview";
      public const string EXPORT_XLS = "exportxls";
      public const string IMPORT_XLS = "importxls";
      public const string EXPORT_DATA_XLS = "dataxls";
   }
   public class MenuItem : ICloneable
   {
      [HtmlAttributeName("menuType")]
      public MenuItemType MenuItemType { get; set; } = MenuItemType.Custom;

      [HtmlAttributeName("content")]
      public string Content { get; set; }

      [HtmlAttributeName("key")]
      public string Key { get; set; }

      [HtmlAttributeName("font-image")]
      public string FontImage { get; set; }

      [HtmlAttributeName("disabled")]
      public bool Disabled { get; set; }

      [HtmlAttributeName("hidden")]
      public bool Hidden { get; set; } = false;

      [HtmlAttributeName("position")]
      public int Position { get; set; } = -1;

      public string ButtonStyle { get; set; }

      public List<MenuItem> Items { get; set; } = new List<MenuItem>();

      public MenuItem()
      {
      }
      public MenuItem(MenuItemType menuItemType, string key, string content, bool disabled)
      {
         MenuItemType = MenuItemType;
         Content = content;
         Key = key;
         Disabled = disabled;
      }

      public MenuItem(string key, string content, bool disabled)
      {
         Content = content;
         Key = key;
         Disabled = disabled;
      }
      public object Clone()
      {
         return new MenuItem()
         {
            MenuItemType = MenuItemType,
            Content = Content,
            Key = Key,
            Disabled = Disabled,
            ButtonStyle = ButtonStyle
         };
      }
   }

   [HtmlTargetElement("esd-menubar")]
   public class MenuBarTagHelper : BaseTagHelper
   {

      [HtmlAttributeName("menu-style")]
      public MenuItemStyle MenuStyle { get; set; } = MenuItemStyle.Custom;

      [HtmlAttributeName("size")]
      public MenuSize MenuSize { get; set; } = MenuSize.Default;

      [HtmlAttributeNotBound]
      public List<MenuItem> MenuItemList { get; set; } = new List<MenuItem>();

      [HtmlAttributeName("Title")]
      public string Title { get; set; }

      [HtmlAttributeName("allow-settings")]
      public bool AllowSettings { get; set; } = false;

      [HtmlAttributeName("onclick")]
      public string OnClick { get; set; }

      [ViewContext]
      [HtmlAttributeNotBound]
      public ViewContext ViewContext { get; set; }
      private Dictionary<string, MenuItem> DefaultItemsList { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         createDefaultItemsList();

         Id = ResolveId(Id);

         output.TagName = HtmlTagNameConstant.NAV;
         output.Attributes.Add("id", Id);
         output.Attributes.Add("name", Id);
         string menuSizeClass = MenuSize == MenuSize.Small ? "esd-menubar-sm" : "esd-menubar-default";
         output.Attributes.Add("class", $"navbar navbar-light bg-light esd-menubar {menuSizeClass}");

         TagBuilder containerTag = new(HtmlTagNameConstant.DIV);
         containerTag.AddCssClass("form-inline");

         switch (MenuStyle)
         {
            case MenuItemStyle.CRUD:
            case MenuItemStyle.CRUDAndPreview:
            case MenuItemStyle.CRUDAndPreviewExport:
            case MenuItemStyle.CRUDAndPreviewExportList:
            case MenuItemStyle.CRUDAndExport:
            case MenuItemStyle.CRUDAndExportList:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString(),false),
                  getDefaultItem(MenuItemType.Edit.ToString(),true),
                  getDefaultItem(MenuItemType.Delete.ToString(),true)
               };

               switch (MenuStyle)
               {
                  case MenuItemStyle.CRUDAndPreview:
                     MenuItemList.Add(getDefaultItem(MenuItemType.Preview.ToString(), true));
                     break;

                  case MenuItemStyle.CRUDAndExport:
                     MenuItemList.Add(getDefaultItem(MenuItemType.ExportXLS.ToString(), true));
                     break;
               }
               break;
            case MenuItemStyle.CRUDAndSaveCancel:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Edit.ToString(),true),
                  getDefaultItem(MenuItemType.Save.ToString(),true),
                  getDefaultItem(MenuItemType.Cancel.ToString(),true),
                  getDefaultItem(MenuItemType.Delete.ToString(),true)
               };
               break;
            case MenuItemStyle.SaveExit:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Save.ToString()),
                  getDefaultItem(MenuItemType.Exit.ToString())
               };
               break;
            case MenuItemStyle.PreviewExport:
            case MenuItemStyle.PreviewExportList:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Preview.ToString())
               };

               switch (MenuStyle)
               {
                  case MenuItemStyle.PreviewExport:
                     MenuItemList.Add(getDefaultItem(MenuItemType.ExportXLS.ToString()));
                     break;

                  case MenuItemStyle.PreviewExportList:
                     MenuItem menuItem = getDefaultItem(MenuItemType.ExportXLS.ToString());
                     menuItem.Key = string.Concat(menuItem.Key, "List");
                     menuItem.Items.Add(getDefaultItem(MenuItemType.ExportXLS.ToString()));
                     menuItem.Items.Add(getDefaultItem(MenuItemType.ExportDataXLS.ToString()));
                     MenuItemList.Add(menuItem);
                     break;
               }
               break;
            case MenuItemStyle.Edit:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Edit.ToString())
               };
               break;
            case MenuItemStyle.Preview:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Preview.ToString())
               };
               break;
            case MenuItemStyle.Exit:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Exit.ToString())
               };
               break;
            case MenuItemStyle.NewDelete:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Delete.ToString())
               };
               break;
            case MenuItemStyle.NewEdit:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Edit.ToString())
               };
               break;
            case MenuItemStyle.NewEditDelete:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Edit.ToString()),
                  getDefaultItem(MenuItemType.Delete.ToString())
               };
               break;
            case MenuItemStyle.NewDeleteAndSaveCancel:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Delete.ToString()),
                  getDefaultItem(MenuItemType.Save.ToString()),
                  getDefaultItem(MenuItemType.Cancel.ToString())
               };
               break;
            case MenuItemStyle.NewEditAndSaveCancel:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Edit.ToString()),
                  getDefaultItem(MenuItemType.Save.ToString()),
                  getDefaultItem(MenuItemType.Cancel.ToString())
               };
               break;
            case MenuItemStyle.EditAndSaveCancel:
               MenuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Edit.ToString()),
                  getDefaultItem(MenuItemType.Save.ToString()),
                  getDefaultItem(MenuItemType.Cancel.ToString())
               };
               break;
            default:
               foreach (MenuItem menuItem in MenuItemList)
               {
                  if (menuItem.MenuItemType != MenuItemType.Custom)
                  {
                     MenuItem menuItemTmp = getDefaultItem(menuItem.MenuItemType.ToString());
                     if (string.IsNullOrEmpty(menuItem.Key)) menuItem.Key = menuItemTmp.Key;
                     if (string.IsNullOrEmpty(menuItem.FontImage)) menuItem.FontImage = menuItemTmp.FontImage;
                     if (string.IsNullOrEmpty(menuItem.Content)) menuItem.Content = menuItemTmp.Content;
                     if (!menuItem.Disabled) menuItem.Disabled = menuItemTmp.Disabled;
                     if (!menuItem.Hidden) menuItem.Hidden = menuItemTmp.Hidden;
                     if (string.IsNullOrEmpty(menuItem.ButtonStyle)) menuItem.ButtonStyle = menuItemTmp.ButtonStyle;
                  }
               }
               break;
         }

         if (For != null)
         {
            List<MenuItem> menuItemListTmp = (List<MenuItem>)For.Model;
            foreach (MenuItem menuItem in menuItemListTmp)
            {
               if (menuItem.MenuItemType != MenuItemType.Custom)
               {
                  MenuItem menuItemTmp = getDefaultItem(menuItem.MenuItemType.ToString());
                  if (string.IsNullOrEmpty(menuItem.Key)) menuItem.Key = menuItemTmp.Key;
                  if (string.IsNullOrEmpty(menuItem.FontImage)) menuItem.FontImage = menuItemTmp.FontImage;
                  if (string.IsNullOrEmpty(menuItem.Content)) menuItem.Content = menuItemTmp.Content;
                  if (!menuItem.Disabled) menuItem.Disabled = menuItemTmp.Disabled;
                  if (!menuItem.Hidden) menuItem.Hidden = menuItemTmp.Hidden;
                  if (string.IsNullOrEmpty(menuItem.ButtonStyle)) menuItem.ButtonStyle = menuItemTmp.ButtonStyle;
               }

               if (menuItem.Position == -1)
                  MenuItemList.Add(menuItem);
               else
               {
                  MenuItemList.Insert(menuItem.Position, menuItem);
                  menuItem.Position = -1;
               }
            }
         }

         bool esSoloLectura = false;
         if (ViewContext != null)
         {
            if (ViewContext.ViewData.ContainsKey("EsSoloLectura") && ViewContext.ViewData["EsSoloLectura"] as bool? == true)
               esSoloLectura = true;
            else if (ViewContext.ViewBag != null && ViewContext.ViewBag.EsSoloLectura == true)
               esSoloLectura = true;
         }

         if (esSoloLectura)
         {
            MenuItemList.RemoveAll(item =>
               item.MenuItemType == MenuItemType.New ||
               item.MenuItemType == MenuItemType.Delete ||
               item.MenuItemType == MenuItemType.Save ||
               item.MenuItemType == MenuItemType.Cancel ||
               item.MenuItemType == MenuItemType.ImportXLS ||
               item.Key == MenuItemKey.NEW ||
               item.Key == MenuItemKey.DELETE ||
               item.Key == MenuItemKey.SAVE ||
               item.Key == MenuItemKey.CANCEL ||
               item.Key == MenuItemKey.IMPORT_XLS ||
               item.Key == "new" ||
               item.Key == "delete" ||
               item.Key == "save" ||
               item.Key == "cancel" ||
               item.Key == "aprobar" ||
               item.Key == "desaprobar" ||
               item.Key == "soli_auto" ||
               item.Key == "gen_pedido" ||
               item.Key == "gen_compro" ||
               item.Key == "cambiar_cotizacion" ||
               item.Key == "cambiar_estado" ||
               item.Key == "enviar_prod" ||
               item.Key == "cerrar" ||
               item.Key == "extornar"
            );

            foreach (var item in MenuItemList)
            {
               if (item.MenuItemType == MenuItemType.Edit || item.Key == MenuItemKey.EDIT || item.Key == "edit")
               {
                  item.MenuItemType = MenuItemType.Custom;
                  item.Key = MenuItemKey.EDIT;
                  item.Content = "Consultar";
                  item.FontImage = "fas fa-search";
               }
            }
         }

         StringBuilder stringBuilder = new();

         foreach (MenuItem menuItem in MenuItemList)
         {
            stringBuilder.Append(createMenuItem(Id, menuItem));
         }

         if (AllowSettings)
            stringBuilder.Append(createMenuItem(Id, new MenuItem()
            {
               Key = "setting",
               FontImage = "fas fa-cog",
               Disabled = false
            }));

         containerTag.InnerHtml.AppendHtml(stringBuilder.ToString());

         if(!string.IsNullOrEmpty(Title) && MenuSize != MenuSize.Small)
            output.Content.AppendHtml("<label class='navbar-brand mb-0 h4'>"+ Title + "</label>");

         output.Content.AppendHtml(containerTag);
      }
      private string createMenuItem(string menuId, MenuItem menuExItem)
      {
         bool esSoloLectura = false;
         if (ViewContext != null)
         {
            if (ViewContext.ViewData != null && ViewContext.ViewData.ContainsKey("EsSoloLectura") && ViewContext.ViewData["EsSoloLectura"] as bool? == true)
               esSoloLectura = true;
            else if (ViewContext.ViewBag != null && ViewContext.ViewBag.EsSoloLectura == true)
               esSoloLectura = true;
            else if (ViewContext.HttpContext != null && ViewContext.HttpContext.Items.ContainsKey("EsSoloLectura") && ViewContext.HttpContext.Items["EsSoloLectura"] as bool? == true)
               esSoloLectura = true;
         }

         if (esSoloLectura)
         {
            if (menuExItem.MenuItemType == MenuItemType.Edit || 
                menuExItem.Key == MenuItemKey.EDIT || 
                menuExItem.Key == "edit" || 
                (!string.IsNullOrEmpty(menuExItem.Content) && menuExItem.Content.Trim().Equals("Editar", StringComparison.OrdinalIgnoreCase)))
            {
               menuExItem.MenuItemType = MenuItemType.Custom;
               menuExItem.Key = MenuItemKey.EDIT;
               menuExItem.Content = "Consultar";
               menuExItem.FontImage = "fas fa-search";
            }
         }

         string optionInternalId = string.Concat(menuId, "_opt_", menuExItem.Key);
         string buttonSizeClass = string.Empty;

         if (string.IsNullOrEmpty(menuExItem.ButtonStyle))
            menuExItem.ButtonStyle = BootstrapButtonStyleClass.BUTTON_INFO;

         if (MenuSize == MenuSize.Small)
            buttonSizeClass = "app-menu-btn-xs";
         else
            buttonSizeClass = string.Concat(BootstrapClass.BUTTON, BootstrapBaseSizeClass.SMALL);

         if (menuExItem.Items.Count == 0)
            return string.Concat("<button id=\"" + optionInternalId + "\"",
               "class=\"btn " + menuExItem.ButtonStyle + " " + buttonSizeClass + " mr-sm-1\"",
               menuExItem.Disabled ? " disabled=\"disabled\"" : string.Empty,
               !string.IsNullOrEmpty(OnClick) ? " onclick=\"" + OnClick + "('" + menuExItem.Key + "');\"" : string.Empty,
               menuExItem.Hidden ? " style='display:none;'" : string.Empty,
               ">",
               !string.IsNullOrEmpty(menuExItem.FontImage) ?
               "<span class=\"" + menuExItem.FontImage + "\" data-fa-mask=\"" + menuExItem.FontImage + "\"></span>" : string.Empty,
               " " + menuExItem.Content,
               "</button>");

         string itemHtml = string.Concat("<div class=\"btn-group\">",
            "<button type=\"button\" class=\"btn " + menuExItem.ButtonStyle + " dropdown-toggle " + buttonSizeClass + "\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">",
            !string.IsNullOrEmpty(menuExItem.FontImage) ? "<span class=\"" + menuExItem.FontImage + "\"></span>" : string.Empty,
            " " + menuExItem.Content,
            "<span class=\"caret\"></span>",
            "</button>",
            "<ul class=\"dropdown-menu dropdown-menu-right\">");

         foreach (MenuItem item in menuExItem.Items)
         {
            if (item.MenuItemType != MenuItemType.Divider)
               itemHtml += "<li><a"
                  + " class=\"dropdown-item\""
                  + (!string.IsNullOrEmpty(OnClick) ? " onclick=\"" + OnClick + "('" + item.Key + "');\"" : string.Empty)
                  + " href=\"javascript:void(0)\">" + item.Content + "</a></li>";
            else
               itemHtml += "<li role=\"separator\" class=\"divider\"></li>";
         }
         itemHtml += "</ul></div>";

         return itemHtml;
      }
      private void createDefaultItemsList()
      {

         string prefixFontImage = "far fa-";

         DefaultItemsList = new Dictionary<string, MenuItem>
         {
            {
               MenuItemType.New.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.New,
                  Key = MenuItemKey.NEW,
                  Content = "Nuevo",
                  ButtonStyle = BootstrapButtonStyleClass.BUTTON_PRIMARY,
                  FontImage = prefixFontImage + "file"
               }
            },
            {
               MenuItemType.Edit.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.Edit,
                  Key = MenuItemKey.EDIT,
                  Content = "Editar",
                  ButtonStyle = BootstrapButtonStyleClass.BUTTON_SUCCESS,
                  FontImage = prefixFontImage + "edit"
               }
            },
            {
               MenuItemType.Copy.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.Copy,
                  Key = MenuItemKey.COPY,
                  Content = "Copiar",
                  FontImage = prefixFontImage + "copy"
               }
            },
            {
               MenuItemType.Delete.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.Delete,
                  Key = MenuItemKey.DELETE,
                  Content = "Eliminar",
                  ButtonStyle = BootstrapButtonStyleClass.BUTTON_DANGER,
                  FontImage = prefixFontImage + "trash-alt"
               }
            },
            {
               MenuItemType.Save.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.Save,
                  Key = MenuItemKey.SAVE,
                  ButtonStyle = BootstrapButtonStyleClass.BUTTON_PRIMARY,
                  Content = "Guardar",
                  FontImage = prefixFontImage + "save"
               }
            },
            {
               MenuItemType.Cancel.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.Cancel,
                  Key = MenuItemKey.CANCEL,
                  Content = "Cancelar",
                  ButtonStyle = BootstrapButtonStyleClass.BUTTON_WARNING,
                  FontImage = prefixFontImage + "ban"
               }
            },
            {
               MenuItemType.Exit.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.Exit,
                  Key = MenuItemKey.EXIT,
                  Content = "Salir",
                  ButtonStyle = BootstrapButtonStyleClass.BUTTON_WARNING,
                  FontImage = prefixFontImage + "sign-out-alt"
               }
            },
            {
               MenuItemType.Preview.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.Preview,
                  Key = MenuItemKey.PREVIEW,
                  Content = "Vista Previa",
                  FontImage = prefixFontImage + "print"
               }
            },
            {
               MenuItemType.ImportXLS.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.ImportXLS,
                  Key = MenuItemKey.IMPORT_XLS,
                  Content = "Importar desde XLS",
                  FontImage = prefixFontImage + "file-import"
               }
            },
            {
               MenuItemType.ExportXLS.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.ExportXLS,
                  Key = MenuItemKey.EXPORT_XLS,
                  Content = "Exportar (XLS)",
                  FontImage = prefixFontImage + "file-export"
               }
            },
            {
               MenuItemType.ExportDataXLS.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.ExportDataXLS,
                  Key = MenuItemKey.EXPORT_DATA_XLS,
                  Content = "Exportar solo Datos(XLS)",
                  FontImage = prefixFontImage + "table"
               }
            },
            {
               MenuItemType.Divider.ToString(),
               new MenuItem()
               {
                  MenuItemType = MenuItemType.Divider
               }
            }
         };
      }
      private MenuItem getDefaultItem(string key)
      {
         return getDefaultItem(key, false);
      }
      private MenuItem getDefaultItem(string key, bool disabled)
      {
         if (!DefaultItemsList.ContainsKey(key))
            return null;

         MenuItem menuItem = (MenuItem)DefaultItemsList[key].Clone();
         menuItem.Disabled = disabled;

         return menuItem;
      }
      public List<MenuItem> CreateMenuStyleList(MenuItemStyle menuItemStyle)
      {
         List<MenuItem> menuItemList = null;

         createDefaultItemsList();

         switch (menuItemStyle)
         {
            case MenuItemStyle.CRUD:
            case MenuItemStyle.CRUDAndPreview:
            case MenuItemStyle.CRUDAndPreviewExport:
            case MenuItemStyle.CRUDAndPreviewExportList:
            case MenuItemStyle.CRUDAndExport:
            case MenuItemStyle.CRUDAndExportList:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString(),false),
                  getDefaultItem(MenuItemType.Edit.ToString(),true),
                  getDefaultItem(MenuItemType.Delete.ToString(),true)
               };

               switch (menuItemStyle)
               {
                  case MenuItemStyle.CRUDAndPreview:
                     menuItemList.Add(getDefaultItem(MenuItemType.Preview.ToString(), true));
                     break;

                  case MenuItemStyle.CRUDAndExport:
                     menuItemList.Add(getDefaultItem(MenuItemType.ExportXLS.ToString(), true));
                     break;
               }
               break;
            case MenuItemStyle.CRUDAndSaveCancel:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Edit.ToString(),true),
                  getDefaultItem(MenuItemType.Save.ToString(),true),
                  getDefaultItem(MenuItemType.Cancel.ToString(),true),
                  getDefaultItem(MenuItemType.Delete.ToString(),true)
               };
               break;
            case MenuItemStyle.SaveExit:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Save.ToString()),
                  getDefaultItem(MenuItemType.Exit.ToString())
               };
               break;
            case MenuItemStyle.PreviewExport:
            case MenuItemStyle.PreviewExportList:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Preview.ToString())
               };

               switch (menuItemStyle)
               {
                  case MenuItemStyle.PreviewExport:
                     menuItemList.Add(getDefaultItem(MenuItemType.ExportXLS.ToString()));
                     break;

                  case MenuItemStyle.PreviewExportList:
                     MenuItem menuItem = getDefaultItem(MenuItemType.ExportXLS.ToString());
                     menuItem.Key = string.Concat(menuItem.Key, "List");
                     menuItem.Items.Add(getDefaultItem(MenuItemType.ExportXLS.ToString()));
                     menuItem.Items.Add(getDefaultItem(MenuItemType.ExportDataXLS.ToString()));
                     menuItemList.Add(menuItem);
                     break;
               }
               break;
            case MenuItemStyle.Preview:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Preview.ToString())
               };
               break;
            case MenuItemStyle.Exit:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Exit.ToString())
               };
               break;
            case MenuItemStyle.NewDelete:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Delete.ToString())
               };
               break;
            case MenuItemStyle.NewEdit:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Edit.ToString())
               };
               break;
            case MenuItemStyle.NewEditDelete:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Edit.ToString()),
                  getDefaultItem(MenuItemType.Delete.ToString())
               };
               break;
            case MenuItemStyle.NewDeleteAndSaveCancel:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Delete.ToString()),
                  getDefaultItem(MenuItemType.Save.ToString()),
                  getDefaultItem(MenuItemType.Cancel.ToString())
               };
               break;
            case MenuItemStyle.NewEditAndSaveCancel:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.New.ToString()),
                  getDefaultItem(MenuItemType.Edit.ToString()),
                  getDefaultItem(MenuItemType.Save.ToString()),
                  getDefaultItem(MenuItemType.Cancel.ToString())
               };
               break;
            case MenuItemStyle.EditAndSaveCancel:
               menuItemList = new List<MenuItem>() {
                  getDefaultItem(MenuItemType.Edit.ToString()),
                  getDefaultItem(MenuItemType.Save.ToString()),
                  getDefaultItem(MenuItemType.Cancel.ToString())
               };
               break;
         }

         return menuItemList;
      }

   }
}




