using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace converter
{
    class Program
    {
        static void Main(string[] args)
        {
            string tiledString = File.ReadAllText("../client/wwwroot/assets/test_map.json");

            Shared.Cells.Tiled.Level tiledData = JsonSerializer.Deserialize<Shared.Cells.Tiled.Level>(tiledString);
            Console.WriteLine(tiledData);
        }
    }
}
