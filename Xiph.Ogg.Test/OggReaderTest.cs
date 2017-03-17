using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Xiph.Ogg.Test
{
    public class OggReaderTest
    {
        [Fact]
        public async Task OggReaderCanReadFromStream()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "valid.opus");

            using (var stream = File.OpenRead(path))
            {
                var reader = new OggReader(stream);

                OggPageHeader pageHeader;

                while ((pageHeader = await reader.ReadPageHeader()) != null)
                {
                    Console.WriteLine(pageHeader.PageHeaderType);
                }
            }
        }
    }
}
