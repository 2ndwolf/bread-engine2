using System;
using static Shared.Engine.World;
// using static Shared.Ecs.Systems;

namespace Shared.Engine.Ecs {
  public partial class Systems {
    public static void Init() {
      // Game logic systems
      World.systems.Add(System.create(WaitSystem));

      // Render systems
      // World.renderSystems.Add(System.create(GlobalRenderSystem));
    }
  }
} 