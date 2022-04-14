using Microsoft.AspNetCore.Http;

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
    }
}
