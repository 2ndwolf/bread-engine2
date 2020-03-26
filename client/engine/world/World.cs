using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blazor.Extensions.Canvas.Canvas2D;
using Leopotam.Ecs;
using FluentIL;

namespace LegendOfWorlds.Engine.World {
  public class Position : Audrey.IComponent {
      public float x;
      public float y;
  }

  public class World {
    // ECS
    public static Audrey.Engine engine = new Audrey.Engine();

    // Rendering
    public static Canvas2DContext baseCanvas;
    public static Dictionary<int, Canvas2DContext> canvases;

    public static void Init(Canvas2DContext _baseCanvas) {
      // Initialize Canvas
      baseCanvas = _baseCanvas;
      canvases = new Dictionary<int, Canvas2DContext>();

      var typeBuilder = TypeFactory
          .Default
          .NewType("Health")
          .Class()
          .Implements<Audrey.IComponent>()
          .Public();

      var field = typeBuilder
          .NewField<int>("points")
          .Private();

      var property = typeBuilder
          .NewProperty<int>("Points")
          .Setter(m => m
              .Private()
              .Body()
              .LdArg0()
              .LdArg1()
              .StFld(field)
              .Ret())
          .Getter(m => m
              .Public()
              .Body()
              .LdArg0()
              .LdFld(field)
              .Ret());

      var setPoints = typeBuilder
          .NewMethod("SetPoints")
          .Public()
          .Param<int>("points")
          .Body(m => m
              .LdArg0()
              .LdArg1()
              .Call(property.SetMethod)
              .Ret());

      var type = typeBuilder.CreateType();
      var obj = (Audrey.IComponent) Activator.CreateInstance(type);

      obj.SetPropertyValue("Points", 300);

      var entity = engine.CreateEntity();
      entity.AddComponent(new Position());
      entity.AddComponent(obj);

      Console.WriteLine(engine.GetEntities()[0].GetComponent(type).GetPropertyValue("Points"));

      // Initialize game loop and render loop.
      Task.Run(Update);
    }

    public static async Task Update() {
      for(;;)
      {
        await Task.Delay(16);
      }
    }
  }
}

