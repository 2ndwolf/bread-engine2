using System;
using static Shared.Engine.World;

using System.Threading.Tasks;
using Audrey;

using Shared.Ecs.Components.GameObject;
using Shared.Ecs.Components.Flag;
using Shared.Ecs.Components.Wait;

#if (CLIENT || EDITOR)
using Shared.Ecs.Components.Render;
using static Shared.Utils.Render;
#endif

/**

  Builds a Gani like character

**/

namespace Shared.Ecs.Entities{

  public static partial class Archetypes{

    public static async Task<Entity> GaniCharacter(Entity entityPart){

      #if (CLIENT || EDITOR)

      if(!entityPart.HasComponent<RenderTargetsComponent>())
        entityPart.AddComponent(new RenderTargetsComponent());

      RenderTargetsComponent targetsComponent = entityPart.GetComponent<RenderTargetsComponent>();
      // TimedSpriteComponent timedSpriteComponent = entityPart.GetComponent<TimedSpriteComponent>();

      // Build the character's visuals
      (Guid shadowTex, Guid shadowRT) = await TexToTarget(
        await CreateTexture(Guid.NewGuid(), "sprites.png"),
        await CreateRenderTarget(Guid.NewGuid()));

      (Guid bodyTex, Guid bodyRT) = await TexToTarget(
        await CreateTexture(Guid.NewGuid(), "body.png"),
        await CreateRenderTarget(Guid.NewGuid()));

      (Guid headTex, Guid headRT) = await TexToTarget(
        await CreateTexture(Guid.NewGuid(), "head19.png"),
        await CreateRenderTarget(Guid.NewGuid()));

      targetsComponent.targets.Add(new RenderTarget() { 
        targetId=shadowRT, 
        textureId=shadowTex,
        targetName="shadow",
        visible=true
      });

      targetsComponent.targets.Add(new RenderTarget() { 
        targetId=bodyRT, 
        textureId=bodyTex,
        targetName="body",
        visible=true
      });
      
      targetsComponent.targets.Add(new RenderTarget() { 
        targetId=headRT, 
        textureId=headTex,
        targetName="head",
        visible=true
      });

      // targetsComponent.targets.Add(new RenderTarget() { 
      //   targetId=headRT,
      //   textureId=headTex,
      //   targetName="hat",
      //   visible=false
      // });
      #endif


      if(!entityPart.HasComponent<PositionComponent>())
        entityPart.AddComponent(new PositionComponent(0,0));
      if(!entityPart.HasComponent<TimedSpriteComponent>())
        entityPart.AddComponent(new TimedSpriteComponent());
      //TODO CustomGaniComponent -> contains the customized gani properties (head, body, colors)...

      PositionComponent positionComponent = entityPart.GetComponent<PositionComponent>();

      return entityPart;
    }

  }

}