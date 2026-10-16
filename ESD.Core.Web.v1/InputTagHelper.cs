using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ESD.Core.Web
{
   public enum InputTagType
   {
      Text = 0,
      Password = 1,
      Checkbox = 2,
      Radio = 3,
      Submit = 4,
      Image = 5,
      Reset = 6,
      Button = 7,
      Hidden = 8,
      File = 9,
      TextArea = 10
   }

   [HtmlTargetElement("input-base-tag")]
   public class InputTagHelper: BaseTagHelper
   {

      [HtmlAttributeName("col-position")]
      public string ColumnPosition { get; set; }

      [HtmlAttributeName("label")]
      public string Label { get; set; }

      [HtmlAttributeName("col-label-position")]
      public string ColumnLabelPosition { get; set; }

      [HtmlAttributeName("placeholder")]
      public string PlaceHolder { get; set; }

      [HtmlAttributeName("use-placeholder")]
      public bool UsePlaceHolder { get; set; } = true;

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {

      }

      public void InitializeTag(ref TagHelperOutput tagHelperOutput, ref TagBuilder tagBuilder, InputTagType? inputTagType) {

         if (For != null)
            Id = ResolveId(For.Name);
         else
            Id = ResolveId(Id);

         if (inputTagType != null)
         {
            if(inputTagType == InputTagType.TextArea)
               tagBuilder = new TagBuilder(HtmlTagNameConstant.TEXTAREA);
            else
               tagBuilder = new TagBuilder(HtmlTagNameConstant.INPUT);
            tagBuilder.Attributes.Add("type", inputTagType.ToString().ToLower());

            tagBuilder.GenerateId(Id, string.Empty);
            tagBuilder.Attributes.Add("name", Id);
         }
         else
         {
            tagHelperOutput.Attributes.Add("id", Id);
            tagHelperOutput.Attributes.Add("name", Id);
         }

         if (!Enable)
            AddAttribute(tagHelperOutput, tagBuilder, "readonly", "readonly");

         if (UsePlaceHolder)
         {

            if (string.IsNullOrEmpty(PlaceHolder))
               PlaceHolder = Label;

            if (!string.IsNullOrEmpty(PlaceHolder))
               AddAttribute(tagHelperOutput, tagBuilder, "placeholder", PlaceHolder);
         }

         //AddAttribute(tagHelperOutput, tagBuilder, "class", "form-control form-control-sm");
      }

      public TagBuilder CreateTagLabel()
      {
         TagBuilder tagBuilder = new(HtmlTagNameConstant.LABEL);

         tagBuilder.MergeAttribute("for", Id);
         tagBuilder.AddCssClass("col-form-label");
         tagBuilder.AddCssClass(ColumnLabelPosition);
         tagBuilder.InnerHtml.AppendHtml(Label);

         return tagBuilder;
      }
   }
}
