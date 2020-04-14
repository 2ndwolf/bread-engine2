using FlatSharp.Attributes;
using Audrey;

namespace Shared.Ecs.Components.Map {
    [FlatBufferTable]
    public class World : object, IComponent
    {
        [FlatBufferItem(0)] public virtual string name { get; set; }
        [FlatBufferItem(1)] public virtual int width { get; set; }
        [FlatBufferItem(2)] public virtual int height { get; set; }
        [FlatBufferItem(3)] public virtual Floor[] floors { get; set; }
    }

    [FlatBufferTable]
    public class Floor : object, IComponent
    {
        [FlatBufferItem(0)] public virtual string name { get; set; }
        [FlatBufferItem(1)] public virtual Cell[] cells { get; set; }
    }

    public class Cell : object, IComponent
    {
        [FlatBufferItem(0)] public virtual string name { get; set; }
        [FlatBufferItem(1)] public virtual Layer[] layers { get; set; }
    }

    [FlatBufferTable]
    public class Layer : object, IComponent
    {
        [FlatBufferItem(0)] public virtual string name { get; set; }
        [FlatBufferItem(1)] public virtual int[] tileIds { get; set; }
    }
}