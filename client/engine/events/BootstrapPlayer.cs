using System;
using System.IO; // I think we can safely remove this one
using System.Threading.Tasks;

using Audrey;

using static Shared.Engine.World;
using static Shared.Ecs.Entities.Archetypes;
using LegendOfWorlds.Engine.Ecs;
using SharedEngine = Shared.Engine;


namespace LegendOfWorlds.Engine {
  public static partial class Events {
    // Events.
    public static Action<object> BootstrapPlayer = (object _msg) => {
      Task.Run(async () => {

        // ECS stuff
        // Entity entity = World.engine.CreateEntity();
        // await GaniCharacter(entity).AddChatText().AddNickname();

      });
    };
  }
}