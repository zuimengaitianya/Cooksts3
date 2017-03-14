using Cooksts.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cooksts
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            MyListView myListView = new MyListView();
            //myListView.ItemsSource = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            myListView.ItemTemplate = new DataTemplate(typeof(Cooksts.CustomCell.StudentCell));
            List<Studens> studensList = new List<Studens>();
            for (int i = 0; i < 10; i++)
            {
                Studens stu = new Studens();
                stu.name = "name" + i;
                stu.age = 10 + i;
                stu.photo = "user.png";
                studensList.Add(stu);
            }
            myListView.ItemsSource = studensList;
            Content = myListView;


            //ListView lv = new ListView();
            //lv.ItemTemplate = new DataTemplate(typeof(Cooksts.CustomCell.StudentCell));
            //List<Studens> studensList = new List<Studens>();
            //for (int i = 0; i < 10; i++)
            //{
            //    Studens stu = new Studens();
            //    stu.name = "name" + i;
            //    stu.age = 10 + i;
            //    stu.photo = "user.png";
            //    studensList.Add(stu);
            //}
            //lv.ItemsSource = studensList;
            //Content = lv;

        }
    }

    public class Studens
    {
        public string name { get; set; }
        public string sex { get; set; }
        public int age { get; set; }
        public string photo { get; set; }
    }
}
