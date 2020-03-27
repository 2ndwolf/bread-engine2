using static LegendOfWorlds.Engine.World;

namespace LegendOfWorlds.Engine.Ecs {
  public partial class Systems {
    public static void Init() {
      World.systems.Add(System.create(RenderSystem));
    }
  }
}