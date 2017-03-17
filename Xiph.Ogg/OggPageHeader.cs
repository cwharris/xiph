namespace Xiph.Ogg
{
    public class OggPageHeader
    {
        public readonly OggPageHeaderType PageHeaderType;
        public readonly long GranulePosition;
        public readonly int BitstreamSerialNumber;
        public readonly int PageSequenceNumber;
        public readonly int[] PacketSizes;
        public readonly bool LastPacketContinues;
        public readonly long PacketStartPosition;

        public OggPageHeader(
            OggPageHeaderType pageHeaderType,
            long granulePosition,
            int bitstreamSerialNumber,
            int pageSequenceNumber,
            int[] packetSizes,
            bool lastPacketContinues,
            long packetStartPosition)
        {
            PacketStartPosition = packetStartPosition;
            LastPacketContinues = lastPacketContinues;
            PageHeaderType = pageHeaderType;
            GranulePosition = granulePosition;
            BitstreamSerialNumber = bitstreamSerialNumber;
            PageSequenceNumber = pageSequenceNumber;
            PacketSizes = packetSizes;
        }
    }
}
