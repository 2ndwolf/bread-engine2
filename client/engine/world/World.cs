using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blazor.Extensions.Canvas.WebGL;
using eeNet;
using LegendOfWorlds.Engine.Ecs;
using Microsoft.JSInterop;

namespace LegendOfWorlds.Engine {
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
    public static List<System> renderSystems = new List<System>();

    // Events
    public static EventEmitter EE = new EventEmitter();

    // Rendering
    public static IJSRuntime jsRuntime;

    public World(IJSRuntime _jsRuntime) {
      jsRuntime = _jsRuntime;
    }

    public static void Init() {
      // Intiailize events.
      Events.Init();

      // Initialize game loop and render loop.
      Task.Run(Render);

      // Task.Run(Update);
      // Task.Run(UpdateVM);
    
    }

    public static async Task Render() {
      for(;;) {
        await LegendOfWorlds.Utils.Render.clearRootCanvas();
         
        foreach(System system in renderSystems) {
          await system.action();
        }
        
        await Task.Delay(8);
      }
    }

    public static async Task Update() {
      /*
      for(;;) {
        
        systems.ForEach((System system) => {
          await system.action();
        });
        await Task.Delay(16);
      }
      */
    }

    public static async Task UpdateVM() {
      /*
      for(;;) {
        systems.ForEach((System system) => {
          system.action();
        });
        await Task.Delay(16);
      }
      */
    }

  }
}

