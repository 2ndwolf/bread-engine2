using static LegendOfWorlds.Engine.World;

namespace LegendOfWorlds.Engine.Ecs {
  public partial class Systems {
    public static void Init() {
      // Game logic systems
      World.systems.Add(System.create(RenderSystem));

      // Render systems
      World.renderSystems.Add(System.create(RenderSystem));
    }
  }
} 