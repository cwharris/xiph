using System;
using System.IO;
using System.Threading.Tasks;
using Xiph.Ogg;

namespace Xiph.App.TestConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "valid.opus");

            using (var stream = File.OpenRead(path))
            {
                var reader = new OggReader(stream);

                var i = 0;
                OggPageHeader pageHeader;

                while ((pageHeader = await reader.ReadPageHeader()) != null)
                {
                    Console.WriteLine("{0}\t{1}", i++, pageHeader.PageHeaderType);
                }
            }

            Console.ReadLine();
        }
    }
}