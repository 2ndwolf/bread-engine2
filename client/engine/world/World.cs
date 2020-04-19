using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Net.Http;

using Microsoft.JSInterop;
using Blazor.Extensions.Canvas.WebGL;
using eeNet;

using LegendOfWorlds.Engine.Ecs;
using static Shared.Utils.Render;
using static LegendOfWorlds.Engine.Ecs.RenderSystems;

using static Shared.Engine.World;
using SharedEngine = Shared.Engine;


namespace LegendOfWorlds.Engine {
  // public struct System {
  //   public string name;
  //   public Func<Task> action;

  //   public static System create(Func<Task> _action) {
  //     // Abstracts so that when you pass the action (which is just a function)
  //     // you also get the name of it.
  //     return new System() { name=nameof(_action), action=_action };
  //   }
  // }

  public partial class World {

    // ECS
    // public static Audrey.Engine engine = new Audrey.Engine();
    // public static List<System> systems = new List<System>();
    public static List<SharedEngine.System> renderSystems = new List<SharedEngine.System>();


    // Events
    // public static EventEmitter EE = new EventEmitter();

    // Rendering
    // public static WebGLContext GL;
    // public static IJSRuntime jsRuntime;

    // public World(IJSRuntime _jsRuntime) {
    //   jsRuntime = _jsRuntime;
    // }

    public static async Task Init(/* WebGLContext baseContext */) {
      // Initialize Canvas
      // GL = Shared.Engine.World.GL;

      // Initialize RenderSystems.
      RenderSystems.Init();

      // Intialize events.
      Events.Init();

      
      // Initialize game loop and render loop.
      //Task.Run(Update);
      //Task.Run(UpdateVM);
      // await RenderTest();
      await Task.Run(Rendering);
    }

    public static async Task Rendering() {
      for(;;) {
        // renderSystems.ForEach((System system) => {
        //   goRender();
        // });
        // await Task.Delay(32);
        await GL.ClearAsync(BufferBits.COLOR_BUFFER_BIT);

        // await Shared.Utils.Render.clearRootCanvas();
         
        foreach(SharedEngine.System system in renderSystems) {
          await system.action();
        }
        
        await Task.Delay(8);
      }
    }

    // public static async Task Update() {
    //   for(;;) {
        
    //     ComputeDeltaTime();

    //     systems.ForEach(async (System system) => {
    //       await system.action();
    //     });
    //     await Task.Delay(16);
    //   }
    // }

      /*
    public static async Task UpdateVM() {
      for(;;) {
        systems.ForEach((System system) => {
          system.action();
        });
        await Task.Delay(16);
      }
    }
      */

  }
}
