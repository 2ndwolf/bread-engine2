using System;
using System.IO; // I think we can safely remove this one
using System.Threading.Tasks;

using Audrey;

using static LegendOfWorlds.Engine.World; 
using static LegendOfWorlds.Utils.Render;
using LegendOfWorlds.Engine.Ecs;

namespace LegendOfWorlds.Engine {
  public static partial class Events {
    // Events.
    public static Action<object> BootstrapPlayer = (object _msg) => {
      Task.Run(async () => {
        Guid textureId = Guid.NewGuid();
        Guid targetId = Guid.NewGuid();

        await CreateRenderTarget(targetId);

        await CreateTexture(textureId, "body.png");

        TexToTarget(textureId, targetId);

        // JS stuff
        // await Utils.Render.createTexture(textureId, "https://localhost:5001/assets/doll.png");
        // await Utils.Render.createTarget(targetId, 0, 0, 1920, 1080);
        // await Utils.Render.drawOnTarget(targetId, textureId, 0, 0);

        // ECS stuff
        Entity entity = engine.CreateEntity();
        RenderTargetsComponent targetsComponent = new RenderTargetsComponent();
        PositionComponent positionComponent = new PositionComponent();

        targetsComponent.targets.Add(new RenderTarget() { 
          targetId=targetId, 
          textureId=textureId
        });
        
        positionComponent.x = 0;
        positionComponent.y = 0;

        entity.AddComponent(targetsComponent);
        entity.AddComponent(positionComponent);

          // Initialize systems.
        Systems.Init();
      });
    };
  }
}