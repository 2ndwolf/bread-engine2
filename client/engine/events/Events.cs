using static LegendOfWorlds.Engine.World;
using System;

namespace LegendOfWorlds.Engine {
  public static partial class Events {
    // Util func.
    public static void AddEvent(Action<object> action) {
      World.EE.On(nameof(action), (Action<object>) action);
    }
    
    public static void EmitEvent(Action action, dynamic data) {
      World.EE.Emit(nameof(action), data);
    }
    
    public static void Init() {
      AddEvent(BootstrapPlayer);
      EmitEvent(BootstrapPlayer, "Hello world!");
    }

  }
}