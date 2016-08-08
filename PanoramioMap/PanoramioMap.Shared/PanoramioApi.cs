using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace PanoramioMap
{
    public static class PanoramioApi
    {
        async public static Task<List<PhotoDescription>> RequestPhotos(int count, string size, Geopoint topLeft, Geopoint bottomRight)
        {
            const string pattern = "http://www.panoramio.com/map/get_panoramas.php?set=public&from=0&to={0}&minx={1}&miny={2}&maxx={3}&maxy={4}&size={5}&mapfilter=true";
            var request = string.Format(pattern, count,
                Math.Min(topLeft.Position.Longitude, bottomRight.Position.Longitude),
                Math.Min(topLeft.Position.Latitude, bottomRight.Position.Latitude),
                Math.Max(topLeft.Position.Longitude, bottomRight.Position.Longitude),
                Math.Max(topLeft.Position.Latitude, bottomRight.Position.Latitude),
                size);
            var panoramioRequest = WebRequest.CreateHttp(request);
            var panoramioResponse = await panoramioRequest.GetResponseAsync();
            var streamReader = new StreamReader(panoramioResponse.GetResponseStream(), Encoding.UTF8);
            var jsonText = streamReader.ReadToEnd();
            var photoDescriptions = PanoramioParser.ParseJsonAnswer(jsonText);
            return photoDescriptions;
        }
    }
}
