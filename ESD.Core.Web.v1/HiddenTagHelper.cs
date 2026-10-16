using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ESD.Core.Web
{
   [HtmlTargetElement("esd-hidden")]
   public class HiddenTagHelper: InputTagHelper
   {

      [HtmlAttributeName("value")]
      public string Value { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {

         if (For != null)
            Id = ResolveId(For.Name);
         else
            Id = ResolveId(Id);

         output.TagName = HtmlTagNameConstant.INPUT;
         output.Attributes.Add("id", Id);
         output.Attributes.Add("name", Id);
         output.Attributes.Add("type", InputTagType.Hidden.ToString().ToLower());

         if (For != null)
            Value = (For.Model == null ? string.Empty : For.Model.ToString());

         output.Attributes.Add("value", Value);
      }

   }
}
