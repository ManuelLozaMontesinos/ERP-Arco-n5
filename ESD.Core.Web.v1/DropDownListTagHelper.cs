using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text;

namespace ESD.Core.Web
{
    [HtmlTargetElement("esd-dropdownlist")]
    public class DropDownListTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("col-position")]
        public string ColumnPosition { get; set; }

        [HtmlAttributeName("label")]
        public string Label { get; set; }

        [HtmlAttributeName("col-label-position")]
        public string ColumnLabelPosition { get; set; }

        [HtmlAttributeName("selected-value")]
        public string SelectedValue { get; set; }

        [HtmlAttributeName("onchange")]
        public string OnChange { get; set; }

        [HtmlAttributeName("multiple")]
        public bool Multiple { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder selectTag = new TagBuilder(HtmlTagNameConstant.SELECT);

            Id = ResolveId(Id);

            selectTag.GenerateId(Id, string.Empty);
            selectTag.Attributes.Add("name", Id);
            selectTag.AddCssClass("form-control form-control-sm");

            if (!Enable)
                selectTag.Attributes.Add("disabled", "disabled");

            if (!string.IsNullOrEmpty(OnChange))
                selectTag.Attributes.Add("onchange", resolveOnChange(OnChange));

            if (Multiple)
                selectTag.Attributes.Add("multiple", "multiple");

            if (For != null)
            {
                if (For.Model != null)
                {
                    StringBuilder sb = new StringBuilder();
                    List<SelectListItem> selectList = (List<SelectListItem>)For.Model;

                    foreach (SelectListItem selectListItem in selectList)
                    {
                        sb.Append(string.Concat("<option value=\"", selectListItem.Value, "\" "));
                        if (!string.IsNullOrEmpty(SelectedValue))
                            sb.Append(string.Concat((selectListItem.Value == SelectedValue ? "selected=\"selected\"" : string.Empty)));
                        else
                            sb.Append(string.Concat((selectListItem.Selected ? "selected=\"selected\"" : string.Empty)));

                        sb.Append(string.Concat(">", selectListItem.Text, "</option>"));
                    }

                    selectTag.InnerHtml.AppendHtml(sb.ToString());
                }
            }

            TagBuilder labelTag = null;
            if (!string.IsNullOrEmpty(Label))
            {
                labelTag = new TagBuilder(HtmlTagNameConstant.LABEL);
                labelTag.MergeAttribute("for", Id);
                labelTag.AddCssClass("col-form-label");
                labelTag.AddCssClass(ColumnLabelPosition);
                labelTag.InnerHtml.AppendHtml(Label);
            }

            output.TagName = HtmlTagNameConstant.DIV;
            output.Attributes.Add("class", ColumnPosition);
            output.Content.AppendHtml(selectTag);

            if (labelTag != null)
                output.PreElement.AppendHtml(labelTag);
        }

        private string resolveOnChange(string value)
        {
            if (value.IndexOf(")") == -1)
                value += "()";

            if (value.IndexOf(";") == -1)
                value += ";";

            return value;
        }
    }
}
