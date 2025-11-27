using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace GreenerConfigurator.Models
{
    public class TreeModel : ItemsControl
    {
        public TreeModel()
        {
           //Items = new List<TreeModel>();
        }

        public string Header{ get; set; }

        public object ItemObject { get; set; }

        //public IList<TreeModel> Items { get; set; }

    }

}
