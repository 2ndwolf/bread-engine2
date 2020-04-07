using System;
using System.Threading.Tasks;

namespace LegendOfWorlds.Utils {
  public class Render {
    public static ValueTask<string> createTexture(string textureId, string url) {
      return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("createTexture", new string[] { textureId, url });
    }

    public static ValueTask<string> deleteTexture(string textureId) {
      return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("deletTexture", new string[] { textureId });
    }

    public static ValueTask<string> createTarget(string targetId, float x, float y, float width, float height) {
      return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("createTarget", new object[] { targetId, x, y, width, height });
    }

    public static ValueTask<string> deleteTarget(string targetId) {
      return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("deleteTarget", new string[] { targetId });
    }

    public static ValueTask<string> drawOnTarget(string targetId, string textureId, float x, float y) {
      return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("drawOnTarget", new object[] { targetId, textureId, x, y });
    }

    public static ValueTask<string> drawSingleTarget(string targetId, float x, float y) {
      return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("drawSingleTarget", new object[] { targetId, x, y });
    }

    public static ValueTask<string> clearRootCanvas() {
      return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("clearRootCanvas", null);
    }
    
  }
}