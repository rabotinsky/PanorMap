using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Xaml.Interactivity;

namespace PanoramioMap
{
    /// <summary>
    /// Gets the panoramio's photos of the area of map control and places their on map
    /// </summary>
    public class MapPanoramioBehavior : DependencyObject, IBehavior
    {
        private const int ButtonsCountOnMap = 70;
        private MapView _mapView;
        private readonly Dictionary<string, PhotoData> _photoUrlToPhotoData = new Dictionary<string, PhotoData>();
        
        public void Attach(DependencyObject associatedObject)
        {
            if (_mapView != null)
            {
                throw new InvalidOperationException();
            }
            var mapView = associatedObject as MapView;
            if (mapView == null)
            {
                throw new InvalidOperationException();
            }
            _mapView = mapView;
            _mapView.SubscribeViewChanged(ChangeViewEventHandler);
        }

        async private void ChangeViewEventHandler(object sender, EventArgs eventArgs)
        {
            _mapView.ClearMap();
            Geopoint topLeft;
            Geopoint bottomRight;
            _mapView.GetBoundLocations(out topLeft, out bottomRight);
            var photoDescriptionsMiniSquare = await PanoramioApi.RequestPhotos(ButtonsCountOnMap, "mini_square", topLeft, bottomRight);
            var photoDescriptionsOriginal = await PanoramioApi.RequestPhotos(ButtonsCountOnMap, "original", topLeft, bottomRight);
            var originalPhotosDict = photoDescriptionsOriginal.ToDictionary(x => x.PhotoUrl, x => x);
            foreach (var photoDescription in photoDescriptionsMiniSquare)
            {
                if (!originalPhotosDict.ContainsKey(photoDescription.PhotoUrl))
                {
                    continue;
                }
                if (!_photoUrlToPhotoData.ContainsKey(photoDescription.PhotoUrl))
                {
                    var bi = new BitmapImage { UriSource = new Uri(photoDescription.PhotoFileUrl) };
                    _photoUrlToPhotoData[photoDescription.PhotoUrl] = new PhotoData
                    {
                        MiniSquareSizeDescription = photoDescription,
                        MiniSquareImage = bi,
                        OriginalSizeDescription = originalPhotosDict[photoDescription.PhotoUrl]
                    };
                }
                var photoData = _photoUrlToPhotoData[photoDescription.PhotoUrl];
                var button = new Button
                {
                    Background = new SolidColorBrush(Colors.White),
                    BorderBrush = new SolidColorBrush(Colors.Gray),
                    Padding = new Thickness(0),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Style = Application.Current.Resources["ImagePreviewButtonStyle"] as Style,
                    Tag = photoData
                };
                button.Tapped += PreviewImageButtonTapped;
                var photoGeoposition = new BasicGeoposition {Latitude = photoDescription.Latitude, Longitude = photoDescription.Longitude };
                var img = new Image
                {
                    Source = photoData.MiniSquareImage,
                    Width = photoDescription.Width,
                    Height = photoDescription.Height,
                };
                button.Content = img;
                button.Width = photoDescription.Width + 10;
                button.Height = photoDescription.Height + 20;
                button.Margin = new Thickness(-button.Width / 2, -button.Height, 0, 0);
                _mapView.MoveUiElement(photoGeoposition, button);
            }
        }

        private static void PreviewImageButtonTapped(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var photoDescription = (PhotoData) button.Tag;
            var page = (Page)((Frame)Window.Current.Content).Content;
            page.Frame.Navigate(typeof (ImageViewPage), photoDescription);
        }

        public void Detach()
        {
            //TODO: _mapView.UnsubscribeViewChanged(ChangeViewEventHandler)
            _mapView = null;
        }

        public DependencyObject AssociatedObject => _mapView;
    }
}
