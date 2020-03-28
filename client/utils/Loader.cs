/*

    Loading abstraction for text files

*/

using System.Threading.Tasks;
using System;

namespace LegendOfWorlds.Loader {

    public class Loader{

        public async Task<string> OpenTxtFile(string uri)
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

        public async void OpenImageFile(string uri)
        {
            
        }

    }
}   