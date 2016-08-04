using System;
using System.IO;
using System.Net;
using System.Text;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace PanoramioMap
{
    /// <summary>
    /// Gets the panoramio's photos of the area of map control and places their in preview bar
    /// </summary>
    public class MapPanoramioBehavior : DependencyObject, IBehavior
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
            _mapView.SubscribeViewChanged(ChangeViewEventHandler);
        }

        async private static void ChangeViewEventHandler(object sender, EventArgs eventArgs)
        {
            var panoramioRequest = WebRequest.CreateHttp(
                "http://www.panoramio.com/map/get_panoramas.php?set=public&from=0&to=20&minx=-180&miny=-90&maxx=180&maxy=90&size=medium&mapfilter=true");
            var panoramioResponse = await panoramioRequest.GetResponseAsync();
            var streamReader = new StreamReader(panoramioResponse.GetResponseStream(), Encoding.UTF8);
            var panoramioResult = streamReader.ReadToEnd();
        }

        public void Detach()
        {
            //TODO: _mapView.UnsubscribeViewChanged(ChangeViewEventHandler)
            _mapView = null;
        }

        public DependencyObject AssociatedObject => _mapView;
    }
}
