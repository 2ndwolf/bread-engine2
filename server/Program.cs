using System;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO.Routing;
using FlatSharp;
using Shared.FlatBuffers;
using Zstandard.Net;
using System.IO.Compression;

namespace server
{
    public sealed class AssetController : WebApiController
    {
        [Route(HttpVerbs.Get, "/assets/{filename?}")]
        public async Task GetAsset(string filename) 
        {
          var stream = new MemoryStream();
          Image img = Image.FromFile("./assets/" + filename);
          img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
          var bitmapData = stream.ToArray();

          LoWImage lowImage = new LoWImage(){ width=img.Width, height=img.Height, data=bitmapData };

          int maxBytesNeeded = FlatBufferSerializer.Default.GetMaxSize(lowImage);
          byte[] buffer = new byte[maxBytesNeeded];
          int bytesWritten = FlatBufferSerializer.Default.Serialize(lowImage, buffer);

           
          // ZStandard compress.
          byte[] compressed = new byte[]{};
          byte[] output = new byte[]{};
          
          using (var memoryStream = new MemoryStream())
          using (var compressionStream = new ZstandardStream(memoryStream, CompressionMode.Compress))
          {
            compressionStream.CompressionLevel = 11;               // optional!!
            compressionStream.Write(buffer, 0, buffer.Length);
            compressionStream.Close();
            compressed = memoryStream.ToArray();
          }

          using (var stream2 = HttpContext.OpenResponseStream())
            await stream2.WriteAsync(compressed, 0, compressed.Length);
           
        }
    }

    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            var url = "http://localhost:9696/";
            if (args.Length > 0)
                url = args[0];

            // Our web server is disposable.
            using (var server = CreateWebServer(url))
            {
                // Once we've registered our modules and configured them, we call the RunAsync() method.
                server.RunAsync();

                var browser = new System.Diagnostics.Process()
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true }
                };
                browser.Start();
                // Wait for any key to be pressed before disposing of our web server.
                // In a service, we'd manage the lifecycle of our web server using
                // something like a BackgroundWorker or a ManualResetEvent.
                Console.ReadKey(true);
            }
        }
	
	// Create and configure our web server.
        private static WebServer CreateWebServer(string url)
        {
            var server = new WebServer(o => o
              .WithUrlPrefix(url)
              .WithMode(HttpListenerMode.EmbedIO))
              .WithWebApi("/api", m => m.WithController<AssetController>());
            return server;
        }
        
    }
}
