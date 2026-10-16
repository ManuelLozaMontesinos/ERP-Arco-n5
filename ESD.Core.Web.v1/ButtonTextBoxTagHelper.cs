using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ESD.Core.Web
{
   public enum ButtonTextBoxItemStyle
   {
      List = 0,
      ListNew = 1
   }
   public enum ButtonTextBoxItemType
   {
      List = 0,
      New = 1
   }
   public class ButtonTextBoxItemKey
   {
      public const string LIST = "list";
      public const string NEW = "new";
   }
   public class ButtonTextBoxItem
   {
      public ButtonTextBoxItemType ButtonTextBoxItemType { get; set; }
      public string Key { get; set; }
      public string FontImage { get; set; }
   }

   [HtmlTargetElement("esd-buttontextbox")]
   public class ButtonTextBoxTagHelper : InputTagHelper
   {

      [HtmlAttributeName("onclick")]
      public string OnClick { get; set; }

      [HtmlAttributeName("allow-edit")]
      public bool AllowEdit { get; set; }

      [HtmlAttributeName("value")]
      public string Value { get; set; }

      [HtmlAttributeName("button-style")]
      public ButtonTextBoxItemStyle ButtonTextBoxStyle { get; set; } = ButtonTextBoxItemStyle.List;

      [HtmlAttributeNotBound]
      private List<ButtonTextBoxItem> ButtonTextBoxItemList { get; set; } = new List<ButtonTextBoxItem>();

      private Dictionary<string, ButtonTextBoxItem> DefaultItemsList { get; set; }
      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         createDefaultItemsList();

         Id = ResolveId(Id);

         TagBuilder tagDiv = new TagBuilder(HtmlTagNameConstant.DIV);
         tagDiv.AddCssClass("input-group");

         TagBuilder tagText = new TagBuilder(HtmlTagNameConstant.INPUT);
         tagText.GenerateId(Id,string.Empty);
         tagText.Attributes.Add("type", InputTagType.Text.ToString().ToLower());
         tagText.Attributes.Add("name", Id);
         tagText.AddCssClass("form-control form-control-sm app-text-transform");

         if (!string.IsNullOrEmpty(Value))
            tagText.Attributes.Add("value", Value);

         switch (ButtonTextBoxStyle)
         {
            case ButtonTextBoxItemStyle.List:
            case ButtonTextBoxItemStyle.ListNew:
               ButtonTextBoxItemList = new List<ButtonTextBoxItem>() {
                  DefaultItemsList[ButtonTextBoxItemType.List.ToString()]
               };

               switch (ButtonTextBoxStyle)
               {
                  case ButtonTextBoxItemStyle.ListNew:
                     ButtonTextBoxItemList.Add(DefaultItemsList[ButtonTextBoxItemType.New.ToString()]);
                     break;
               }
               break;
         }

         if(!AllowEdit)
            tagText.Attributes.Add("readonly", "readonly");

         resolveOnClick();

         TagBuilder tagButton = new TagBuilder(HtmlTagNameConstant.DIV);
         tagButton.Attributes.Add("class", "input-group-append");
         tagButton.InnerHtml.AppendHtml(createButtonItem());

         tagText.InnerHtml.AppendHtml(tagButton);

         tagDiv.InnerHtml.AppendHtml(tagText);

         output.TagName = HtmlTagNameConstant.DIV;

         if (!string.IsNullOrEmpty(ColumnPosition))
            output.Attributes.Add("class", ColumnPosition);

         output.Content.AppendHtml(tagDiv);

         if (!string.IsNullOrEmpty(Label))
         {
            TagBuilder tagLabel = CreateTagLabel();
            output.PreElement.AppendHtml(tagLabel);
         }

      }
      private string createButtonItem()
      {
         StringBuilder sb = new StringBuilder();

         foreach (ButtonTextBoxItem buttonTextBoxItem in ButtonTextBoxItemList) {
            TagBuilder tagButton = new TagBuilder(HtmlTagNameConstant.BUTTON);
            tagButton.Attributes.Add("type", HtmlTagNameConstant.BUTTON);
            tagButton.AddCssClass("btn btn-outline-secondary " + buttonTextBoxItem.FontImage + " input-group-addon app-input-group-button");
            tagButton.GenerateId(string.Concat(Id, "_button_", buttonTextBoxItem.Key),string.Empty);

            if (!string.IsNullOrEmpty(OnClick)) {
               if (ButtonTextBoxItemList.Count == 1)
                  tagButton.Attributes.Add("onclick", string.Concat(OnClick,"()"));
               else
                  tagButton.Attributes.Add("onclick", string.Concat(OnClick,"('",buttonTextBoxItem.Key,"')"));
            }

            if (!Enable)
               tagButton.Attributes.Add("disabled", "true");

            using (var writer = new System.IO.StringWriter())
            {
               tagButton.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
               sb.Append(writer.ToString());
            }
         }

         return sb.ToString();
      }
      private void createDefaultItemsList()
      {

         string prefixFontImage = "far fa-";
         DefaultItemsList = new Dictionary<string, ButtonTextBoxItem>
         {
            {
               ButtonTextBoxItemType.List.ToString(),
               new ButtonTextBoxItem()
               {
                  ButtonTextBoxItemType = ButtonTextBoxItemType.List,
                  Key = ButtonTextBoxItemKey.LIST,
                  FontImage = prefixFontImage + "list-alt"
               }
            },
            {
               ButtonTextBoxItemType.New.ToString(),
               new ButtonTextBoxItem()
               {
                  ButtonTextBoxItemType = ButtonTextBoxItemType.New,
                  Key = ButtonTextBoxItemKey.NEW,
                  FontImage = prefixFontImage + "file"
               }
            }
         };
      }
      private void resolveOnClick() {
         if (string.IsNullOrEmpty(OnClick))
            return;

         OnClick.Replace("(",string.Empty);
         OnClick.Replace(")", string.Empty);
      }
   }
}
