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
using Android.Util;
using Android.Views.Animations;

//using PullToRefresharp.Android.Views;
//using PullToRefresharp.Android.Delegates;
//using PullToRefresharp.SwipeMenuList;
using Android.Support.V4.View;
using Android.Graphics.Drawables;

namespace Cooksts.Droid.SWipeMenuList
{
    public class SwipeListView : global::Android.Widget.ListView
    {

        private const int TOUCH_STATE_NONE = 0;
        private const int TOUCH_STATE_X = 1;
        private const int TOUCH_STATE_Y = 2;

        private int MAX_Y = 5;
        private int MAX_X = 3;
        private float mDownX;
        private float mDownY;
        private int mTouchState;
        private int mTouchPosition;

        private SwipeMenuLayout mTouchView;
        private IOnSwipeListener mOnSwipeListener;

        private ISwipeMenuCreator mMenuCreator;
        private IOnMenuItemClickListener mOnMenuItemClickListner;

        public IInterpolator CloseInterpolator { get; set; }
        public IInterpolator OpenInterpolator { get; set; }

        //ListViewDelegate ptr_delegate;

        public SwipeListView(Context context) : base(context)
        {
            MAX_X = Dp2Px(MAX_X);
            MAX_Y = Dp2Px(MAX_Y);
            mTouchState = TOUCH_STATE_NONE;
            this.mMenuCreator = new LvSwipeMenuCreator();
        }

        public SwipeListView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            MAX_X = Dp2Px(MAX_X);
            MAX_Y = Dp2Px(MAX_Y);
            mTouchState = TOUCH_STATE_NONE;
            this.mMenuCreator = new LvSwipeMenuCreator();
        }

        public SwipeListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            MAX_X = Dp2Px(MAX_X);
            MAX_Y = Dp2Px(MAX_Y);
            mTouchState = TOUCH_STATE_NONE;
            this.mMenuCreator = new LvSwipeMenuCreator();
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return base.OnInterceptTouchEvent(ev);
        }

        #region Touch Handling

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (!(e.Action != MotionEventActions.Down && mTouchView == null))
            {
                switch (e.Action)
                {
                    case MotionEventActions.Down:
                        {
                            int oldPos = mTouchPosition;
                            mDownX = e.RawX;
                            mDownY = e.RawY;
                            mTouchState = TOUCH_STATE_NONE;

                            mTouchPosition = PointToPosition((int)e.GetX(), (int)e.GetY());

                            if (mTouchPosition == oldPos && mTouchView != null
                                  && mTouchView.IsOpen())
                            {
                                mTouchState = TOUCH_STATE_X;
                                mTouchView.OnSwipe(e);
                                return true;
                            }

                            View view = GetChildAt(mTouchPosition - FirstVisiblePosition);

                            if (mTouchView != null && mTouchView.IsOpen())
                            {
                                mTouchView.SmoothCloseMenu();
                                mTouchView = null;

                                MotionEvent cancelEvent = MotionEvent.Obtain(e);
                                cancelEvent.Action = MotionEventActions.Cancel;
                                OnTouchEvent(cancelEvent);
                                return true;
                            }
                            if (view is SwipeMenuLayout)
                            {
                                mTouchView = (SwipeMenuLayout)view;
                            }
                            if (mTouchView != null)
                            {
                                mTouchView.OnSwipe(e);
                            }
                        }
                        break;
                    case MotionEventActions.Move:
                        {
                            float dy = Math.Abs(e.RawY - mDownY);
                            float dx = Math.Abs(e.RawX - mDownX);
                            if (mTouchState == TOUCH_STATE_X)
                            {
                                if (mTouchView != null)
                                {
                                    mTouchView.OnSwipe(e);
                                }
                                Selector.SetState(new int[] { 0 });
                                e.Action = MotionEventActions.Cancel;
                                base.OnTouchEvent(e);
                                return true;
                            }
                            else if (mTouchState == TOUCH_STATE_NONE)
                            {
                                if (Math.Abs(dy) > MAX_Y)
                                {
                                    mTouchState = TOUCH_STATE_Y;
                                }
                                else if (dx > MAX_X)
                                {
                                    mTouchState = TOUCH_STATE_X;
                                    if (mOnSwipeListener != null)
                                    {
                                        mOnSwipeListener.OnSwipeStart(mTouchPosition);
                                    }
                                }
                            }
                        }
                        break;
                    case MotionEventActions.Up:
                        {
                            if (mTouchState == TOUCH_STATE_X)
                            {
                                mTouchView.OnSwipe(e);
                                if (!mTouchView.IsOpen())
                                {
                                    mTouchPosition = -1;
                                    mTouchView = null;
                                }
                                if (mOnSwipeListener != null)
                                {
                                    mOnSwipeListener.OnSwipeEnd(mTouchPosition);
                                }
                                e.Action = MotionEventActions.Cancel;
                                base.OnTouchEvent(e);
                                return true;
                            }
                        }
                        break;
                }
            }
            //if (mTouchState != TOUCH_STATE_X)
            //{
            //    return (Parent as ViewWrapper).OnTouchEvent(e) || IgnoreTouchEvents || base.OnTouchEvent(e);
            //}
            //else
            //{
                return base.OnTouchEvent(e);
            //}
        }

        #endregion

        public void ForceHandleTouchEvent(MotionEvent e)
        {
            base.OnTouchEvent(e);
        }

        private int Dp2Px(int dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp,
                    Context.Resources.DisplayMetrics);
        }

        //public bool IgnoreTouchEvents
        //{
        //    get
        //    {
        //        return ptr_delegate.IgnoreTouchEvents;
        //    }
        //    set
        //    {
        //        ptr_delegate.IgnoreTouchEvents = value;
        //    }
        //}

        public void SetOnSwipeListener(IOnSwipeListener onSwipeListener)
        {
            this.mOnSwipeListener = onSwipeListener;
        }

        public void SetMenuCreator(ISwipeMenuCreator menuCreator)
        {
            this.mMenuCreator = menuCreator;
        }

    }

    /// <summary>
    /// 定义滑动窗口
    /// </summary>
    public class LvSwipeMenuCreator : ISwipeMenuCreator
    {
        public void Create(SwipeMenu menu)
        {
            SwipeMenuItem openItem = new SwipeMenuItem(MainActivity.mainActivity);
            openItem.Background = new ColorDrawable(Android.Graphics.Color.Green);
            openItem.Title = "打开";
            openItem.TitleSize = 20;
            openItem.Width = 100;
            openItem.TitleColor = Android.Graphics.Color.White;

            SwipeMenuItem deleteItem = new SwipeMenuItem(MainActivity.mainActivity);
            deleteItem.Background = new ColorDrawable(Android.Graphics.Color.Gray);
            deleteItem.Title = "删除";
            deleteItem.TitleSize = 20;
            deleteItem.Width = 100;
            deleteItem.TitleColor = Android.Graphics.Color.Black;

            menu.AddMenuItem(openItem);
            menu.AddMenuItem(deleteItem);
        }
    }

}