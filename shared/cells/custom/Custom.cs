using MessagePack;

namespace Shared.Cells.Custom {
    [MessagePackObject]
    public class World : object
    {
        [Key(0)] public virtual string name { get; set; }
        [Key(1)] public virtual int width { get; set; }
        [Key(2)] public virtual int height { get; set; }
        [Key(3)] public virtual Cell[] cells { get; set; }
    }

    [MessagePackObject]
    public class Cell : object
    {
        [Key(0)] public virtual string name { get; set; }
        [Key(1)] public virtual int[] tileIds { get; set; }
    }
}
