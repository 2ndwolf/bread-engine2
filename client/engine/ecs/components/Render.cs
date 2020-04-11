using System.Collections.Generic;
using Audrey;

namespace LegendOfWorlds.Engine.Ecs {
  public struct RenderTarget {
    public string targetId;
    public string textureId;
  }

  public class RenderTargetsComponent : IComponent {
    public List<RenderTarget> targets = new List<RenderTarget>();
  }

  public class TexturesComponent : IComponent {
    public List<string> textures = new List<string>();
  }
}