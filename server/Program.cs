using System;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO.Routing;
using FlatSharp;
using Shared.FlatBuffers;

namespace server
{
    public sealed class AssetController : WebApiController
    {
        [Route(HttpVerbs.Get, "/assets/{filename?}")]
        public async Task GetAsset(string filename) 
        {
          var ctx = HttpContext;
          var stream = new MemoryStream();
          
          Image img = Image.FromFile("./assets/" + filename);
          img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
          var bitmapData = stream.ToArray();

          LoWImage lowImage = new LoWImage(){ width=img.Width, height=img.Height, data=bitmapData };

          int maxBytesNeeded = FlatBufferSerializer.Default.GetMaxSize(lowImage);
          byte[] buffer = new byte[maxBytesNeeded];
          int bytesWritten = FlatBufferSerializer.Default.Serialize(lowImage, buffer);
          
          await ctx.SendDataAsync(bytesWritten);
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
              .WithLocalSessionManager()
              .WithWebApi("/api", m => m
                    .WithController<AssetController>());
            return server;
        }
        
    }
}
