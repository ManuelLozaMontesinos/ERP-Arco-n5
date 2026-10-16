using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ESD.Core.Web
{
   [HtmlTargetElement("esd-viewmodelstate")]
   public class ViewModelStateTagHelper : InputTagHelper
   {

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {

         Id = "__vms";

         output.TagName = HtmlTagNameConstant.INPUT;
         output.Attributes.Add("id", Id);
         output.Attributes.Add("name", Id);
         output.Attributes.Add("type", InputTagType.Hidden.ToString().ToLower());

         output.Attributes.Add("value", For.Model == null ? string.Empty : For.Model.ToString());
      }

   }
}
