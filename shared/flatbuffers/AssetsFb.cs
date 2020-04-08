using FlatSharp;
using FlatSharp.Attributes;
using FlatSharp.TypeModel;

namespace Shared.FlatBuffers {
   [FlatBufferTable]
    public class LoWImage : object
    {   
        [FlatBufferItem(0)] public virtual int width { get; set; }
        [FlatBufferItem(1)] public virtual int height { get; set; }
        [FlatBufferItem(2)] public virtual byte[] data { get; set; }
    }
}