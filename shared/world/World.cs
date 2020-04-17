using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Net.Http;

using Microsoft.JSInterop;
using eeNet;

using static Shared.Engine.Ecs.Systems;
using Shared.Engine.Ecs;


namespace Shared.Engine {
  public struct System {
    public string name;
    public Func<Task> action;

    public static System create(Func<Task> _action) {
      // Abstracts so that when you pass the action (which is just a function)
      // you also get the name of it.
      return new System() { name=nameof(_action), action=_action };
    }
  }

  public partial class World {

    // ECS
    public static Audrey.Engine engine = new Audrey.Engine();
    public static List<System> systems = new List<System>();
    // public static List<System> renderSystems = new List<System>();


    // Events
    public static EventEmitter EE = new EventEmitter();

    // Rendering
    // public static WebGLContext GL;
    public static IJSRuntime jsRuntime;

    public World(IJSRuntime _jsRuntime) {
      jsRuntime = _jsRuntime;
    }

    public static async Task Init() {

      // Initialize systems.
      Systems.Init();

      // Intiailize events.
      // Events.Init();

      await Task.Run(Update);
    }

    // public static async Task Rendering() {
    //   for(;;) {
    //     // renderSystems.ForEach((System system) => {
    //     //   goRender();
    //     // });
    //     // await Task.Delay(32);
    //     await GL.ClearAsync(BufferBits.COLOR_BUFFER_BIT);

    //     // await LegendOfWorlds.Utils.Render.clearRootCanvas();
         
    //     foreach(System system in renderSystems) {
    //       await system.action();
    //     }
        
    //     await Task.Delay(8);
    //   }
    // }

    public static async Task Update() {
      for(;;) {
        
        ComputeDeltaTime();

        systems.ForEach(async (System system) => {
          await system.action();
        });
        await Task.Delay(16);
      }
    }

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
