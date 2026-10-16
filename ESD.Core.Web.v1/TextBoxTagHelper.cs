using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ESD.Core.Web
{
   [HtmlTargetElement("esd-textbox")]
   public class TextBoxTagHelper: InputTagHelper
   {

      [HtmlAttributeName("max-length")]
      public int MaxLength { get; set; }

      [HtmlAttributeName("is-password")]
      public bool IsPassword { get; set; }

      [HtmlAttributeName("use-textcase")]
      public bool UseTextCase { get; set; } = true;

      [HtmlAttributeName("generate-label-id")]
      public bool GenerateLabelId { get; set; } = false;

      [HtmlAttributeName("value")]
      public string Value { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {

         TagBuilder tagBuilder = null;
         if (string.IsNullOrEmpty(ColumnPosition))
         {
            output.TagName = HtmlTagNameConstant.INPUT;
         }
         else {
            output.TagName = HtmlTagNameConstant.DIV;
            output.Attributes.Add("class", ColumnPosition);
         }

         InputTagType inputTagType = InputTagType.Text;
         if (IsPassword) {
            inputTagType = InputTagType.Password;
            UseTextCase = false;
         }

         InitializeTag(ref output, ref tagBuilder, inputTagType);

         if (MaxLength > 0)
            AddAttribute(output, tagBuilder, "maxlength", MaxLength);

         if (!UseTextCase)
            AddAttribute(output, tagBuilder, "class", "form-control form-control-sm");
         else
            AddAttribute(output, tagBuilder, "class", "form-control form-control-sm app-text-transform");

         if (For != null)
            Value = (For.Model == null ? string.Empty : For.Model.ToString());

         AddAttribute(output, tagBuilder, "value", Value);

         if (tagBuilder != null)
            output.Content.AppendHtml(tagBuilder);

         if (!string.IsNullOrEmpty(Label))
         {
            TagBuilder tagLabel = CreateTagLabel();

            if (GenerateLabelId)
               tagLabel.Attributes.Add("id", string.Concat(Id,"Label"));

            output.PreElement.AppendHtml(tagLabel);
         }
      }

   }
}
