﻿using System;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO.Actions;
using EmbedIO.Routing;

namespace server
{

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
              .WithModule(new ActionModule(
                "/api/assets", 
                HttpVerbs.Get, 
                ctx => {
                  var stream = new MemoryStream();
                  Image img = Image.FromFile("../client/wwwroot/assets/doll.png");
                  img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                  var bitmapData = stream.ToArray();
                  return ctx.SendDataAsync(new { Width=img.Width, Height=img.Height, Data=bitmapData});
                }
              ));
            return server;
        }
        /*
        [Route(HttpVerbs.Get, "/binary")]
        public async Task GetBinary() 
        {
          // Call a fictional external source
         using (var stream = HttpContext.OpenResponseStream())
                    await stream.WriteAsync(dataBuffer, 0, 0);
        }
        */
    }
}
