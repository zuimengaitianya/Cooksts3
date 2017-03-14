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
    public interface IOnSwipeItemClickListener
    {
        void OnItemClick(SwipeMenuView view, SwipeMenu menu, int index);
    }
}