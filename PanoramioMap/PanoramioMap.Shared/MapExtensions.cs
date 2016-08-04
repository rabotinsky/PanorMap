// мопед не мой, я только разместил объяву (с)
// для меня оказалось проблемой, что для приложения Universal App для платформы Windows 8.1
// не оказалось такого же элемента управления - карты, как для Windows Phone 8.1
// взял код из статьи https://blogs.msdn.microsoft.com/bingdevcenter/2014/06/24/how-to-make-use-of-maps-in-universal-apps/
using System.Collections.Generic;
using Windows.Devices.Geolocation;
#if WINDOWS_APP
using Bing.Maps;
#endif

namespace PanoramioMap
{
    public static class MapExtensions
    {
#if WINDOWS_APP

        public static LocationCollection ToLocationCollection(this IList<BasicGeoposition> pointList)
        {
            var locs = new LocationCollection();

            foreach (var p in pointList)
            {
                locs.Add(p.ToLocation());
            }

            return locs;
        }

        public static Geopoint ToGeopoint(this Location location)
        {
            return new Geopoint(new BasicGeoposition { Latitude = location.Latitude, Longitude = location.Longitude });
        }

        public static Location ToLocation(this Geopoint location)
        {
            return new Location(location.Position.Latitude, location.Position.Longitude);
        }

        public static Location ToLocation(this BasicGeoposition location)
        {
            return new Location(location.Latitude, location.Longitude);
        }

#elif WINDOWS_PHONE_APP

        //Add any required Windows Phone Extensions

#endif
    }
}
