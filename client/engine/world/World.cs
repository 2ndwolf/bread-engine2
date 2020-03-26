using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blazor.Extensions.Canvas.Canvas2D;
using FluentIL;
using MessagePack;
using Janus;

namespace LegendOfWorlds.Engine.World {
  [MessagePackObject]
  public class MyClass
  {
      // Key attributes take a serialization index (or string name)
      // The values must be unique and versioning has to be considered as well.
      // Keys are described in later sections in more detail.
      [Key(0)]
      public int Age { get; set; }

      [Key(1)]
      public string FirstName { get; set; }

      [Key(2)]
      public string LastName { get; set; }

      // All fields or properties that should not be serialized must be annotated with [IgnoreMember].
      [IgnoreMember]
      public string FullName { get { return FirstName + LastName; } }
  }

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


      var mc = new MyClass
        {
            Age = 99,
            FirstName = "hoge",
            LastName = "huga",
        };
      // Call Serialize/Deserialize, that's all.
      byte[] bytes = MessagePackSerializer.Serialize(mc);
      dynamic mc2 = MessagePackSerializer.Deserialize<dynamic>(bytes);

      // You can dump msgpack binary blobs to human readable json.
      // Using indexed keys (as opposed to string keys) will serialize to msgpack arrays,
      // hence property names are not available.
      // [99,"hoge","huga"]
      var json = MessagePackSerializer.ConvertToJson(bytes);
      Console.WriteLine(json);
      Console.WriteLine(mc2);


			Timeline<float> a = new Timeline<float>("x");
      Timeline<float> a1 = new Timeline<float>("y");
      Timeline<float> a2 = new Timeline<float>("z");

      Console.WriteLine(a);

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

