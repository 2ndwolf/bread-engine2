using System;
using static Shared.Engine.World;

namespace LegendOfWorlds.Engine {
  public static partial class Events {
    // Util func.
    public static void AddEvent(Action<object> action) {
      EE.On(nameof(action), action);
    }
    
    public static void EmitEvent(Action<object> action, object data) {
      EE.Emit(nameof(action), data);
    }
    
    public static void Init() {
      AddEvent(BootstrapPlayer);

      EmitEvent(BootstrapPlayer, "Hello world!");
    }
 
  }
}