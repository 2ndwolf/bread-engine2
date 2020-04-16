using MessagePack; 

namespace Shared.FlatBuffers {
    [MessagePackObject]
    public class LoWImage
    {   
        [Key(0)]
        public virtual int width { get; set; }
        [Key(1)]
        public virtual int height { get; set; }
        [Key(2)]
        public virtual byte[] data { get; set; }
    }
}