using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blazor.Extensions.Canvas.WebGL;
using System.Runtime.InteropServices;
using eeNet;
using LegendOfWorlds.Engine.Ecs;
using Microsoft.JSInterop;


//using Microsoft.JSInterop;

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

    public static async Task Init(WebGLContext baseContext) {
      //Image.Image img = Loaders.Loaders.OpenImageFile("https://localhost:5001/assets/images/body.png").Result;
      // Initialize Canvas
      GL = baseContext;
      //canvases = new Dictionary<int, WebGLContext>();
      //canvases.Add(-1, GL);

      // Initialize systems.
      Systems.Init();

      // Intiailize events.
      Events.Init();

      // Initialize game loop and render loop.
      //Task.Run(Update);
      //Task.Run(UpdateVM);
      await RenderTest();
      Task.Run(Render);
    }

    public static async Task Render() {
      for(;;) {
        // renderSystems.ForEach((System system) => {
        //   goRender();
        // });
        // await Task.Delay(32);
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
