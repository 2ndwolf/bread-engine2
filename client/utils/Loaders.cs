/*

    Loading abstraction for text files

*/

using System.Threading.Tasks;
using System.Net;
using System.IO;
using System;

namespace LegendOfWorlds.Loaders {

    public static class Loaders{

        public static async Task<string> OpenTxtFile(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using(HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using(Stream stream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static async void OpenImageFile(string uri)
        {
            
        }

    }
}   