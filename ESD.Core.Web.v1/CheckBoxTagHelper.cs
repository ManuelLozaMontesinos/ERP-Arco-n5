using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ESD.Core.Web
{
   public enum CheckBoxStyle
   {
      Default = 0,
      Enhanced = 1
   }

   [HtmlTargetElement("esd-checkbox")]
   public class CheckBoxTagHelper: BaseTagHelper
   {

      [HtmlAttributeName("col-position")]
      public string ColumnPosition { get; set; }

      [HtmlAttributeName("label")]
      public string Label { get; set; }

      [HtmlAttributeName("onchange")]
      public string OnChange { get; set; }

      [HtmlAttributeName("checked")]
      public bool Checked { get; set; }

      [HtmlAttributeName("checkbox-style")]
      public CheckBoxStyle CheckBoxStyle { get; set; } = CheckBoxStyle.Enhanced;

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {

         TagBuilder inputTag = new(HtmlTagNameConstant.INPUT);
         inputTag.Attributes.Add("type", InputTagType.Checkbox.ToString().ToLower());

         if (For != null)
            Id = ResolveId(For.Name);
         else
            Id = ResolveId(Id);

         inputTag.GenerateId(Id, string.Empty);
         inputTag.Attributes.Add("name", Id);

         string value = string.Empty;
         if (For != null)
            value = (For.Model == null ? string.Empty : For.Model.ToString());

         if ((value == DigitoConstante.UNO || value == LetraConstante.A) || value.ToLower() == "true")
            inputTag.Attributes.Add("checked", "checked");
         else
         {
            if (Checked)
               inputTag.Attributes.Add("checked", "checked");
         }

         if (!Enable)
            inputTag.Attributes.Add("disabled", "disabled");

         TagBuilder labelTag = null;
         if (!string.IsNullOrEmpty(Label))
         {

            labelTag = new TagBuilder(HtmlTagNameConstant.LABEL);
            labelTag.MergeAttribute("for", Id);
            labelTag.AddCssClass("form-check-label");
            labelTag.InnerHtml.AppendHtml(Label);
         }

         if (labelTag != null)
            inputTag.InnerHtml.AppendHtml(labelTag);

         output.TagName = HtmlTagNameConstant.DIV;

         if(CheckBoxStyle == CheckBoxStyle.Enhanced)
            output.Attributes.Add("class", ColumnPosition + " icheck-primary");
         else
         {
            inputTag.Attributes.Add("class", "form-check-input");
            output.Attributes.Add("class", ColumnPosition + " form-check");
         }

         output.Content.AppendHtml(inputTag);

         if (!string.IsNullOrEmpty(OnChange))
            output.Attributes.Add("onchange", OnChange);

      }

   }
}
