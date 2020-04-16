using MessagePack;
using Audrey;

namespace Shared.Ecs.Components.Map {
    [MessagePackObject]
    public class World : object, IComponent
    {
        [Key(0)] public virtual string name { get; set; }
        [Key(1)] public virtual int width { get; set; }
        [Key(2)] public virtual int height { get; set; }
        [Key(3)] public virtual Floor[] floors { get; set; }
    }

    [MessagePackObject]
    public class Floor : object, IComponent
    {
        [Key(0)] public virtual string name { get; set; }
        [Key(1)] public virtual Cell[] cells { get; set; }
    }

    [MessagePackObject]
    public class Cell : object, IComponent
    {
        [Key(0)] public virtual string name { get; set; }
        [Key(1)] public virtual Layer[] layers { get; set; }
    }

    [MessagePackObject]
    public class Layer : object, IComponent
    {
        [Key(0)] public virtual string name { get; set; }
        [Key(1)] public virtual int[] tileIds { get; set; }
    }
}
