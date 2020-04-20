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

      if(!entityPart.HasComponent<RenderablesComponent>())
        entityPart.AddComponent(new RenderablesComponent());

      RenderablesComponent targetsComponent = entityPart.GetComponent<RenderablesComponent>();
      // TimedSpriteComponent timedSpriteComponent = entityPart.GetComponent<TimedSpriteComponent>();

      // Build the character's visuals
      Task<Guid> shadowTask = TexToTarget(
        await UseTexture("sprites.png"),
        await CreateRenderTarget(Guid.NewGuid()));

      Task<Guid> bodyTask = TexToTarget(
        await UseTexture("body.png"),
        await CreateRenderTarget(Guid.NewGuid()));

      Task<Guid> headTask = TexToTarget(
        await UseTexture("head19.png"),
        await CreateRenderTarget(Guid.NewGuid()));

      Task<Guid> hatTask = TexToTarget(
        await UseTexture("hat0.png"),
        await CreateRenderTarget(Guid.NewGuid()));

      Task<Guid> shieldTask = TexToTarget(
        await UseTexture("shield1.png"),
        await CreateRenderTarget(Guid.NewGuid()));

      Guid shadow = await shadowTask;
      Guid body = await bodyTask;
      Guid head = await headTask;
      Guid hat = await hatTask;
      Guid shield = await shieldTask;


      // targetsComponent.targets.Add(new Renderable() { 
      //   targetId=shadow, 
      //   targetName="shadow",
      //   visible=true,
      //   layer=0
      // });

      SpriteComponent bodySprite = new SpriteComponent(
        [32,32],[0,0],1,11,
        [[1],[4],[3],[3],[1],[1],[1],[3],[1],[1],[1]])
      
      bodySprite.hasStates = true;
      bodySprite.loop = -1;
      bodySprite.leftToRight = false;
      bodySprite.frameDurations = {.2f};

      RenderableStateComponent bodyStates = 
      new RenderableStateComponent(new PositionComponent(0,32));

      targetsComponent.targets["body"] = new Renderable() {
        spriteComponent = bodySprite,
        targetState = bodyStates,
        targetId=body, 
        visible=true,
        layer=1,
        x=10,
        y=10
      };

      
      
      // targetsComponent.targets.Add(new Renderable() { 
      //   targetId=head, 
      //   targetName="head",
      //   visible=true,
      //   layer=2,
      //   x=100,
      //   y=100
      // });

      // targetsComponent.targets.Add(new Renderable() { 
      //   targetId=hat,
      //   targetName="hat",
      //   visible=false,
      //   layer=3
      // });

      // targetsComponent.targets.Add(new Renderable() { 
      //   targetId=shield,
      //   targetName="shield",
      //   visible=false,
      //   layer=4
      // });

      // targetsComponent.targets.Sort();
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