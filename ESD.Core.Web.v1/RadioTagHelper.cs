using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ESD.Core.Web
{
   [HtmlTargetElement("esd-radiobutton")]
   public class RadioTagHelper : BaseTagHelper
   {

      [HtmlAttributeName("col-position")]
      public string ColumnPosition { get; set; }

      [HtmlAttributeName("label")]
      public string Label { get; set; }

      [HtmlAttributeName("onchange")]
      public string OnChange { get; set; }

      [HtmlAttributeName("group-name")]
      public string GroupName { get; set; }

      [HtmlAttributeName("checked")]
      public bool Checked { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {

         TagBuilder inputTag = new TagBuilder(HtmlTagNameConstant.INPUT);
         inputTag.Attributes.Add("type", InputTagType.Radio.ToString().ToLower());

         if (For != null)
            Id = ResolveId(For.Name);
         else
            Id = ResolveId(Id);

         inputTag.GenerateId(Id, string.Empty);

         if(string.IsNullOrEmpty(GroupName))
            inputTag.Attributes.Add("name", Id);
         else
            inputTag.Attributes.Add("name", GroupName);

         //string value = string.Empty;
         //if (For != null)
         //   value = (For.Model == null ? string.Empty : For.Model.ToString());

         if (Checked)
            inputTag.Attributes.Add("checked", "checked");

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
         output.Attributes.Add("class", ColumnPosition + " icheck-primary");
         output.Content.AppendHtml(inputTag);

         if (!string.IsNullOrEmpty(OnChange))
            output.Attributes.Add("onchange", OnChange);

      }

   }
}
