/*

    Loading abstraction for text files

*/

using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System;
using LegendOfWorlds.Image;

namespace LegendOfWorlds.Loaders {

    public struct ImageWithData{
        public int width, height;
        public uint[] data;
    }

    public static class Load{
        private static HttpClient HC = new HttpClient();
        private static bool baSet = false;

        public static async void OpenTxtFile()
        {

        }

        public struct ImageWithData {
            public int width, height;
            public uint[] data;
        }

            public static async Task<ImageWithData> LoadImage(string url){

                if(!baSet){
                    baSet = true;
                    HC.BaseAddress = new Uri("https://localhost:5001");
                }

                var imageFile = await HC.GetAsync(url);
                byte[] byteArray = await imageFile.Content.ReadAsByteArrayAsync();

                ImageWithData img = new ImageWithData();

                BigGustave.Png png = BigGustave.Png.Open(byteArray);

                img.width = png.Width;
                img.height = png.Height;

                Console.WriteLine(png.Height);

                uint[] abView = new uint[png.Width * png.Height];

                for(var i = 0; i < abView.Length; i ++){ //Row
                    BigGustave.Pixel pix = png.GetPixel(i, 9);
                    abView[i] = (uint) pix.R << 24; // R
                    abView[i] = abView[i] | (uint) pix.G << 16; //G
                    abView[i] = abView[i] | (uint) pix.B << 8; //B
                    abView[i] = abView[i] | (uint) pix.A; //A
                }

                img.data = abView;
                
                return img;
            }
        }
    }

 