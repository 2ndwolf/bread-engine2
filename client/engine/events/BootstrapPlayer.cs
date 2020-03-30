using static LegendOfWorlds.Engine.World;
using System;

namespace LegendOfWorlds.Engine {
  public static partial class Events {
    // Events.
    public static Action<object> BootstrapPlayer = (object _msg) => {
      string msg = (string) _msg;
      Console.WriteLine(msg);
    };
  }
}