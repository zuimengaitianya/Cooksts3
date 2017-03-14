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
    public class SwipeMenuAdapter : BaseAdapter,
        IOnSwipeItemClickListener
    {
        private IListAdapter mAdapter;
        private Context mContext;

        private IOnMenuItemClickListener onMenuItemClickListner;
        private Action<SwipeMenuView, SwipeMenu, int> onMenuItemClickDelegate;

        private ISwipeMenuCreator mSwipeMenuCreator;
        private Action<SwipeMenu> mSwipeMenuCreatorDelegate;

        public SwipeMenuAdapter(Context context, IListAdapter adapter)
        {
            mAdapter = adapter;
            mContext = context;
        }

        #region IWrapperListAdapter

        public override bool AreAllItemsEnabled()
        {
            return mAdapter.AreAllItemsEnabled();
        }

        public override bool IsEnabled(int position)
        {
            return mAdapter.IsEnabled(position);
        }

        public override int Count
        {
            get
            {
                return mAdapter.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return mAdapter.GetItem(position);
        }

        public override long GetItemId(int position)
        {
            return mAdapter.GetItemId(position);
        }

        public override int GetItemViewType(int position)
        {
            return mAdapter.GetItemViewType(position);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            SwipeMenuLayout layout = null;
            if (convertView == null)
            {
                View contentView = mAdapter.GetView(position, convertView, parent);
                SwipeMenu menu = new SwipeMenu(mContext);
                menu.ViewType = mAdapter.GetItemViewType(position);
                CreateMenu(menu);
                Cooksts.Droid.SWipeMenuList.SwipeListView listview = (Cooksts.Droid.SWipeMenuList.SwipeListView)parent;
                SwipeMenuView menuView = new SwipeMenuView(menu, listview);
                menuView.ItemClickListener = this;
                layout = new SwipeMenuLayout(contentView, menuView,
                    listview.CloseInterpolator,
                    listview.OpenInterpolator);
                layout.Position = position;
            }
            else
            {
                layout = (SwipeMenuLayout)convertView;
                layout.CloseMenu();
                layout.Position = position;
                View view = mAdapter.GetView(position, layout.ContentView, parent);
            }
            return layout;
        }

        public override bool HasStableIds
        {
            get
            {
                return mAdapter.HasStableIds;
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return mAdapter.IsEmpty;
            }
        }

        public override void RegisterDataSetObserver(global::Android.Database.DataSetObserver observer)
        {
            mAdapter.RegisterDataSetObserver(observer);
        }

        public override void UnregisterDataSetObserver(global::Android.Database.DataSetObserver observer)
        {
            mAdapter.UnregisterDataSetObserver(observer);
        }

        public override int ViewTypeCount
        {
            get
            {
                return mAdapter.ViewTypeCount;
            }
        }

        #endregion

        #region IOnSwipeItemClickListener

        public void OnItemClick(SwipeMenuView view, SwipeMenu menu, int index)
        {
            if (onMenuItemClickDelegate != null)
            {
                onMenuItemClickDelegate(view, menu, index);
            }
            if (onMenuItemClickListner != null)
            {
                onMenuItemClickListner.OnMenuItemClick(view.Position,
                    menu, index);
            }
        }

        #endregion

        public void SetOnMenuItemClickListener(IOnMenuItemClickListener onMenuItemClickListener)
        {
            this.onMenuItemClickListner = onMenuItemClickListener;
        }

        public void SetOnMenuItemClickDelegate(Action<SwipeMenuView, SwipeMenu, int> omic)
        {
            this.onMenuItemClickDelegate = omic;
        }

        public void SetSwipeMenuCreator(ISwipeMenuCreator creator)
        {
            this.mSwipeMenuCreator = creator;
        }

        public void SetSwipeMenuCreator(Action<SwipeMenu> creator)
        {
            this.mSwipeMenuCreatorDelegate = creator;
        }

        public void CreateMenu(SwipeMenu menu)
        {
            if (mSwipeMenuCreator != null)
            {
                mSwipeMenuCreator.Create(menu);
            }
            if (mSwipeMenuCreatorDelegate != null)
            {
                mSwipeMenuCreatorDelegate(menu);
            }
        }
    }
}