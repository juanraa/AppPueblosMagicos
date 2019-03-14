using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace AppPueblosMagicos
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                string FileName = value as string;
                if(string.IsNullOrEmpty(FileName))
                {
                    return null;
                }
                else
                {

                    var file = Windows.Storage.KnownFolders.MusicLibrary.GetFileAsync(@"PueblosMagicos\" + FileName).AsTask().Result;
                    var stream = file.OpenReadAsync().AsTask().Result;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);
                    return bitmapImage;
                }
            }
            catch(Exception ex)
            {

                return null;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
