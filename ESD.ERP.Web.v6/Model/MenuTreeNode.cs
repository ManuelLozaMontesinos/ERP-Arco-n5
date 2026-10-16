using System.Collections.Generic;

namespace ESD.ERP.Web.Model
{
   public class MenuTreeNode
   {
      public string Text { get; set; }
      public string Icon { get; set; }
      public string SelectedIcon { get; set; }
      public string Url { get; set; }
      public bool Selectable { get; set; } 
      public List<MenuTreeNode> Nodes { get; set;} = new List<MenuTreeNode>();
   }
}