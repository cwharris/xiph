using System.Linq;

namespace Xiph.Ogg
{
    public static class ByteArrayExtensions
    {
        public static bool VerifySpan(this byte[] buffer, int offset, string span)
        {
            return !span.Where((c, index) => buffer[offset + index] != c).Any();
        }
    }
}
