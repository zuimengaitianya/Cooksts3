using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cooksts.CustomCell
{
    public class StudentCell : ViewCell
    {
        public StudentCell()
        {

            Image studentPhoto = new Image { WidthRequest = 14, };
            Label studentName = new Label { TextColor = Color.FromHex("#20824F") };
            Label studentAge = new Label { TextColor = Color.FromHex("#20824F") };

            studentPhoto.SetBinding(Image.ScaleProperty, new Binding("photo", BindingMode.OneWay, new MailReadStatusConverter()));
            studentName.SetBinding(Label.TextProperty, new Binding("name", BindingMode.OneWay));
            studentAge.SetBinding(Label.TextProperty, new Binding("age", BindingMode.OneWay));

            StackLayout layoutStudent = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    studentPhoto,
                    new StackLayout {
                        Orientation=StackOrientation.Vertical,
                        Children= {
                            studentName,
                            studentAge,
                        }
                    }
                }
            };

            View = layoutStudent;
        }
    }

    /// <summary>
    /// 获取邮件状态图标
    /// </summary>
    public class MailReadStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource image = ImageFormat.GetImageSource("dog.png");
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public static class ImageFormat
    {
        /// <summary>
        /// 获取图片资源
        /// </summary>
        /// <param name="ImageName"></param>
        /// <returns></returns>
        public static ImageSource GetImageSource(string ImageName)
        {
            return Device.OnPlatform(
               iOS: ImageSource.FromFile(ImageName),//Resources 目录
               Android: ImageSource.FromFile(ImageName),//Resources/drawable 目录
               WinPhone: ImageSource.FromFile("Images/" + ImageName));//Images 目录
        }
    }
}
