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
using SimpleOnGestureListener = Android.Views.GestureDetector.SimpleOnGestureListener;

namespace Cooksts.Droid.SWipeMenuList
{
    public class DefaultOnGestureListener : SimpleOnGestureListener
    {
        private Action<bool> SetFling;
        private int MinFling;
        private int MaxVelocityx;

        public DefaultOnGestureListener(Action<bool> setFling, int minFling, int maxVelocityx)
        {
            SetFling = setFling;
            MinFling = minFling;
            MaxVelocityx = maxVelocityx;
        }

        public override bool OnDown(MotionEvent e)
        {
            SetFling(false);
            return true;
        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            if ((e1.GetX() - e2.GetX()) > MinFling && velocityX < MaxVelocityx)
            {
                SetFling(true);
            }
            return base.OnFling(e1, e2, velocityX, velocityY);
        }
    }
}