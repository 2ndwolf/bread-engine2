using Audrey;
using MessagePack;

namespace Shared.Ecs.Components.GameObject {

  [MessagePackObject]
  public class PositionComponent : object, IComponent {
    [Key(0)] public float x { get; set; }
    [Key(1)] public float y { get; set; }

    public PositionComponent(int x, int y){
      this.x = (float) x;
      this.y = (float) y;
    }
  }

  [MessagePackObject]
  public class BoundsComponent : object, IComponent {
    [Key(0)] public int width { get; set; }
    [Key(1)] public int height { get; set; }

    public BoundsComponent(int width, int height){
      this.width = width;
      this.height = height;
    }
  }

  [MessagePackObject]
  public class CollisionBoxComponent : object, IComponent {
    [Key(0)] public PositionComponent offset { get; set; }
    [Key(1)] public BoundsComponent bounds { get; set; }

    public CollisionBoxComponent(PositionComponent offset, BoundsComponent bounds){
      this.offset = offset;
      this.bounds = bounds;
    }
  }

  [MessagePackObject]
  public class TextComponent : object, IComponent {
    [Key(0)] public string content {get; set;}
  }

  [MessagePackObject]
  public class ChatComponent : object, IComponent {
    [Key(0)] public TextComponent text {get; set;}
    [Key(1)] public PositionComponent offset {get; set;}
  }

  [MessagePackObject]
  public class NicknameComponent : object, IComponent {
    [Key(0)] public TextComponent text {get; set;}
    [Key(1)] public PositionComponent offset {get; set;}
  }
}