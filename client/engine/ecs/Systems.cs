using System;
using static LegendOfWorlds.Engine.World;

namespace LegendOfWorlds.Engine.Ecs {
  public class Systems {
    public static void Init() {
      World.systems.Add(new System(){ name="RenderSystem", action=RenderSystem });
    }

    public static void RenderSystem() {
      Console.WriteLine("HELLO WORLD!!!");
    }
  }
}