using static Shared.Engine.World;
using static Shared.Utils.Render;

using Shared.Ecs.Components.GameObject;
using Shared.Ecs.Components.Render;

using System.Threading.Tasks;
using System;
using Audrey;

namespace LegendOfWorlds.Engine.Ecs {
  public partial class RenderSystems {
    public static async Task GlobalRenderSystem() {

      // await Utils.Render.clearRootCanvas();
      Family family = Family.All(typeof(RenderTargetsComponent), typeof(PositionComponent)).Get();
      ImmutableList<Entity> famEntities = engine.GetEntitiesFor(family);

      foreach(Entity entity in famEntities) {
        RenderTargetsComponent renderTargetsComponent = entity.GetComponent<RenderTargetsComponent>();
        PositionComponent positionComponent = entity.GetComponent<PositionComponent>();

        renderTargetsComponent.targets.ForEach(async (RenderTarget target) => {
          unchecked {
          await DrawSingleTarget(target.targetId, (int)positionComponent.x, (int)positionComponent.y);
          }
        });
      }
    }
  }
}
