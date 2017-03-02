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
using PullToRefresharp.SwipeMenuList;
using Android.Graphics.Drawables;

namespace Cooksts.Droid.BLL
{
    /// <summary>
    /// ���廬����ʼ�ͽ���
    /// </summary>
    public class LvSwipeListener : IOnSwipeListener
    {
        public void OnSwipeStart(int position)
        {

        }

        public void OnSwipeEnd(int position)
        {

        }
    }
    /// <summary>
    /// ���廬������
    /// </summary>
    public class LvSwipeMenuCreator : ISwipeMenuCreator
    {
        public void Create(SwipeMenu menu)
        {
            SwipeMenuItem openItem = new SwipeMenuItem(MainActivity.mainActivity);
            openItem.Background = new ColorDrawable(Android.Graphics.Color.Green);
            openItem.Title = "��";
            openItem.TitleSize = 20;
            openItem.Width = 100;
            openItem.TitleColor = Android.Graphics.Color.White;

            SwipeMenuItem deleteItem = new SwipeMenuItem(MainActivity.mainActivity);
            deleteItem.Background = new ColorDrawable(Android.Graphics.Color.Gray);
            deleteItem.Title = "ɾ��";
            deleteItem.TitleSize = 20;
            deleteItem.Width = 100;
            deleteItem.TitleColor = Android.Graphics.Color.Black;

            menu.AddMenuItem(openItem);
            menu.AddMenuItem(deleteItem);
        }
    }

    public class LvMenuItemClickListener : IOnMenuItemClickListener
    {
        public bool OnMenuItemClick(int position, SwipeMenu menu, int index)
        {

            List<SwipeMenuItem> allSwipeMenuItem = menu.GetMenuItems();

            SwipeMenuItem selectMenuItem = allSwipeMenuItem[index];

            

            return true;
        }
    }
}