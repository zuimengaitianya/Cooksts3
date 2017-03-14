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
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Cooksts.Droid.SWipeMenuList
{
    public class SwipeMenuItem
    {
        private Context mContext;

        public String ID { get; set; }
        public String Title { get; set; }
        public Color TitleColor { get; set; }
        public int TitleSize { get; set; }
        public Drawable Icon { get; set; }
        public Drawable Background { get; set; }
        public int Width { get; set; }

        public SwipeMenuItem(Context context)
        {
            mContext = context;
        }

        public void SetTitle(int resId)
        {
            Title = mContext.GetString(resId);
        }

        public void SetIcon(int resId)
        {
            Icon = mContext.Resources.GetDrawable(resId);
        }

        public void SetBackground(int resId)
        {
            Background = mContext.Resources.GetDrawable(resId);
        }
    }
}