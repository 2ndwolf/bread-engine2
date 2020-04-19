using System;

using System.Collections.Generic;
using MessagePack;
using Audrey;

using Shared.Ecs.Components.GameObject;
using Shared.Ecs.Components.Sprites;

namespace Shared.Ecs.Components.Render {

  #if (CLIENT || EDITOR)
  public struct RenderTarget {
    public Guid targetId;
    public Guid textureId;
    public SpriteComponent spriteComponent;
    public TargetState targetState;
    public string targetName;
    public bool visible;
  }

  public class RenderTargetsComponent : IComponent {
    public List<RenderTarget> targets = new List<RenderTarget>();
  }
  #endif

  [MessagePackObject]
  public class TargetState: object{
    [Key(0)] public virtual int currentState {get; set;}
    [Key(1)] public virtual int currentFrame {get; set;}
    [Key(2)] public virtual bool play {get; set;}
    [Key(3)] public virtual bool reversed {get; set;}
    [Key(4)] public virtual PositionComponent offset {get; set;}
    // Populated when sending data over network
    [Key(5)] public virtual string targetName {get; set;}

    public TargetState(PositionComponent offset){
      this.offset = offset;
      this.currentState = 0;
      this.currentFrame = 0;
      this.play = true;
      this.reversed = false;
      this.targetName = "NotSet";
    }
    public TargetState(PositionComponent offset, int currentState, int currentFrame){
      this.offset = offset;
      this.currentState = currentState;
      this.currentFrame = currentFrame;
      this.play = true;
      this.reversed = false;
      this.targetName = "NotSet";
    }
    public TargetState(PositionComponent offset, int currentState, int currentFrame, bool play, bool reversed){
      this.offset = offset;
      this.currentState = currentState;
      this.currentFrame = currentFrame;
      this.play = play;
      this.reversed = reversed;
      this.targetName = "NotSet";
    }

  }

}

/// <summary>
/// Create a new <see cref="Pixel"/>.
/// </summary>
/// <param name="r">The red value for the pixel.</param>
/// <param name="g">The green value for the pixel.</param>
/// <param name="b">The blue value for the pixel.</param>
/// <param name="a">The alpha transparency value for the pixel.</param>
/// <param name="isGrayscale">Whether the pixel is grayscale.</param>