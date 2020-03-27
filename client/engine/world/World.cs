using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blazor.Extensions.Canvas.Canvas2D;

using LegendOfWorlds.Engine.Ecs;

namespace LegendOfWorlds.Engine {
  public struct System {
    public string name;
    public Action action;
  }

  public class World {
    // ECS
    public static Audrey.Engine engine = new Audrey.Engine();
    public static List<System> systems = new List<System>();

    // Rendering
    public static Canvas2DContext baseCanvas;
    public static Dictionary<int, Canvas2DContext> canvases;

    public static void Init(Canvas2DContext _baseCanvas) {
      // Initialize Canvas
      baseCanvas = _baseCanvas;
      canvases = new Dictionary<int, Canvas2DContext>();
      canvases.Add(-1, baseCanvas);

      // Initialize systems.
      Systems.Init();

      // Intiailize events.

      // Initialize game loop and render loop.
      Task.Run(Render);
      Task.Run(Update);
    }

    public static async Task Render() {
      for(;;) {
        systems.ForEach((System system) => {
          system.action();
        });
        await Task.Delay(8);
      }
    }

    public static async Task Update() {
      for(;;) {
        await Task.Delay(16);
      }
    }

  }
}

