using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shared.Ecs.Components.Wait;
using Shared.Ecs.Components.Flag;


//If client side
using static Shared.Engine.World;
// using static Shared.Engine.Ecs.Systems;

using Audrey;

namespace Shared.Engine.Ecs {
  public partial class Systems {
    public static async Task WaitSystem() {

      // await Utils.Render.clearRootCanvas();

      Family family = Family.One(typeof(TimedSpriteComponent)).Get();
      ImmutableList<Entity> famEntities = engine.GetEntitiesFor(family);

      foreach(Entity entity in famEntities) {
        // WaitSpriteComponent waitSpriteComponent = entity.GetComponent<WaitSpriteComponent>();

        var wC = new List<WaitComponent>();

        if(entity.HasComponent<TimedSpriteComponent>()){
          // wC.Add(entity.GetComponent<TimedSpriteComponent>());
          //Get render target because it is which contains the timings
        }
        
        // if (entity.has(wait.InterpolationComponent)){
        //   wC.push(entity.get(wait.InterpolationComponent))
        // }

        // if(entity.has(wait.SpriteComponent)){
        //   wC.push(entity.get(wait.SpriteComponent))
        // }

        foreach(WaitComponent w in wC){
          
          if(w.waitState == WaitState.Standby && w.secondsToWait > 0){
            w.elapsedTime = deltaTime;
            w.waitState = WaitState.Wait;
          }
          
          if(w.waitState == WaitState.Done){
            w.waitState = WaitState.Standby;
            
          }else if(w.elapsedTime >= w.secondsToWait - deltaTime){ //<-- consider removing 1/60 to elapsedTime so the waiting frame doesn't count
            w.waitState = WaitState.Done;

          } else if (w.waitState == WaitState.Wait){
            w.elapsedTime += deltaTime;
          }
        }
      }
    }
  }
}

  //implementation would look like
  /*
    if(entity.has(wait.GaniComponent)){
      if(wC.state == wait.State.Wait) return
      if(wC.state == wait.State.Standby) {
        if(wC.runOnce) wC.secondsToWait = 0
        else return
      }
      //so that if wait.State.Done the logic runs
    }

    //rest of logic...

  */