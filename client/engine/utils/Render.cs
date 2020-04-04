using System;
using System.Threading.Tasks;

namespace LegendOfWorlds.Utils {
  public class Render {
    public static async Task createTexture(string textureId, string url) {
      await LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("createTexture", new string[] { textureId, url });
    }

    public static async Task deleteTexture(string textureId) {
      await LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("deletTexture", new string[] { textureId });
    }

    public static async Task createTarget(string targetId, float x, float y, float width, float height) {
      await LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("createTarget", new object[] { targetId, x, y, width, height });
    }

    public static async Task deleteTarget(string targetId) {
      await LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("deleteTarget", new string[] { targetId });
    }

    public static async Task drawOnTarget(string targetId, string textureId, float x, float y) {
      await LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("drawOnTarget", new object[] { targetId, textureId, x, y });
    }

    public static async Task setTargetPos(string targetId, float x, float y) {
      await LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("setTargetPos", new object[] { targetId, x, y });
    }

    public static async Task drawSingleTarget(string targetId) {
      await LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("drawSingleTarget", new string[] { targetId });
    }

    public static async Task drawAllTargets() {
      await LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("drawAllTargets", new string[] {});
    }

    public static async Task clearRootCanvas() {
      await LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("clearRootCanvas", new string[] {});
    }
    
  }
}