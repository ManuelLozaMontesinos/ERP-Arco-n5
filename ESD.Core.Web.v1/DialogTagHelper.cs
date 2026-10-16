using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESD.Core.Web
{
   public enum DialogSize
   {
      Default = 0,
      Small = 1,
      Medium = 2,
      Large = 3,
      ExtraLarge = 4
   }
   public enum DialogButtonStyle
   {
      None = 0,
      OkCancel = 1,
      Cancel = 2,
      Close = 3
   }

   [HtmlTargetElement("esd-dialog")]
   public class DialogTagHelper: BaseTagHelper
   {

      [HtmlAttributeName("title")]
      public string Title { get; set; }

      [HtmlAttributeName("size")]
      public DialogSize DialogSize { get; set; } = DialogSize.Default;

      [HtmlAttributeName("button-style")]
      public DialogButtonStyle DialogButtonStyle { get; set; } = DialogButtonStyle.OkCancel;

      [HtmlAttributeName("onclick")]
      public string OnClick { get; set; }

      [HtmlAttributeName("enable-dismissclick")]
      public bool EnableDismissClick { get; set; }

      private Dictionary<string, MenuItem> DefaultItemsList { get; set; }
      public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
      {

         TagBuilder tagDialog = new(HtmlTagNameConstant.DIV);
         tagDialog.AddCssClass("modal-dialog");
         tagDialog.Attributes.Add("role", "document");

         switch (DialogSize)
         {
            case DialogSize.Small:
               tagDialog.AddCssClass("modal-sm");
               break;
            case DialogSize.Medium:
               tagDialog.AddCssClass("modal-md");
               break;
            case DialogSize.Large:
               tagDialog.AddCssClass("modal-lg");
               break;
            case DialogSize.ExtraLarge:
               tagDialog.AddCssClass("modal-xl");
               break;
         }

         TagBuilder tagContent = new(HtmlTagNameConstant.DIV);
         tagContent.AddCssClass("modal-content");

         TagBuilder tagHeader = new(HtmlTagNameConstant.DIV);
         tagHeader.AddCssClass("modal-header");

         TagBuilder tagTitle = new("h6");
         tagTitle.Attributes.Add("id",string.Concat(Id, "-title"));
         tagTitle.AddCssClass("modal-title");
         tagTitle.InnerHtml.AppendHtml(Title);

         TagBuilder tagCloseButton = new(HtmlTagNameConstant.BUTTON);
         tagCloseButton.Attributes.Add("type", InputTagType.Button.ToString());
         tagCloseButton.AddCssClass("close");
         tagCloseButton.Attributes.Add("data-dismiss", "modal");
         tagCloseButton.Attributes.Add("aria-label", "Cerrar");

         if(!string.IsNullOrEmpty(OnClick))
            if (EnableDismissClick)
               tagCloseButton.Attributes.Add("onclick", OnClick + "('close');");

         tagCloseButton.InnerHtml.AppendHtml("<span aria-hidden=\"true\">&times;</span>");

         TagBuilder tagBody = new(HtmlTagNameConstant.DIV);
         tagBody.AddCssClass("modal-body");

         TagBuilder tagFooter = null;

         if(DialogButtonStyle != DialogButtonStyle.None)
         {
            tagFooter = new TagBuilder(HtmlTagNameConstant.DIV);
            tagFooter.AddCssClass("modal-footer");

            TagBuilder tagFooterButton = new(HtmlTagNameConstant.BUTTON);

            switch (DialogButtonStyle)
            {
               case DialogButtonStyle.OkCancel:
                  tagFooterButton.Attributes.Add("type", InputTagType.Button.ToString());
                  tagFooterButton.AddCssClass("btn btn-sm btn-secondary");
                  tagFooterButton.Attributes.Add("data-dismiss", "modal");

                  if (!string.IsNullOrEmpty(OnClick))
                     if (EnableDismissClick)
                        tagFooterButton.Attributes.Add("onclick", OnClick + "('cancel');");

                  tagFooterButton.InnerHtml.AppendHtml("Cancelar");
                  tagFooter.InnerHtml.AppendHtml(tagFooterButton);

                  tagFooterButton = new TagBuilder(HtmlTagNameConstant.BUTTON);
                  tagFooterButton.Attributes.Add("id", string.Concat(Id, "-OkButton"));
                  tagFooterButton.Attributes.Add("name", string.Concat(Id, "-OkButton"));
                  tagFooterButton.Attributes.Add("type", InputTagType.Button.ToString());
                  tagFooterButton.AddCssClass("btn btn-sm btn-primary");

                  if (!string.IsNullOrEmpty(OnClick))
                     if (EnableDismissClick)
                        tagFooterButton.Attributes.Add("onclick", OnClick + "('ok');");
                     else
                        tagFooterButton.Attributes.Add("onclick", OnClick + "();");

                  tagFooterButton.InnerHtml.AppendHtml("Aceptar");
                  tagFooter.InnerHtml.AppendHtml(tagFooterButton);
                  break;

               case DialogButtonStyle.Cancel:
                  tagFooterButton.Attributes.Add("type", InputTagType.Button.ToString());
                  tagFooterButton.AddCssClass("btn btn-sm btn-primary");
                  tagFooterButton.Attributes.Add("data-dismiss", "modal");

                  if (!string.IsNullOrEmpty(OnClick))
                     if (EnableDismissClick)
                        tagFooterButton.Attributes.Add("onclick", OnClick + "('cancel');");

                  tagFooterButton.InnerHtml.AppendHtml("Cancelar");
                  tagFooter.InnerHtml.AppendHtml(tagFooterButton);
                  break;

               case DialogButtonStyle.Close:
                  tagFooterButton.Attributes.Add("type", InputTagType.Button.ToString());
                  tagFooterButton.AddCssClass("btn btn-sm btn-primary");
                  tagFooterButton.Attributes.Add("data-dismiss", "modal");

                  if (!string.IsNullOrEmpty(OnClick))
                     if (EnableDismissClick)
                        tagFooterButton.Attributes.Add("onclick", OnClick + "('close');");

                  tagFooterButton.InnerHtml.AppendHtml("Cerrar");
                  tagFooter.InnerHtml.AppendHtml(tagFooterButton);
                  break;
            }

         }

         tagHeader.InnerHtml.AppendHtml(tagTitle);
         tagHeader.InnerHtml.AppendHtml(tagCloseButton);
         tagContent.InnerHtml.AppendHtml(tagHeader);
         tagContent.InnerHtml.AppendHtml(tagBody);
         if(tagFooter != null)
            tagContent.InnerHtml.AppendHtml(tagFooter);
         tagDialog.InnerHtml.AppendHtml(tagContent);

         output.TagName = HtmlTagNameConstant.DIV;
         output.Attributes.Add("id", Id);
         output.Attributes.Add("role", "dialog");
         output.Attributes.Add("aria-labelledby", "myModalLabel");
         output.Attributes.Add("aria-hidden", "true");
         output.Attributes.Add("data-dialog-params", string.Empty);
         output.Attributes.Add("class", "modal fade");

         string childContent = (await output.GetChildContentAsync()).GetContent();

         tagBody.InnerHtml.AppendHtml(childContent);

         output.Content.AppendHtml(tagDialog);

         StringBuilder sb = new();

         sb.Append("$(function(){");
         sb.Append(string.Format("$('#{0}').modal(", Id));
         sb.Append("{ backdrop: 'static', show: false, keyboard: false }");
         sb.Append(");");
         sb.Append("});");

         TagBuilder tagScript = CreateScriptTag();
         tagScript.InnerHtml.AppendHtml(sb.ToString());

         output.PostElement.AppendHtml(tagScript);
      }

   }
}
