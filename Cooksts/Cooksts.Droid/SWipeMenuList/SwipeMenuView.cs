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
    public class SwipeMenuView : LinearLayout, ViewGroup.IOnClickListener
    {
        private Cooksts.Droid.SWipeMenuList.SwipeListView mListView;
        private SwipeMenu mMenu;

        public SwipeMenuLayout Layout { get; set; }
        public IOnSwipeItemClickListener ItemClickListener { get; set; }
        public int Position { get; set; }

        public SwipeMenuView(SwipeMenu menu, Cooksts.Droid.SWipeMenuList.SwipeListView listView)
            : base(menu.Context)
        {
            mListView = listView;
            mMenu = menu;
            int id = 0;
            foreach (SwipeMenuItem item in menu.GetMenuItems())
            {
                AddItem(item, id++);
            }
        }

        private void AddItem(SwipeMenuItem item, int id)
        {
            LayoutParams param = new LayoutParams(item.Width, LayoutParams.MatchParent);
            LinearLayout parent = new LinearLayout(Context);
            parent.Id = id;
            parent.SetGravity(GravityFlags.Center);
            parent.Orientation = global::Android.Widget.Orientation.Vertical;
            parent.LayoutParameters = param;
            parent.SetBackgroundDrawable(item.Background);
            parent.SetOnClickListener(this);
            AddView(parent);

            if (item.Icon != null)
            {
                parent.AddView(CreateIcon(item));
            }

            if (!String.IsNullOrEmpty(item.Title))
            {
                parent.AddView(CreateTitle(item));
            }
        }

        private ImageView CreateIcon(SwipeMenuItem item)
        {
            ImageView iv = new ImageView(Context);
            iv.SetImageDrawable(item.Icon);
            return iv;
        }

        private TextView CreateTitle(SwipeMenuItem item)
        {
            TextView tv = new TextView(Context);
            tv.Text = item.Title;
            tv.Gravity = GravityFlags.Center;
            tv.TextSize = item.TitleSize;
            tv.SetTextColor(item.TitleColor);
            return tv;
        }

        #region IOnClickListener

        public void OnClick(View v)
        {
            if (ItemClickListener != null && Layout.IsOpen())
            {
                ItemClickListener.OnItemClick(this, mMenu, v.Id);
            }
        }

        #endregion
    }
}