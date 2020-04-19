using System.IO;
using System.Text.Json;
using FlatSharp;

namespace converter
{
    class Program
    {
        static void Main(string[] args)
        {
            string tiledString = File.ReadAllText("../client/wwwroot/assets/test_map.json");

            Shared.Cells.Tiled.Level tiledData = JsonSerializer.Deserialize<Shared.Cells.Tiled.Level>(tiledString);
            var cell = new Shared.Cells.Custom.Cell() { 
              name="test_map",
              tileIds=tiledData.layers[0].data
            };
            var cells = new Shared.Cells.Custom.Cell[] {
              cell
            };
            var world = new Shared.Cells.Custom.World() {  
              name="test_map",
              width=1,
              height=1,
              cells=cells
            };
            
            int maxBytesNeeded = FlatBufferSerializer.Default.GetMaxSize(world);
            byte[] buffer = new byte[maxBytesNeeded];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(world, buffer);

            File.WriteAllBytes("../client/wwwroot/assets/test_map.cell", buffer);
        }
    }
}

