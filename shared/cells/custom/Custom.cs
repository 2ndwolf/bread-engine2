using FlatSharp.Attributes;

namespace Shared.Cells.Custom {
    [FlatBufferTable]
    public class World : object
    {
        [FlatBufferItem(0)] public virtual string name { get; set; }
        [FlatBufferItem(1)] public virtual int width { get; set; }
        [FlatBufferItem(2)] public virtual int height { get; set; }
        [FlatBufferItem(3)] public virtual Cell[] cells { get; set; }
    }

    [FlatBufferTable]
    public class Cell : object
    {
        [FlatBufferItem(0)] public virtual string name { get; set; }
        [FlatBufferItem(1)] public virtual int[] tileIds { get; set; }
    }
}