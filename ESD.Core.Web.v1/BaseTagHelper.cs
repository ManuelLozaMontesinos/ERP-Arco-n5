using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ESD.Core.Web
{

   [HtmlTargetElement("internal-base-tag")]
   public class BaseTagHelper : TagHelper
   {
      [HtmlAttributeName("id")]
      public string Id { get; set; }

      [HtmlAttributeName("enable")]
      public bool Enable { get; set; } = true;

      [HtmlAttributeName("for")]
      public ModelExpression For { get; set; }

      [HtmlAttributeName("in-modal")]
      public bool InModal { get; set; } = false;

      public void AddAttribute(TagHelperOutput tagHelperOutput, TagBuilder tagBuilder, string key, object value) {
         if (tagBuilder == null)
            tagHelperOutput.Attributes.Add(key, value);
         else
            tagBuilder.Attributes.Add(key, value != null ? value.ToString() : string.Empty);
      }
      public string ResolveId(string value)
      {

         int index = value.LastIndexOf(SeparadorConstante.PUNTO);

         string newId = (index == -1 ? value : value.Substring(index + 1));

         return (InModal ? string.Concat("mp-",newId) : newId);
      }
      public TagBuilder CreateScriptTag()
      {
         TagBuilder tagBuilder = new TagBuilder("script");
         tagBuilder.Attributes.Add("type", "text/javascript");

         return tagBuilder;
      }
   }
}
