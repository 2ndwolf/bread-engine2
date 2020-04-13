/*

    Loading abstraction for text files

*/

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Zstandard.Net;

using Shared.FlatBuffers;
using FlatSharp;


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

        public struct ImageWithData {
            public int width, height;
            public uint[] data;
        }

        public static async Task<ImageWithData> LoadImage(string url){

            if(!baSet){
                baSet = true;
                HC.BaseAddress = new Uri("http://localhost:9696/api/assets/");
            }

            var gotImage = await HC.GetAsync(url);

            byte[] compressedImg = await gotImage.Content.ReadAsByteArrayAsync();


            // ZStandard compress.
            byte[] decompressedImg = new byte[]{};
            
            using (var memoryStream = new MemoryStream())
            using (var decompressionStream = new ZstandardStream(memoryStream, CompressionMode.Decompress))
            {
                decompressionStream.CompressionLevel = 11;               // optional!!
                decompressionStream.Write(compressedImg, 0, compressedImg.Length);
                decompressionStream.Close();
                decompressedImg = memoryStream.ToArray();
            }

            //deserialize flatbuffer
            LoWImage imgData = FlatBufferSerializer.Default.Parse<LoWImage>(decompressedImg);


            // byte[] byteArray = await imageFile.Content.ReadAsByteArrayAsync();

            ImageWithData img = new ImageWithData();

            img.width = imgData.width;
            img.height = imgData.height;

            uint[] abView = new uint[img.width * img.height];

            for(var i = 0; i < abView.Length; i ++){

                abView[i] = (uint) imgData.data[i * 4] << 24; // R
                abView[i] = abView[i] | (uint) imgData.data[i * 4 + 1] << 16; //G
                abView[i] = abView[i] | (uint) imgData.data[i * 4 + 2] << 8; //B
                abView[i] = abView[i] | (uint) imgData.data[i * 4 + 3]; //A

            }

            img.data = abView;
            
            return img;
        }
    }
}

 