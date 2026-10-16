using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace ESD.Core.Web
{
   public enum NumericType
   {
      Integer = 0,
      Decimal = 1
   }

   [HtmlTargetElement("esd-numerictextbox")]
   public class NumericTextBoxTagHelper : BaseTagHelper
   {

      [HtmlAttributeName("col-position")]
      public string ColumnPosition { get; set; }

      [HtmlAttributeName("label")]
      public string Label { get; set; }

      [HtmlAttributeName("col-label-position")]
      public string ColumnLabelPosition { get; set; }

      [HtmlAttributeName("numeric-type")]
      public NumericType NumericType { get; set; } = NumericType.Decimal;

      [HtmlAttributeName("allow-minus")]
      public bool AllowMinus { get; set; }

      [HtmlAttributeName("allow-plus")]
      public bool AllowPlus { get; set; }

      [HtmlAttributeName("decimal-places")]
      public int DecimalPlaces { get; set; }

      [HtmlAttributeName("integer-places")]
      public int IntegerPlaces { get; set; }

      [HtmlAttributeName("decimal-separator")]
      public string DecimalSeparator { get; set; }

      [HtmlAttributeName("group-separator")]
      public string GroupSeparator { get; set; }

      [HtmlAttributeName("value")]
      public object Value { get; set; }

      [HtmlAttributeName("generate-label-id")]
      public bool GenerateLabelId { get; set; } = false;

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {

         if (For != null)
            Id = ResolveId(For.Name);
         else
            Id = ResolveId(Id);

         TagBuilder tagInput = new TagBuilder(HtmlTagNameConstant.INPUT);
         tagInput.Attributes.Add("type", InputTagType.Text.ToString().ToLower());

         tagInput.GenerateId(Id, string.Empty);
         tagInput.Attributes.Add("name", Id);
         tagInput.Attributes.Add("class", "form-control form-control-sm");

         if (For != null) {
            Value = (For.Model ?? 0);
            if (NumericType == NumericType.Decimal)
               Value = Convert.ToDecimal(Value);
            else
               Value = Convert.ToInt32(Value);
         }

         if (Value == null) {
            if (NumericType == NumericType.Decimal)
               Value = decimal.Zero;
            else
               Value = int.Parse("0");
         }

         tagInput.Attributes.Add("value", Value.ToString());

         if (!Enable)
            tagInput.Attributes.Add("readonly", "readonly");

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

            if (GenerateLabelId)
               tagLabel.Attributes.Add("id", string.Concat(Id, "Label"));

            output.PreElement.AppendHtml(tagLabel);
         }

         string numericName = string.Empty;
         if (NumericType == NumericType.Integer)
            numericName = "integer";
         else
            numericName = "decimal";

         StringBuilder sb = new StringBuilder();

         sb.Append(string.Format("$(\"#{0}\").inputmask(\"{1}\",", Id, numericName));
         sb.Append("{");
         sb.Append(string.Format("allowMinus: {0},", AllowMinus.ToString().ToLower()));
         sb.Append(string.Format("allowPlus: {0}", AllowMinus.ToString().ToLower()));

         if (NumericType == NumericType.Decimal)
         {
            sb.Append(",autoGroup: true");

            if(string.IsNullOrEmpty(DecimalSeparator))
               DecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if (string.IsNullOrEmpty(GroupSeparator))
               GroupSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator;

            sb.Append(string.Format(",radixPoint: \"{0}\"",DecimalSeparator));
            sb.Append(string.Format(",groupSeparator: \"{0}\"", GroupSeparator));
            sb.Append(",groupSize: 3");

            if (DecimalPlaces == 0)
               DecimalPlaces = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits;

            sb.Append(string.Format(",digits: \"{0}\"", DecimalPlaces));

         }

         if (IntegerPlaces > 0)
            sb.Append(string.Format(",integerDigits:  \"{0}\"", IntegerPlaces.ToString()));

         sb.Append("});");
         TagBuilder tagScript = CreateScriptTag();
         tagScript.InnerHtml.AppendHtml(sb.ToString());

         output.PostElement.AppendHtml(tagScript);

      }

   }
}
