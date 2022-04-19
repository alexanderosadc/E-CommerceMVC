using Microsoft.AspNetCore.Http;
using System.IO.Compression;

namespace Ebay.Infrastructure.Helpers
{
    public static class FileHelper
    {
        public static byte[] TransformToBinary(IFormFile file)
        {
            byte[] fileBytes;

            if (file.Length <= 0)
            {
                throw new ArgumentException("File has 0 size.");
            }


            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return fileBytes;
        }

        public static byte[] Compress(byte[] buffer)
        {
            byte[] compressedByte;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Compress))
                {
                    ds.Write(buffer, 0, buffer.Length);
                }

                compressedByte = ms.ToArray();
            }

            return compressedByte;
        }
    }
}
