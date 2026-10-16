using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ESD.Core.Web
{
   [HtmlTargetElement("esd-textarea")]
   public class TextAreaTagHelper: InputTagHelper
   {

      [HtmlAttributeName("max-length")]
      public int MaxLength { get; set; }

      [HtmlAttributeName("rows")]
      public int Rows { get; set; } = 2;

      [HtmlAttributeName("use-textcase")]
      public bool UseTextCase { get; set; } = true;

      [HtmlAttributeName("generate-label-id")]
      public bool GenerateLabelId { get; set; } = false;

      [HtmlAttributeName("value")]
      public string Value { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {

         TagBuilder tagBuilder = null;
         InputTagType? inputTagType = null;
         if (string.IsNullOrEmpty(ColumnPosition))
         {
            output.TagName = HtmlTagNameConstant.TEXTAREA;
         }
         else {
            output.TagName = HtmlTagNameConstant.DIV;
            output.Attributes.Add("class", ColumnPosition);
            inputTagType = InputTagType.TextArea;
         }

         InitializeTag(ref output, ref tagBuilder, inputTagType);

         if (MaxLength > 0)
            AddAttribute(output, tagBuilder, "maxlength", MaxLength);

         AddAttribute(output, tagBuilder, "rows", Rows);

         if (!UseTextCase)
            AddAttribute(output, tagBuilder, "class", "form-control form-control-sm");
         else
            AddAttribute(output, tagBuilder, "class", "form-control form-control-sm app-text-transform");

         if (For != null)
            Value = (For.Model == null ? string.Empty : For.Model.ToString());

         if (string.IsNullOrEmpty(Value))
            Value = string.Empty;

         if(tagBuilder != null)
            tagBuilder.InnerHtml.Append(Value);
         else
            output.Content.AppendHtml(Value);

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
