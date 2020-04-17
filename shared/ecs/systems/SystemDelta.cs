using System;

namespace Shared.Engine.Ecs {
  public partial class Systems {
    private static long lastTime = Environment.TickCount;
    public static long deltaTime = 0;

    public static void ComputeDeltaTime(){
      long currentTick = Environment.TickCount;
      deltaTime = currentTick - lastTime;
      lastTime = currentTick;
    }
  }
}