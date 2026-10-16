using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace ESD.Core.Web
{
   public enum ViewModelEditAction
   {
      NoAction = 0,
      New = 1,
      Edit = 2,
      Copy = 3,
      Delete = 4
   }
   public class ViewModel
   {
      private string title = string.Empty;
      public string Title
      {
         get => HttpUtility.HtmlDecode(title);
         set => title = value;
      }
      public string NavigationPath { get; set; }
      public bool IsNew { get; set; }
      public bool IsAdmin { get; set; }
      public List<MenuItem> MenuItemList { get; set; }
      public ViewModelState ViewModelState { get; set; } = new ViewModelState();
      public ViewModelEditAction EditAction
      {
         get => ViewModelState.EditAction;
         set => ViewModelState.EditAction = value;
      }
      public List<SelectedItem> SelectedItemList { get; set; }
      public bool GetSelectedItemDisabled(string key)
      {
         if (SelectedItemList == null)
            return false;

         SelectedItem selectedItem = SelectedItemList.Where(x => x.Name == key).FirstOrDefault();
         if (selectedItem == null)
            return false;

         return selectedItem.Disabled;
      }
      public string ModalEditId { get; set; }
      public string ToJson(object value)
      {
         return JsonSerializer.Serialize(value);
      }
   }
   public class ViewModelState
   {
      public ViewModelEditAction EditAction { get; set; } = ViewModelEditAction.NoAction;
      public string Operation { get; set; }
      public string CID { get; set; }
      public string Value
      {
         get { return ToString(); }
      }
      public override string ToString()
      {
         return HttpUtility.HtmlDecode(
            "{'EditAction':" + ((int)EditAction).ToString() + ","
            + "'Operation':'" + (string.IsNullOrEmpty(Operation) ? string.Empty : Operation)+"',"
            + "'CID':''"
            + "}");
      }
   }

   public enum SelectedItemType
   {
      [EnumMember(Value = "value")]
      Value = 0,
      [EnumMember(Value = "firstElement")]
      FirstElement = 1
   }
   public class SelectedItem
   {
      public string Name { get; set; }
      public bool Disabled { get; set; }
      public string Value { get; set; }
      [JsonConverter(typeof(JsonStringEnumConverter))]
      public SelectedItemType SelectedItemType { get; set; }
   }
}
