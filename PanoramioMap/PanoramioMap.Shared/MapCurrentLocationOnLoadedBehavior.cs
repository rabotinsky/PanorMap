using System;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace PanoramioMap
{
    /// <summary>
    /// Scrolls map to the current location of user after control's load event
    /// </summary>
    public class MapCurrentLocationOnLoadedBehavior : DependencyObject, IBehavior
    {
        private MapView _mapView;

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
            _mapView.Loaded += MapViewOnLoaded;
        }

        public void Detach()
        {
            _mapView.Loaded -= MapViewOnLoaded;
            _mapView = null;
        }

        async private void MapViewOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {

            var geolocator = new Geolocator();
            Geoposition geoposition = null;
            try
            {
                geoposition = await geolocator.GetGeopositionAsync();
            }
            catch (Exception ex)
            {
                //TODO: log + error message
            }
            if (geoposition != null)
            {
                _mapView.Center = geoposition.Coordinate.Point;
                _mapView.Zoom = 15;
            }
            else
            {
                //TODO: log + error message
            }
        }

        public DependencyObject AssociatedObject => _mapView;
    }
}
