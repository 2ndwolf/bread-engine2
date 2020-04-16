/*

    Loading abstraction for text files

*/

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Shared.FlatBuffers;
using MessagePack; 

using System.IO.Compression;


// using static LegendOfWorlds.Engine.World;




namespace LegendOfWorlds.Loaders {

    public struct ImageWithData{
        public int width, height;
        public uint[] data;
    }

    public static class Load{
        private static HttpClient HC = new HttpClient();
        private static bool baSet = false;

        // public static async Task OpenTxtFile()
        // {
        // }


        public static async Task<ImageWithData> LoadImage(string url){

            if(!baSet){
                baSet = true;
                HC.BaseAddress = new Uri("http://localhost:9696/api/assets/");
            }

            var gotImage = await HC.GetAsync(url).ConfigureAwait(false);

            byte[] compressedImg = await gotImage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            byte[] decompressedImg = LegendOfWorlds.Shared.Utils.LZMA.Decompress(compressedImg);

            var imgData = MessagePackSerializer.Deserialize<LoWImage>(decompressedImg);

            int startOffset = imgData.data[10];
            int colorDepth = imgData.data[28];

            ImageWithData img = new ImageWithData();

            img.width = imgData.width;
            img.height = imgData.height;

            // Console.WriteLine("[{0}]", string.Join(", ", imgData.data));
            uint[] abView = new uint[img.width * img.height];
            int j = ((imgData.data.Length - startOffset) / 4) - 1;
            // Console.WriteLine(j * 4 + ", " + imgData.data.Length);
            // Console.WriteLine(j);

            if(colorDepth == 24){
                Console.WriteLine("Non transparent images will display mirrored until I fix it");
                for(var i = 0; i < abView.Length; i ++){

                    abView[i] = abView[i] | (uint) 255; //A OK
                    abView[i] = abView[i] | (uint) imgData.data[j * 3] << 8; //B OK?
                    abView[i] = abView[i] | (uint) imgData.data[j * 3 + 1] << 16; //G OK
                    abView[i] = abView[i] | (uint) imgData.data[j * 3 + 2] << 24; // R OK
                    j--;
                }

            } else if (colorDepth == 32){
                j = 0;
                for(var i = img.height - 1; i >= 0; i --){
                    for(var k = 0; k < img.width; k ++){
                        uint currPos = (uint)(startOffset + ((i * img.width + k) << 2));
                        abView[j] = abView[j] | (uint) imgData.data[currPos] << 8; // b OK
                        abView[j] = abView[j] | (uint) imgData.data[currPos + 1] << 16; // g OK
                        abView[j] = abView[j] | (uint) imgData.data[currPos + 2] << 24; //r OK
                        abView[j] = abView[j] | (uint) imgData.data[currPos + 3]; //A OK
                        j++;
                    }
                }
            } else {
                Console.WriteLine("The supported bit depths for images are 32 (RGBA) and 24 (RGB), the server conversion to bitmap should have taken care of this.");
            }
            img.data = abView;
            
            return img;
        }
    }
}

 