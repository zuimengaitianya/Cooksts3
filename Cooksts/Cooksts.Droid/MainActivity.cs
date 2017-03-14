using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using System.Threading.Tasks;
using PullToRefresharp.SwipeMenuList;
using Cooksts.Droid.BLL;
//using Com.Fortysevendeg.Swipelistview;
using Cooksts.Droid.Renderers;
using System.Collections.Generic;

namespace Cooksts.Droid
{
    [Activity(Label = "Cooksts", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity//,ListActivity
    {

        private static string[] ITEMS = new string[] { "Abbaye de Belloc", "Abbaye du Mont des Cats", "Abertam", "Abondance", "Ackawi", "Acorn", "Adelost", "Affidelice au Chablis", "Afuega'l Pitu", "Airag", "Airedale", "Aisy Cendre", "Allgauer Emmentaler", "Abbaye de Belloc", "Abbaye du Mont des Cats", "Abertam", "Abondance", "Ackawi", "Acorn", "Adelost", "Affidelice au Chablis", "Afuega'l Pitu", "Airag", "Airedale", "Aisy Cendre", "Allgauer Emmentaler" };

        public static MainActivity mainActivity;

        protected override void OnCreate(Bundle bundle)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            #region 下拉列表显示效果

            mainActivity = this;

            SetContentView(Resource.Layout.main);

            PullToRefresharp.Android.Widget.ListView lv = (PullToRefresharp.Android.Widget.ListView)FindViewById(Resource.Id.myGridView1);
            //lv.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, ITEMS);

            List<Students> studentsList = new List<Students>();
            for (int i = 0; i < 10; i++)
            {
                Students stu = new Students
                {
                    photo = "user.png",
                    name = "name" + i,
                    age = 10 + i,
                    //sex = item.sex,
                };
                studentsList.Add(stu);
            }
            lv.Adapter = new MyAdapter(studentsList, Android.App.Application.Context);


            lv.RefreshActivated += (e, s) =>
            {
                Task.Delay(1000).ContinueWith((t) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                       {
                           lv.OnRefreshCompleted();
                       });
                });
            };

            lv.SetMenuCreator(new LvSwipeMenuCreator());

            lv.SetOnMenuItemClickListener(new LvMenuItemClickListener());

            LvSwipeListener swipeListener = new LvSwipeListener();

            lv.SetOnSwipeListener(swipeListener);
            #endregion

            #region 自定义adapater
            //ListView lv = new ListView(this);
            //lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ITEMS);

            #endregion

            #region com.Swipelistview
            //SetContentView(Resource.Layout.activity_main);
            //Com.Fortysevendeg.Swipelistview.SwipeListView lv = (Com.Fortysevendeg.Swipelistview.SwipeListView)FindViewById(Resource.Id.example_lv_list);
            //lv.Adapter= new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, ITEMS);

            #endregion

            //LoadApplication(new App());

        }

        //protected override void OnListItemClick(ListView l, View v, int position, long id)
        //{
        //    var t = ITEMS[position];
        //    Toast.MakeText(this, t, ToastLength.Short).Show();
        //}
    }
}

