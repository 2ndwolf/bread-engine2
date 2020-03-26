using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blazor.Extensions.Canvas.Canvas2D;
using Leopotam.Ecs;

namespace LegendOfWorlds.Engine.World {
  struct Position {
      public int x;
      public int y;
  }

  class TestSystem : IEcsRunSystem, IEcsInitSystem {
      readonly EcsFilter<Position> _filter = null;
      readonly EcsWorld _world = null;
    
      void IEcsInitSystem.Init() {
          ref var newEntity = ref _world.NewEntity().Set<Position>();
          Console.WriteLine(newEntity);
      }

      void IEcsRunSystem.Run() {
          foreach (var i in _filter) {
              // its valid code.
              ref var component1 = ref _filter.Get1(i);

              component1.x += 1;
              component1.y += 1;
          }
      }
  }
class RenderSystem : IEcsInitSystem {
    readonly EcsFilter<Position> _filter = null;
    readonly EcsWorld _world = null;
  
    void IEcsInitSystem.Init() {
      Task.Run(async () => {
        for(;;) {
          foreach (var i in _filter) {
              // its valid code.
              var position = _filter.Get1(i);
              await World.baseCanvas.ClearRectAsync(0, 0, 500, 500);
              await World.baseCanvas.SetFillStyleAsync("green");
              await World.baseCanvas.FillRectAsync(position.x, position.y, 100, 100);
              await Task.Delay(4);
          }
        }
      });
    }
  }


  public class World {
    // ECS
    public static EcsWorld _world;
    public static EcsSystems _systems;

    // Rendering
    public static Canvas2DContext baseCanvas;
    public static Dictionary<int, Canvas2DContext> canvases;

    public static void Init(Canvas2DContext _baseCanvas) {
      // Initialize Canvas
      baseCanvas = _baseCanvas;
      canvases = new Dictionary<int, Canvas2DContext>();

      _world = new EcsWorld();

      _systems = new EcsSystems(_world);
      _systems.Add(new TestSystem());
      _systems.Add(new RenderSystem());
      _systems.Init();

      // Initialize game loop and render loop.
      Task.Run(Update);
    }

    public static async Task Update() {
      for(;;)
      {
        _systems.Run();
        await Task.Delay(16);
      }
    }
  }
}

