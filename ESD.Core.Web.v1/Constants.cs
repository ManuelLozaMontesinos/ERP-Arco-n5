namespace ESD.Core.Web
{
   internal class HtmlTagNameConstant
   {
      internal const string INPUT = "input";
      internal const string LABEL = "label";
      internal const string DIV = "div";
      internal const string SELECT = "select";
      internal const string BUTTON = "button";
      internal const string SPAN = "span";
      internal const string NAV = "nav";
      internal const string TEXTAREA = "textarea";
      internal const string I = "i";
   }
   internal class BootstrapClass
   {
      public const string BUTTON = "btn";
   }
   internal class BootstrapBaseSizeClass
   {
      public const string LARGE = "-lg";
      public const string SMALL = "-sm";
      public const string EXTRA_SMALL = "-xs";
   }
   internal class BootstrapButtonStyleClass
   {
      public const string BUTTON_DEFAULT = BootstrapClass.BUTTON + "-default";
      public const string BUTTON_PRIMARY = BootstrapClass.BUTTON + "-primary";
      public const string BUTTON_SUCCESS = BootstrapClass.BUTTON + "-success";
      public const string BUTTON_INFO = BootstrapClass.BUTTON + "-info";
      public const string BUTTON_WARNING = BootstrapClass.BUTTON + "-warning";
      public const string BUTTON_DANGER = BootstrapClass.BUTTON + "-danger";
      public const string BUTTON_LINK = BootstrapClass.BUTTON + "-link";
      public const string BUTTON_OUTLINE_PRIMARY = BootstrapClass.BUTTON + "-outline-primary";
   }

   public enum ButtonSize
   {
      Default = 0,
      Small = 1,
      Large = 2,
      ExtraSmall = 3
   }
   public enum ButtonStyle
   {
      Default = 0,
      Primary = 1,
      Success = 2,
      Info = 3,
      Warning = 4,
      Danger = 5,
      Link = 6
   }
   public enum MenuSize
   {
      Default = 0,
      Small = 1
   }
   public enum SelectListFirstOptionType
   {
      Empty = 0,
      Blank = 1,
      All = 2,
      Select = 3,
      ListItems = 9
   }
   public class SelectListFirstOptionNameConstant
   {
      public const string BLANK = "";
      public const string ALL = "(TODOS)";
      public const string SELECT = "(SELECCIONE)";

   }
}
