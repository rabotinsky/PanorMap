using System.Collections.Generic;
using Windows.Data.Json;

namespace PanoramioMap
{
    public static class PanoramioParser
    {
        public static List<PhotoDescription> ParseJsonAnswer(string jsonString)
        {
            var result = new List<PhotoDescription>();
            var parsedJson = JsonValue.Parse(jsonString);
            var photos = parsedJson.GetObject().GetNamedArray("photos");
            foreach (var photo in photos)
            {
                var photoObject = photo.GetObject();
                var photoDescription = new PhotoDescription
                {
                    Latitude = photoObject.GetNamedNumber("latitude"),
                    Longitude = photoObject.GetNamedNumber("longitude"),
                    PhotoTitle = photoObject.GetNamedString("photo_title"),
                    PhotoFileUrl = photoObject.GetNamedString("photo_file_url"),
                    PhotoUrl = photoObject.GetNamedString("photo_url"),
                    Width = photoObject.GetNamedNumber("width"),
                    Height = photoObject.GetNamedNumber("height")
                };
                result.Add(photoDescription);
            }
            return result;
        }
    }
}
