/*

    Loading abstraction for text files

*/

using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using System;
using LegendOfWorlds.Image;

namespace LegendOfWorlds.Loaders {

    public static class Loaders{
        private static HttpClient HC = new HttpClient();

        public static async void OpenTxtFile()
        {

        }

        public static async Task<Image.Image> OpenImageFile(string uri)
        {
            //HC.BaseAddress = new Uri("http://localhost:5001");
            var bob = await HC.GetAsync(uri);
            byte[] byteArray = await bob.Content.ReadAsByteArrayAsync();

            string[] splitDot = uri.Split('.');

            Console.WriteLine(splitDot[splitDot.Length-1]);

            return new Image.Image(byteArray, splitDot[splitDot.Length-1]);

        }



    }
}   