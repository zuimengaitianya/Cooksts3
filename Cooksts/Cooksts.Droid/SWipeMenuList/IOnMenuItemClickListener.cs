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
    public interface IOnMenuItemClickListener
    {
        bool OnMenuItemClick(int position, SwipeMenu menu, int index);
    }
}