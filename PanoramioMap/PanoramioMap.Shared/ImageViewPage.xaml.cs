using System;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace PanoramioMap
{
    public sealed partial class ImageViewPage
    {
        public ImageViewPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var photoData = (PhotoData)e.Parameter;
            var bi = new BitmapImage { UriSource = new Uri(photoData.OriginalSizeDescription.PhotoFileUrl) };
            var img = new Image
            {
                Source = bi,
                Width = photoData.OriginalSizeDescription.Width,
                Height = photoData.OriginalSizeDescription.Height,
            };
            Frame.Content = img;
        }
    }
}
