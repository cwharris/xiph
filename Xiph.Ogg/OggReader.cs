using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Xiph.Ogg
{
    public class OggReader
    {
        private readonly byte[] _readBuffer = new byte[65307];

        private readonly FileStream _stream;

        public OggReader(FileStream stream)
        {
            _stream = stream;
        }

        public async Task<OggPageHeader> ReadPageHeader()
        {
            var byteReadCount1 = await _stream.ReadAsync(_readBuffer, 0, 27);

            if (byteReadCount1 == 0)
            {
                return null;
            }

            if (byteReadCount1 != 27)
            {
                throw new InvalidOperationException("unexpected end of stream.");
            }

            if (!_readBuffer.VerifySpan(0, "OggS"))
            {
                throw new InvalidOperationException("unexpected signature.");
            }

            if (_readBuffer[4] != 0)
            {
                throw new InvalidOperationException("unexpected version.");
            }

            var pageHeaderType = (OggPageHeaderType) _readBuffer[5];
            var granulePosition = BitConverter.ToInt64(_readBuffer, 6);
            var bitstreamSerialNumber = BitConverter.ToInt32(_readBuffer, 14);
            var pageSequenceNumber = BitConverter.ToInt32(_readBuffer, 18);

            // TODO(cwharris): verify crc checksum
            var crcChecksum = BitConverter.ToInt32(_readBuffer, 22);

            var numberPageSegments = (int) _readBuffer[26];

            var byteReadCount2 = await _stream.ReadAsync(_readBuffer, 0, numberPageSegments);

            if (byteReadCount2 != numberPageSegments)
            {
                throw new FormatException();
            }

            var packetSizes = new List<int>();
            var packetSizesFinalized = 0;

            for (var i = 0; i < numberPageSegments; i++)
            {
                var lacingValue = _readBuffer[i];

                if (packetSizes.Count == packetSizesFinalized)
                {
                    packetSizes.Add(0);
                }

                packetSizes[packetSizesFinalized] += lacingValue;

                if (lacingValue < 255)
                {
                    packetSizesFinalized++;
                }
            }

            var lastPacketContinues = _readBuffer[numberPageSegments] >= 255;

            var size = packetSizes.Aggregate(0, (a, b) => a + b);

            var byteReadCount3 = _stream.Read(_readBuffer, 0, size);

            if (byteReadCount3 != size)
            {
                throw new Exception("unexpected end of stream.");
            }

            return new OggPageHeader(
                pageHeaderType,
                granulePosition,
                bitstreamSerialNumber,
                pageSequenceNumber,
                packetSizes.ToArray(),
                lastPacketContinues,
                _stream.Position);
        }
    }
}
