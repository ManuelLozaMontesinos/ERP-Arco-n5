using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace ESD.Core.Web
{
   [HtmlTargetElement("esd-masktextbox")]
   public class MaskTextBoxTagHelper : BaseTagHelper
   {

      [HtmlAttributeName("col-position")]
      public string ColumnPosition { get; set; }

      [HtmlAttributeName("label")]
      public string Label { get; set; }

      [HtmlAttributeName("col-label-position")]
      public string ColumnLabelPosition { get; set; }

      [HtmlAttributeName("mask")]
      public string Mask { get; set; }

      [HtmlAttributeName("place-holder")]
      public string PlaceHolder { get; set; }

      [HtmlAttributeName("onchange")]
      public string OnChange { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         string value = string.Empty;

         if (For != null)
            Id = ResolveId(For.Name);
         else
            Id = ResolveId(Id);

         TagBuilder tagInput = new TagBuilder(HtmlTagNameConstant.INPUT);
         tagInput.Attributes.Add("type", InputTagType.Text.ToString().ToLower());

         tagInput.GenerateId(Id, string.Empty);
         tagInput.Attributes.Add("name", Id);
         tagInput.Attributes.Add("class", "form-control form-control-sm");

         if (For != null)
            value = (For.Model == null ? string.Empty : For.Model.ToString());

         tagInput.Attributes.Add("value", value);

         if(!Enable)
            tagInput.Attributes.Add("readonly", "readonly");

         if (!string.IsNullOrEmpty(OnChange))
            tagInput.Attributes.Add("onchange", resolveOnChange(OnChange));

         output.TagName = HtmlTagNameConstant.DIV;
         if (!string.IsNullOrEmpty(ColumnPosition))
            output.Attributes.Add("class", ColumnPosition);

         output.Content.AppendHtml(tagInput);

         if (!string.IsNullOrEmpty(Label))
         {
            TagBuilder tagLabel = new TagBuilder(HtmlTagNameConstant.LABEL);

            tagLabel.MergeAttribute("for", Id);
            tagLabel.AddCssClass("col-form-label");
            tagLabel.AddCssClass(ColumnLabelPosition);
            tagLabel.InnerHtml.AppendHtml(Label);
            output.PreElement.AppendHtml(tagLabel);
         }

         if (!string.IsNullOrEmpty(Mask)) {

            string options = buildAdditionalOptions();

            string js = string.Concat(
               "$('#",Id,"').inputmask('",Mask,"'",
               !string.IsNullOrEmpty(options) ? string.Concat(",{",options,"}") : string.Empty,
               ");"
               );

            TagBuilder tagScript = CreateScriptTag();
            tagScript.InnerHtml.AppendHtml(js);

            output.PostElement.AppendHtml(tagScript);
         }
      }
      private string buildAdditionalOptions() {

         return string.Concat(
            PlaceHolder == null ? string.Empty : string.Concat("'placeholder':'", PlaceHolder,"'")
            );
      }
      private string resolveOnChange(string value)
      {
         if (value.IndexOf(")") == -1)
            value += "()";

         if (value.IndexOf(";") == -1)
            value += ";";

         return value;
      }
   }
}
