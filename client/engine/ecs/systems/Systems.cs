using static LegendOfWorlds.Engine.World;

namespace LegendOfWorlds.Engine.Ecs {
  public partial class Systems {
    public static void Init() {
      // Game logic systems

      // Render systems
      World.renderSystems.Add(System.create(RenderSystem));
    }
  }
} 