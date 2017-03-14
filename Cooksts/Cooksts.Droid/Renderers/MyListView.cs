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
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Cooksts.Droid.SWipeMenuList;
using Android.Graphics.Drawables;
using Cooksts.Renderers;
using Android.Graphics;

[assembly: ExportRenderer(typeof(Cooksts.Renderers.MyListView), typeof(Cooksts.Droid.Renderers.MyListViewRenderer))]
namespace Cooksts.Droid.Renderers
{
    public class MyListViewRenderer : ViewRenderer<MyListView, SwipeListView>, Android.Views.View.IOnTouchListener
    {
        //PullToRefresharp.Android.Widget.ListView androidListView = null;
        //protected override void OnElementChanged(ElementChangedEventArgs<MyListView> e)
        //{
        //    base.OnElementChanged(e);

        //    LayoutInflater inflatorservice = (LayoutInflater)Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService);
        //    //androidListView=(SwipeListView)inflatorservice.Inflate(Resource.Id.mySwipeListView, null, false);
        //    androidListView = (PullToRefresharp.Android.Widget.ListView)FindViewById(Resource.Id.myGridView1);

        //    androidListView.SetMenuCreator(new Cooksts.Droid.BLL.LvSwipeMenuCreator());

        //    if (e.OldElement == null && androidListView != null)
        //    {
        //        //执行初始设置
        //        base.SetNativeControl(androidListView);
        //    }

        //}



        SwipeListView androidListView = null;
        protected override void OnElementChanged(ElementChangedEventArgs<MyListView> e)
        {
            base.OnElementChanged(e);

            System.Collections.IList dataSource = (System.Collections.IList)e.NewElement.ItemsSource;
            List<Studens> stusList = (List<Studens>)e.NewElement.ItemsSource;
            List<Students> studentsList = new List<Students>();
            foreach (var item in stusList)
            {
                Students stu = new Students
                {
                    photo = item.photo,
                    name = item.name,
                    age = item.age,
                    sex = item.sex,
                };
                studentsList.Add(stu);
            }

            LayoutInflater inflatorservice = (LayoutInflater)Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService);
            androidListView = (SwipeListView)inflatorservice.Inflate(Resource.Layout.mySwipeListView, null, false);

            //安卓适配器
            //androidListView.Adapter = new ArrayAdapter(Android.App.Application.Context, Android.Resource.Layout.SimpleListItem1, dataSource);
            androidListView.Adapter = new MyAdapter(studentsList, Android.App.Application.Context);


            //设置间隔图片为颜色
            androidListView.Divider = new ColorDrawable(Android.Graphics.Color.Gray);
            androidListView.DividerHeight = 1;

            //androidListView.


            //androidListView = (SwipeListView)FindViewById(Resource.Id.mySwipeListView);
            //androidListView = Control as SwipeListView;

            if (e.OldElement == null && androidListView != null)
            {
                //执行初始设置
                base.SetNativeControl(androidListView);
            }

        }

    }

    public class Students : Java.Lang.Object
    {
        public string name { get; set; }
        public string sex { get; set; }
        public int age { get; set; }
        public string photo { get; set; }
    }



    public class MyAdapter : BaseAdapter
    {
        private List<Students> stuList;
        private LayoutInflater inflater;
        public MyAdapter(List<Students> stuList, Context context)
        {
            this.stuList = stuList;
            this.inflater = (LayoutInflater)Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService);
        }

        public override int Count
        {
            get { return stuList == null ? 0 : stuList.Count(); }

        }

        public override Java.Lang.Object GetItem(int position)
        {
            //Studens stu = stuList[position];
            //Java.Lang.Object obj = stu;
            return stuList[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            //加载布局为一个视图
            Android.Views.View view = inflater.Inflate(Resource.Layout.layout_student_item, null);
            Students student = (Students)GetItem(position);
            //在view视图中查找id为image_photo的控件
            ImageView image_photo = (ImageView)view.FindViewById(Resource.Id.image_photo);
            TextView tv_name = (TextView)view.FindViewById(Resource.Id.name);
            TextView tv_age = (TextView)view.FindViewById(Resource.Id.age);

            //using (Bitmap bm = AndroidCommon.NativeImageLoader.GetInstance().LoadNativeImage(student.photo))
            //{
            //    image_photo.SetImageBitmap(bm);
            //}
            image_photo.SetImageResource(Resource.Drawable.dog);
            tv_name.Text = student.name;
            tv_name.SetHighlightColor(Android.Graphics.Color.Gray);
            tv_age.Text = student.age.ToString();
            tv_age.SetHighlightColor(Android.Graphics.Color.Gray);
            return view;
        }
    }
}