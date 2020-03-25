using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blazor.Extensions.Canvas.Canvas2D;

namespace LegendOfWorlds.Engine.World {
  public class World {
    // Temp entity position.
    public static int x = 0;
    public static int y = 0;

    public static Dictionary<int, Canvas2DContext> canvases;

    // Rendering
    public static Canvas2DContext baseCanvas;

    public static void Init(Canvas2DContext _baseCanvas) {
      baseCanvas = _baseCanvas;
      canvases = new Dictionary<int, Canvas2DContext>();

      Console.WriteLine("Initializing world!");

      Task.Run(Update);
      Task.Run(Render);
    }

    public static async Task Update() {
      for(;;)
      {
        Console.WriteLine("Game Logic!");
        await Task.Delay(16);
      }
    }

    public static async Task Render() {
      for(;;)
      {
        x++;
        y++;
        await baseCanvas.ClearRectAsync(0, 0, 500, 500);
        await baseCanvas.SetFillStyleAsync("green");
        await baseCanvas.FillRectAsync(x, y, 100, 100);
        Console.WriteLine("Rendering!");
        await Task.Delay(4);
      }
    }
  }
}

