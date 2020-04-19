
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Net.Http;

// using Microsoft.JSInterop;
using eeNet;

using static Shared.Engine.Ecs.Systems;
using Shared.Engine.Ecs;

#if WEBGL
using Blazor.Extensions.Canvas.WebGL;
using static Shared.Utils.Render;
#endif

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

    // public static IJSRuntime jsRuntime;

    // public World(IJSRuntime _jsRuntime) {
    //   jsRuntime = _jsRuntime;
    // }

#if (CLIENT || EDITOR)
    // Rendering
    public static WebGLContext GL;

    public static async Task Init(WebGLContext baseContext) {

      GL = baseContext;

      Console.WriteLine("Initializing WebGL");
      await InitWebGL();
      Console.WriteLine("WebGL initialized");

      // Initialize systems.
      Systems.Init();

      // Intialize events.
      // Events.Init();

      await Task.Run(Update);
    }
#elif SERVER
    public static async Task Init() {

      // Initialize systems.
      Systems.Init();

      // Intialize events.
      // Events.Init();

      await Task.Run(Update);
    }
#endif

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
