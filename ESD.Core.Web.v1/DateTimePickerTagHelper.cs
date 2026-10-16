using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;
using System.Threading;

namespace ESD.Core.Web
{
   public enum DateTimePickerType
   {
      Date = 0,
      Time = 1,
      DateTime = 3
   }

   [HtmlTargetElement("esd-datetimepicker")]
   public class DateTimePickerTagHelper: InputTagHelper
   {

      [HtmlAttributeName("datetimepicker-type")]
      public DateTimePickerType DateTimePickerType { get; set; } = DateTimePickerType.Date;

      [HtmlAttributeName("min-date")]
      public DateTime? MinDate { get; set; }

      [HtmlAttributeName("max-date")]
      public DateTime? MaxDate { get; set; }

      [HtmlAttributeName("format")]
      public string Format { get; set; }
      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         DateTime? value = null;
         string internalFormat = string.Empty;

         if (For != null)
            Id = ResolveId(For.Name);
         else
            Id = ResolveId(Id);

         TagBuilder tagDateDiv = new TagBuilder(HtmlTagNameConstant.DIV);
         string dateDivId = string.Concat(Id, "-ddiv");
         tagDateDiv.Attributes.Add("id", dateDivId);
         tagDateDiv.Attributes.Add("name", dateDivId);
         tagDateDiv.Attributes.Add("data-target-input", "nearest");
         tagDateDiv.AddCssClass("input-group input-group-sm date");

         TagBuilder tagText = new TagBuilder(HtmlTagNameConstant.INPUT);
         tagText.GenerateId(Id,string.Empty);
         tagText.Attributes.Add("type", InputTagType.Text.ToString().ToLower());
         tagText.Attributes.Add("name", Id);
         tagText.Attributes.Add("data-target", string.Concat("#", dateDivId));

         tagText.AddCssClass("form-control form-control-sm datetimepicker-input");

         if (For != null)
            value = (DateTime?)For.Model;

         if (!Enable)
            tagText.Attributes.Add("readonly", "readonly");

         if (string.IsNullOrEmpty(Format)) {
            
            string shortDatePattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;

            switch (DateTimePickerType)
            {
               case DateTimePickerType.DateTime:
                  Format = string.Concat(shortDatePattern.ToUpper(), " HH:mm");
                  internalFormat = string.Concat(shortDatePattern, " HH:mm");
                  break;

               case DateTimePickerType.Time:
                  Format = "HH:mm:ss";
                  break;

               default:
                  Format = shortDatePattern.ToUpper();
                  internalFormat = shortDatePattern;
                  break;
            }
         }

         if (value.HasValue)
            tagText.Attributes.Add("value", value.Value.ToString(internalFormat));

         TagBuilder tagButton = new TagBuilder(HtmlTagNameConstant.DIV);
         tagButton.Attributes.Add("class", "input-group-append");
         tagButton.Attributes.Add("data-target", string.Concat("#", dateDivId));
         tagButton.Attributes.Add("data-toggle", "datetimepicker");
         tagButton.InnerHtml.AppendHtml("<div class=\"input-group-text\"><i class=\"far fa-calendar-alt\"></i></div>");

         tagDateDiv.InnerHtml.AppendHtml(tagText);
         tagDateDiv.InnerHtml.AppendHtml(tagButton);

         output.TagName = HtmlTagNameConstant.DIV;

         if (!string.IsNullOrEmpty(ColumnPosition))
            output.Attributes.Add("class", ColumnPosition);

         output.Content.AppendHtml(tagDateDiv);

         if (!string.IsNullOrEmpty(Label))
         {
            TagBuilder tagLabel = CreateTagLabel();
            output.PreElement.AppendHtml(tagLabel);
         }

         StringBuilder sb = new StringBuilder();

         sb.Append("$(function(){$('#" + dateDivId + "').datetimepicker({");

         if (MinDate != null)
            sb.Append("startDate : new Date(" + MinDate.Value.Year.ToString() + ", " + MinDate.Value.Month.ToString() + ", " +
               MinDate.Value.Day.ToString() + ", " + MinDate.Value.Hour.ToString() + ", " + MinDate.Value.Minute.ToString() + ") ,");

         if (MaxDate != null)
            sb.Append("startDate : new Date(" + MaxDate.Value.Year.ToString() + ", " + MaxDate.Value.Month.ToString() + ", " +
               MaxDate.Value.Day.ToString() + ", " + MaxDate.Value.Hour.ToString() + ", " + MaxDate.Value.Minute.ToString() + ") ,");

         sb.Append(" locale : 'es', format : '" + Format + "'");
         sb.Append("});");

         if (!Enable)
            sb.Append("$('#" + Id + "').prop('readonly', true);");

         sb.Append("});");

         TagBuilder tagScript = CreateScriptTag();
         tagScript.InnerHtml.AppendHtml(sb.ToString());

         output.PostElement.AppendHtml(tagScript);

      }

   }
}
