using static LegendOfWorlds.Engine.World; 
using System.Threading.Tasks;
using System;
using Audrey;

namespace LegendOfWorlds.Engine.Ecs {
  public partial class Systems {
    public static async Task GlobalRenderSystem() {
      await Utils.Render.clearRootCanvas();
      Family family = Family.All(typeof(RenderTargetsComponent), typeof(PositionComponent)).Get();
      ImmutableList<Entity> famEntities = engine.GetEntitiesFor(family);

      foreach(Entity entity in famEntities) {
        RenderTargetsComponent renderTargetsComponent = entity.GetComponent<RenderTargetsComponent>();
        PositionComponent positionComponent = entity.GetComponent<PositionComponent>();

        renderTargetsComponent.targets.ForEach(async (RenderTarget target) => {
          await Utils.Render.drawSingleTarget(target.targetId, positionComponent.x, positionComponent.y);
        });
      }
    }
  }
}
