using static Shared.Engine.World;
using static LegendOfWorlds.Engine.World;
using SharedEngine = Shared.Engine;
// using static Shared.Ecs.Systems;

namespace LegendOfWorlds.Engine.Ecs {
  public partial class RenderSystems {
    public static void Init() {
      // Game logic systems
      // World.systems.Add(System.create(WaitSystem));

      // Render systems
      LegendOfWorlds.Engine.World.renderSystems.Add(SharedEngine.System.create(GlobalRenderSystem));
    }
  }
} 