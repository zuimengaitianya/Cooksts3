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
using GestureDetectorCompat = Android.Support.V4.View.GestureDetectorCompat;
using IOnGestureDetector = Android.Views.GestureDetector.IOnGestureListener;
using SimpleOnGestureListener = Android.Views.GestureDetector.SimpleOnGestureListener;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views.Animations;

namespace Cooksts.Droid.SWipeMenuList
{
    public class SwipeMenuLayout : FrameLayout
    {
        private const int CONTENT_VIEW_ID = 1;
        private const int MENU_VIEW_ID = 2;

        private const int STATE_CLOSE = 0;
        private const int STATE_OPEN = 1;

        public View ContentView { get; private set; }
        public SwipeMenuView MenuView { get; private set; }
        public int Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
                MenuView.Position = value;
            }
        }

        private int mPosition;
        private int mDownX;
        private int state = STATE_CLOSE;
        private GestureDetectorCompat mGestureDetector;
        private IOnGestureDetector mGestureListener;
        private bool IsFling;
        private int MIN_FLING;
        private int MAX_VELOCITYX;
        private ScrollerCompat mOpenScroller;
        private ScrollerCompat mCloseScroller;
        private int mBaseX;
        private IInterpolator mCloseInterpolator;
        private IInterpolator mOpenInterpolator;

        public SwipeMenuLayout(View contentView, SwipeMenuView menuView)
            : this(contentView, menuView, null, null) { }

        public SwipeMenuLayout(View contentView, SwipeMenuView menuView,
            IInterpolator closeInterpolator, IInterpolator openInterpolator)
            : base(contentView.Context)
        {
            mCloseInterpolator = closeInterpolator;
            mOpenInterpolator = openInterpolator;
            ContentView = contentView;
            MenuView = menuView;
            MenuView.Layout = this;
            Init();
        }

        private SwipeMenuLayout(Context context, IAttributeSet attrs)
            : base(context, attrs) { }

        private SwipeMenuLayout(Context context)
            : base(context) { }

        public void SetPosition(int position)
        {
            Position = position;
            MenuView.Position = position;
        }

        private void Init()
        {
            MIN_FLING = Dp2Px(15);
            MAX_VELOCITYX = -Dp2Px(500);
            LayoutParameters = new AbsListView.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent);
            mGestureListener = new DefaultOnGestureListener((x) =>
            {
                IsFling = x;
            }, MIN_FLING, MAX_VELOCITYX);
            mGestureDetector = new GestureDetectorCompat(Context, mGestureListener);

            if (mCloseInterpolator != null)
            {
                mCloseScroller = ScrollerCompat.Create(Context, mCloseInterpolator);
            }
            else
            {
                mCloseScroller = ScrollerCompat.Create(Context);
            }
            if (mOpenInterpolator != null)
            {
                mOpenScroller = ScrollerCompat.Create(Context, mOpenInterpolator);
            }
            else
            {
                mOpenScroller = ScrollerCompat.Create(Context);
            }

            LayoutParams contentParams = new LayoutParams(LayoutParams.MatchParent,
                LayoutParams.WrapContent);
            ContentView.LayoutParameters = contentParams;
            if (ContentView.Id < 1)
            {
                ContentView.Id = CONTENT_VIEW_ID;
            }

            MenuView.Id = MENU_VIEW_ID;
            MenuView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);

            AddView(ContentView);
            AddView(MenuView);
        }

        public bool OnSwipe(MotionEvent e)
        {
            mGestureDetector.OnTouchEvent(e);
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    {
                        mDownX = (int)e.GetX();
                        IsFling = false;
                    }
                    break;
                case MotionEventActions.Move:
                    {
                        int dis = (int)(mDownX - e.GetX());
                        if (state == STATE_OPEN)
                        {
                            dis += MenuView.Width;
                        }
                        Swipe(dis);
                    }
                    break;
                case MotionEventActions.Up:
                    {
                        if (IsFling || (mDownX - e.GetX()) > (MenuView.Width / 2))
                        {
                            SmoothOpenMenu();
                        }
                        else
                        {
                            SmoothCloseMenu();
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }

        public bool IsOpen()
        {
            return state == STATE_OPEN;
        }

        private void Swipe(int dis)
        {
            if (dis > MenuView.Width)
            {
                dis = MenuView.Width;
            }
            if (dis < 0)
            {
                dis = 0;
            }
            ContentView.Layout(-dis, ContentView.Top,
                ContentView.Width - dis, MeasuredHeight);
            MenuView.Layout(ContentView.Width - dis, MenuView.Top,
                ContentView.Width + MenuView.Width - dis,
                MenuView.Bottom);
        }

        public override void ComputeScroll()
        {
            if (state == STATE_OPEN)
            {
                if (mOpenScroller.ComputeScrollOffset())
                {
                    Swipe(mOpenScroller.CurrX);
                    PostInvalidate();
                }
            }
            else
            {
                if (mCloseScroller.ComputeScrollOffset())
                {
                    Swipe(mBaseX - mCloseScroller.CurrX);
                    PostInvalidate();
                }
            }
        }

        public void SmoothCloseMenu()
        {
            state = STATE_CLOSE;
            mBaseX = -ContentView.Left;
            mCloseScroller.StartScroll(0, 0, mBaseX, 0, 350);
            PostInvalidate();
        }

        public void SmoothOpenMenu()
        {
            state = STATE_OPEN;
            mOpenScroller.StartScroll(-ContentView.Left, 0,
                MenuView.Width, 0, 350);
            PostInvalidate();
        }

        public void CloseMenu()
        {
            if (mCloseScroller.ComputeScrollOffset())
            {
                mCloseScroller.AbortAnimation();
            }
            if (state == STATE_OPEN)
            {
                state = STATE_CLOSE;
                Swipe(0);
            }
        }

        public void OpenMenu()
        {
            if (state == STATE_CLOSE)
            {
                state = STATE_OPEN;
                Swipe(MenuView.Width);
            }
        }

        private int Dp2Px(int dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp,
                Context.Resources.DisplayMetrics);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            MenuView.Measure(MeasureSpec.MakeMeasureSpec(0,
                 MeasureSpecMode.Unspecified), MeasureSpec.MakeMeasureSpec(
                 MeasuredHeight, MeasureSpecMode.Exactly));
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            ContentView.Layout(0, 0, MeasuredWidth,
                ContentView.MeasuredHeight);
            MenuView.Layout(MeasuredWidth, 0,
                MeasuredWidth + MenuView.MeasuredWidth,
                ContentView.MeasuredHeight);
        }

        public void SetMenuHieght(int measuredHeight)
        {
            Log.Info("yzf", "pos = " + Position + ", height = " + measuredHeight);
            LayoutParams param = (LayoutParams)MenuView.LayoutParameters;
            if (param.Height != measuredHeight)
            {
                param.Height = measuredHeight;
                MenuView.LayoutParameters = param;
            }
        }
    }
}