using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Cooksts.Droid.SWipeMenuList
{
    public class SwipeMenu
    {
        private List<SwipeMenuItem> mItems;

        public Context Context { get; private set; }
        public int ViewType { get; set; }

        public SwipeMenu(Context context)
        {
            Context = context;
            mItems = new List<SwipeMenuItem>();
        }

        public void AddMenuItem(SwipeMenuItem item)
        {
            mItems.Add(item);
        }

        public void RemoveMenuItem(SwipeMenuItem item)
        {
            mItems.Remove(item);
        }

        public List<SwipeMenuItem> GetMenuItems()
        {
            return mItems;
        }

        public SwipeMenuItem GetMenuItemAt(int index)
        {
            return mItems[index];
        }
    }
}