using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClientWebApp.Common
{
    public static class ConvertApp
    {
        public static string ToBase64String(IFormFile image)
        {
            if (image==null)
            {
                return null;
            }
            byte[] fileBytes = null;
            if (image.Length == 0) return null;
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                fileBytes = ms.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }

        public static IFormFile ToIFormFile(string image)
        {
            if (image==null)
            {
                return null;
            }
            byte[] bytes = Convert.FromBase64String(image);
            MemoryStream stream = new MemoryStream(bytes);

            return new FormFile(stream, 0, bytes.Length, "Image", "Image");

        }


    }
}
