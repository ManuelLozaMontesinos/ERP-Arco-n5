using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ESD.Core.Web
{
   [HtmlTargetElement("esd-button")]
   public class ButtonTagHelper : BaseTagHelper
   {
      [HtmlAttributeName("content")]
      public string Content { get; set; }

      [HtmlAttributeName("block")]
      public bool Block { get; set; }

      [HtmlAttributeName("size")]
      public ButtonSize ButtonSize { get; set; } = ButtonSize.Default;

      [HtmlAttributeName("button-style")]
      public ButtonStyle ButtonStyle { get; set; } = ButtonStyle.Default;

      [HtmlAttributeName("onclick")]
      public string OnClick { get; set; }

      [HtmlAttributeName("font-image")]
      public string FontImage { get; set; }

      [HtmlAttributeName("class")]
      public string Class { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {

         if (!string.IsNullOrEmpty(Id))
            Id = ResolveId(Id);

         output.TagName = HtmlTagNameConstant.BUTTON;
         output.Attributes.Add("id", Id);
         //output.Attributes.Add("name", Id);
         output.Attributes.Add("type", HtmlTagNameConstant.BUTTON);

         string style = resolveStyle();
         string size = resolveSize();

         output.Attributes.Add("class", style + " " + size 
            + (Block ? " btn-block" : string.Empty)
            + (!string.IsNullOrEmpty(Class) ? " " + Class : string.Empty)
            );

         if (!string.IsNullOrEmpty(FontImage)) {
            TagBuilder tagBuilder = new TagBuilder(HtmlTagNameConstant.I);
            tagBuilder.AddCssClass(FontImage);

            output.Content.AppendHtml(tagBuilder);
         }

         if (!string.IsNullOrEmpty(Content))
            output.Content.AppendHtml(Content);

         resolveOnClick();
         if (!string.IsNullOrEmpty(OnClick))
            output.Attributes.Add("onclick", OnClick);

      }
      private string resolveSize()
      {
         string className = string.Concat(BootstrapClass.BUTTON, SeparadorConstante.ESPACIO);

         switch (ButtonSize)
         {
            case ButtonSize.Default:
            case ButtonSize.Large:
               className = string.Concat(className, BootstrapClass.BUTTON, BootstrapBaseSizeClass.LARGE);
               break;

            case ButtonSize.Small:
               className = string.Concat(className, BootstrapClass.BUTTON, BootstrapBaseSizeClass.SMALL);
               break;

            case ButtonSize.ExtraSmall:
               className = string.Concat(className, BootstrapClass.BUTTON, BootstrapBaseSizeClass.EXTRA_SMALL);
               break;
         }

         return className;
      }
      private string resolveStyle()
      {
         string className = string.Empty;

         switch (ButtonStyle)
         {
            case ButtonStyle.Default:
               className = BootstrapButtonStyleClass.BUTTON_DEFAULT;
               break;
            case ButtonStyle.Primary:
               className = BootstrapButtonStyleClass.BUTTON_PRIMARY;
               break;
            case ButtonStyle.Success:
               className = BootstrapButtonStyleClass.BUTTON_SUCCESS;
               break;
            case ButtonStyle.Info:
               className = BootstrapButtonStyleClass.BUTTON_INFO;
               break;
            case ButtonStyle.Warning:
               className = BootstrapButtonStyleClass.BUTTON_WARNING;
               break;
            case ButtonStyle.Danger:
               className = BootstrapButtonStyleClass.BUTTON_DANGER;
               break;
            case ButtonStyle.Link:
               className = BootstrapButtonStyleClass.BUTTON_LINK;
               break;
         }

         return className;
      }
      private void resolveOnClick()
      {
         if (string.IsNullOrEmpty(OnClick))
            return;

         if (OnClick.IndexOf("(") > 0)
            return;

         OnClick += "()";
      }

   }
}
