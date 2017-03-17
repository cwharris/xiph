using System;

namespace Xiph.Ogg
{
    [Flags]
    public enum OggPageHeaderType : byte
    {
        None = 0,
        ContinuedPacket = 1,
        BeginingOfStream = 2,
        EndOfStream = 4
    }
}
